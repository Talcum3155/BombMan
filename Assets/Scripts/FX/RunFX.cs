using System;
using UnityEngine;

namespace FX
{
    public class RunFX : MonoBehaviour
    {
        public void ReturnPool() => ObjectPools.Instance.ReturnPool(gameObject);
    }
}
