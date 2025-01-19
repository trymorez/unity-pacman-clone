using UnityEngine;

public class Home : MonoBehaviour
{
    public static Home Instance { get; private set; }

    public bool[] GhostInSlot = {true, true, true };
    public float[] SlotXPos = { -2, 0, 2};
    public float SlotExitYPos = -3.5f;

    void Awake()
    {
        Instance = this;
    }
}
