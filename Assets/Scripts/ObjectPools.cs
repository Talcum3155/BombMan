using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPools : MonoBehaviour
{
    public static ObjectPools Instance;

    [Header("RunFX")]
    public GameObject runFXPrefab;
    public int runFxCount;
    public readonly Queue<GameObject> RunFxPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        
        FillPool();
    }

    private void FillPool()
    {
        for (var i = 0; i < runFxCount; i++)
        {
            ReturnPool(Instantiate(runFXPrefab));
        }
    }

    public void ReturnPool(GameObject o)
    {
        o.SetActive(false);
        RunFxPool.Enqueue(o);
    }

    public GameObject GetRunFXObject()
    {
        if (RunFxPool.Count == 0)
        {
            FillPool();
        }
        return RunFxPool.Dequeue();
    }
}
