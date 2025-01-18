using UnityEngine;

public class Bean : MonoBehaviour
{
    void Awake()
    {
        BeanManager.Instance.bean.Add(this);
    }

    void Start()
    {
        
    }

}
