using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static int Life = 3;
    public static int Score = 0;
    public static int HighScore = 0;

    public static Action<GameState> OnBeforeGameStateChange;
    public static Action<GameState> OnAfterGameStateChange;
    public static Action OnGamePlaying;
    
    public GameState State { get; private set; }
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

        State = newState;

        OnAfterGameStateChange?.Invoke(newState);
    }

    void GamePlaying()
    {
        OnGamePlaying?.Invoke();
    }

    void GamePaused()
    {

    }
}
