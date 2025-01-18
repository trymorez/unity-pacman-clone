using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BeanManager : Singleton<BeanManager>
{
    public List<Bean> bean = new List<Bean>();

    void Start()
    {
        Debug.Log(bean.Count);
    }

    void Update()
    {
        
    }
}
