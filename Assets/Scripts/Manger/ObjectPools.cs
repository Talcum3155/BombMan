using System.Collections.Generic;
using UnityEngine;

namespace Manger
{
    public class ObjectPools : Singleton<ObjectPools>
    {
        [Header("RunFX")]
        public GameObject runFXPrefab;
        public int runFxCount;
        public readonly Queue<GameObject> RunFxPool = new Queue<GameObject>();

        protected override void Awake()
        {
            base.Awake();
            FillPool();
        }

        private void FillPool()
        {
            for (var i = 0; i < runFxCount; i++)
            {
                var runFX = Instantiate(runFXPrefab);
                ReturnPool(runFX);
                DontDestroyOnLoad(runFX);
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
}
