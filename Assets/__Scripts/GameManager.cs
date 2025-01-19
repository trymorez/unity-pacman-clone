using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int Life {  get; private set; }
    public int Score {  get; private set; }
    public int HighScore {  get; private set; }

    public static Action<GameState> OnBeforeGameStateChange;
    public static Action<GameState> OnAfterGameStateChange;
    
    public GameState State { get; private set; }
    public enum GameState 
    { 
        Starting = 0,
        Started = 1,
        PacmanPowerUp = 2,
        PacmanDying = 3,
        LevelCompleted = 4,
        GameOver = 5,
    }
    public bool gameInProgress = true;

    void Start()
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
            case GameState.Started:
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
}
