using UnityEngine;

public class Home : MonoBehaviour
{
    public static Home Instance { get; private set; }

    public bool[] SlotOpen;

    void Awake()
    {
        Instance = this;
    }
}
