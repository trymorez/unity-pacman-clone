using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static int Life = 2;
    public static int Score = 0;
    public static int HighScore = 0;
    public static int Level = 0;

    public static Action<GameState> OnBeforeGameStateChange;
    public static Action<GameState> OnAfterGameStateChange;
    public static Action OnGamePlaying;
    public static Action OnPowerUpFading;
    public static Action<Vector2, int> OnGhostEatenPoint;
    public static Action<int> OnScoreUpdate;
    public static Action<int> OnLifeUpdate;

    [SerializeField] float PowerUpTime = 6;
    [SerializeField] float PowerUpFadeTime = 3;
    [SerializeField] float PowerUpTimeSpent = 0;

    public static GameState State { get; private set; }
    [SerializeField] GameState initialState;
    int ghostEatCount = 0;
    int ghostEatInitialPoint = 200;

    public enum GameState 
    { 
        Starting,
        Playing,
        Paused,
        PacmanPowerUp,
        PacmanDying,
        LevelCompleted,
        GameOver,
    }
    public bool gameInProgress = true;
    bool PowerUpFading = false;

    protected override void Awake()
    {
        base.Awake();
        Ghost.OnGhostEaten += OnGhostEaten;
    }

    void Start()
    {
        State = initialState;
        OnLifeUpdate?.Invoke(Life);
        GameStateChange(State);
    }

    void OnDestroy()
    {
        Ghost.OnGhostEaten -= OnGhostEaten;
    }

    void OnGhostEaten(Vector2 pos)
    {
        int point = ghostEatInitialPoint;
        for (int i = 0; i < ghostEatCount; i++)
            point += point;
        Score += point;
        OnScoreUpdate?.Invoke(Score);
        ghostEatCount++;
        OnGhostEatenPoint?.Invoke(pos, point);
    }

    void Update()
    {
        switch (State)
        {
            case GameState.Starting:
                break;
            case GameState.Playing:
                GamePlaying();
                break;
            case GameState.Paused:
                GamePaused();
                break;
            case GameState.PacmanPowerUp:
                GamePowerUp();
                break;
            case GameState.PacmanDying:
                break;
            case GameState.LevelCompleted:
                break;
            case GameState.GameOver:
                break;
        }
    }

    public void GameStateChange(GameState newState)
    {
        BeforeGameStateChange(newState);
        OnBeforeGameStateChange?.Invoke(newState);

        State = newState;

        OnAfterGameStateChange?.Invoke(newState);
    }

    void BeforeGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                PlayIntroAndWait();
                break;
            case GameState.Playing:
                SoundManager.PlayLoop("Warning");
                break;
            case GameState.PacmanPowerUp:
                SoundManager.StopLoop();
                SoundManager.PlayforDuration("PowerUp", PowerUpTime - 0.1f);
                ghostEatCount = 0;
                PowerUpTimeSpent = 0;
                PowerUpFading = false;
                break;
            case GameState.PacmanDying:
                SoundManager.StopLoop();
                break;
        }
    }

    public void DecreaseLife()
    {
        if (--Life < 0)
        {
            Life = 2;
            Score = 0;
            Level = 0;
            SceneManager.LoadScene(0);
        }
        else
        {
            OnLifeUpdate?.Invoke(Life);
        }
    }

    void PlayIntroAndWait()
    {
        SoundManager.Play("Intro");
        StartCoroutine(WaitAndStartGame());
    }

    IEnumerator WaitAndStartGame()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(4.2f);
        Time.timeScale = 1;
        GameStateChange(GameState.Playing);
    }

    void GamePlaying()
    {
        OnGamePlaying?.Invoke();
    }

    void GamePaused()
    {

    }
        
    void GamePowerUp()
    {
        ProcessPowerUpTime();

        OnGamePlaying?.Invoke();
    }

    void ProcessPowerUpTime()
    {
        PowerUpTimeSpent += Time.deltaTime;
        if (PowerUpTimeSpent >= PowerUpTime && !PowerUpFading)
        {
            PowerUpFading = true;
            OnPowerUpFading?.Invoke();
            SoundManager.PlayforDuration("Fading", PowerUpFadeTime - 0.1f);
        }
        if (PowerUpTimeSpent >= PowerUpTime + PowerUpFadeTime)
        {
            GameStateChange(GameState.Playing);
        }
    }

    public void NextLevel()
    {
        Level++; 
        SceneManager.LoadScene(0);
    }
}
