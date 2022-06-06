using System;
using Manger;
using UnityEngine;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        private void Awake()
        {
            if (UIManager.Instance.playerUI)
            {
                //如果UIManager中的PlayerUI已经注册过，就摧毁自身
                Destroy(gameObject);
                return;
            }
            UIManager.Instance.RegisterPlayerUI(gameObject);
        }
    }
}
