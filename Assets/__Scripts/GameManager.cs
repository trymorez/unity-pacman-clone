using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Life {  get; private set; }
    public int Score {  get; private set; }
    public int HighScore {  get; private set; }

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
        initialState = State;
    }

    void Update()
    {
        GameStateChange(GameState.Starting);
    }

    public void GameStateChange(GameState newState)
    {
        OnBeforeGameStateChange?.Invoke(newState);

        State = newState;
        switch (State)
        {
            case GameState.Starting:
                break;
            case GameState.Playing:
                GamePlaying();
                break;
            case GameState.Paused:
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

        OnAfterGameStateChange?.Invoke(newState);
    }

    void GamePlaying()
    {
        OnGamePlaying?.Invoke();
    }
}
