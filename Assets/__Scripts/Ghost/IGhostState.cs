using UnityEngine;

public interface IGhostState
{
    void Enter(Ghost ghost);
    void Execute(Ghost ghost);
    void Exit(Ghost ghost);
}
