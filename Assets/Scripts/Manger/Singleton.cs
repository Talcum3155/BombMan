using System;
using UnityEngine;

namespace Manger
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = (T) this;
            DontDestroyOnLoad(this);
        }
    }
}