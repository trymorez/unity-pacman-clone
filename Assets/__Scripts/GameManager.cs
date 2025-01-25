using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

public class GameManager : Singleton<GameManager>
{
    public static int Life = 3;
    public static int Score = 0;
    public static int HighScore = 0;
    public static int Level = 0;

    public static Action<GameState> OnBeforeGameStateChange;
    public static Action<GameState> OnAfterGameStateChange;
    public static Action OnGamePlaying;
    public static Action OnPowerUpFading;

    [SerializeField] float PowerUpTime = 6;
    [SerializeField] float PowerUpFadeTime = 3;
    [SerializeField] float PowerUpTimeSpent = 0;

    public static GameState State { get; private set; }
    [SerializeField] GameState initialState;
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

    void Start()
    {
        State = initialState;
        GameStateChange(State);
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
        OnBeforeGameStateChange?.Invoke(newState);
        BeforeGameStateChange(newState);

        State = newState;

        OnAfterGameStateChange?.Invoke(newState);
    }

    void BeforeGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Starting:
                break;
            case GameState.Playing:
                SoundManager.PlayLoop("Warning");
                break;
            case GameState.Paused:
                break;
            case GameState.PacmanPowerUp:
                SoundManager.StopLoop(SoundManager.loopingClip);
                SoundManager.PlayforDuration("PowerUp", PowerUpTime - 0.1f);
                PowerUpTimeSpent = 0;
                PowerUpFading = false;
                break;
            case GameState.PacmanDying:
                break;
            case GameState.LevelCompleted:
                break;
            case GameState.GameOver:
                break;
        }
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
            Debug.Log("back to playing");
        }
    }
}
