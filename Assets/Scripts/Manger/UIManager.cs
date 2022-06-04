using UnityEngine;

namespace Manger
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject[] hearts;

        public void UpdateHealth(float currentHealth)
        {
            Debug.Log($"更新血量 {currentHealth}");
            for (var i = 0; i < 3 - currentHealth; i++)
            {
                hearts[i].SetActive(false);
            }
        }
    }
}