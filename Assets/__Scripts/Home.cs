using UnityEngine;

public class Home : MonoBehaviour
{
    public static Home Instance { get; private set; }

    public bool[] GhostInSlot = { true, true, true };
    public float[] SlotXPos = { -2, 0, 2};
    public float SlotExitYPos = -3.5f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.OnBeforeGameStateChange += OnBeforeGameStateChange;
    }

    void OnDestroy()
    {
        GameManager.OnBeforeGameStateChange -= OnBeforeGameStateChange;
    }

    void OnBeforeGameStateChange(GameManager.GameState State)
    {
        switch (State)
        {
            case GameManager.GameState.PacmanDying:
                ResetGhostSlot();
                break;
        }
    }

    void ResetGhostSlot()
    {
        for (var i = 0; i < GhostInSlot.Length; ++i)
            GhostInSlot[i] = true;
    }
    
}