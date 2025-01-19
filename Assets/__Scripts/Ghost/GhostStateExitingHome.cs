using UnityEngine;

public class GhostStateExitingHome : IGhostState
{
    public void Enter(Ghost ghost)
    {
        Debug.Log("Ghost start exiting home");
    }

    public void Execute(Ghost ghost)
    {
        Debug.Log("Ghost exiting home");
    }

    public void Exit(Ghost ghost)
    {
        Debug.Log("Ghost exited home");
    }
}
