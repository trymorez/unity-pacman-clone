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
    [Serializable] public enum GameState 
    { 
        Starting = 0,
        Started = 1,
        PowerUp = 2,
        Dieing = 3,
        LevelCleared = 4,
        GameOver = 5,
    }

    void Start()
    {
        
    }

    public void GameStateChange(GameState newState)
    {
        OnBeforeGameStateChange!.Invoke(newState);

        OnAfterGameStateChange!.Invoke(newState);
    }
}
