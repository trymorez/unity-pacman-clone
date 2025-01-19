using UnityEngine;

public class GhostStatetInHome : IGhostState
{
    public void Enter(Ghost ghost)
    {
        Debug.Log("Ghost in home");
    }

    public void Execute(Ghost ghost)
    {
        Debug.Log("Ghost acting in home");
    }

    public void Exit(Ghost ghost)
    {
        Debug.Log("Ghost exiting home");
    }
}
