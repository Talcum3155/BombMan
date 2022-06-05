using System;
using Manger;
using UnityEngine;

namespace FX
{
    public class RunFX : MonoBehaviour
    {
        public void ReturnPool() => ObjectPools.Instance.ReturnPool(gameObject);
    }
}
