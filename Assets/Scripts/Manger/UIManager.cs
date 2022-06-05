using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manger
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("HEALTH")] public GameObject[] hearts;
        public Slider bossHealthSlider;

        [Header("Menu")] public GameObject pauseMenu;
        public GameObject gameOverPanel;

        #region 血条控制

        /// <summary>
        /// 根据当前生命值更新血量UI
        /// </summary>
        /// <param name="currentHealth"></param>
        public void UpdateHealth(float currentHealth)
        {
            switch (currentHealth)
            {
                case 1:
                    hearts[0].SetActive(false);
                    hearts[1].SetActive(false);
                    hearts[2].SetActive(true);
                    break;
                case 2:
                    hearts[0].SetActive(false);
                    hearts[1].SetActive(true);
                    hearts[2].SetActive(true);
                    break;
                case 3:
                    hearts[0].SetActive(true);
                    hearts[1].SetActive(true);
                    hearts[2].SetActive(true);
                    break;
            }
        }

        #endregion

        #region UI事件

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        /// <summary>
        /// 重新开始游戏
        /// </summary>
        public void RetryGame()
            => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        /// <summary>
        /// GameOver菜单
        /// </summary>
        public void GameOverMenu() => gameOverPanel.SetActive(true);

        #endregion
    }
}