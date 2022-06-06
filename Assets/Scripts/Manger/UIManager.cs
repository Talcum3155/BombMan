using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Manger
{
    public class UIManager : Singleton<UIManager>
    {
        public GameObject playerUI;

        [Header("HEALTH")] public GameObject[] hearts;
        public Slider bossHealthSlider;

        [Header("Menu")] public GameObject pauseMenu;
        public GameObject gameOverPanel;

        public void RegisterPlayerUI(GameObject o)
        {
            playerUI = o;
            playerUI.transform.GetChild(0).gameObject.SetActive(true);
            playerUI.transform.GetChild(1).gameObject.SetActive(true);
            playerUI.SetActive(false);
            DontDestroyOnLoad(playerUI);
        }

        public void RegisterBossHealthBar(float health)
        {
            bossHealthSlider.maxValue = health;
            bossHealthSlider.gameObject.SetActive(true);
        }

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

        #region 主菜单

        /// <summary>
        ///     开始新游戏，删除所有存档数据
        /// </summary>
        public void StartNewGame()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            playerUI.SetActive(true);
        }

        /// <summary>
        ///     继续游戏，加载人物的所有数据
        /// </summary>
        public void ContinueGameExecutor()
        {
            var allInfo = SaveManager.Instance.LoadAllInfo(SaveManager.Instance.allInfoStr);
            StartCoroutine(ContinueGame(allInfo.Health,
                allInfo.PlayerPosition, allInfo.SceneIndex));
        }

        private IEnumerator ContinueGame(float health, Vector3 position, int sceneIndex)
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
            var playerController = GameManager.Instance.player;
            playerController.transform.position = position;
            playerController.health = health;
            UpdateHealth(health);
            //启用玩家UI
            playerUI.SetActive(true);
        }

        #endregion

        #region UI事件
        
        /// <summary>
        ///     返回主菜单，保存人物所有数据
        /// </summary>
        public void BackToMainMenuExecutor()
        {
            StartCoroutine(BackToMainMenu());
        }

        private IEnumerator BackToMainMenu()
        {
            Time.timeScale = 1;
            var playerController = GameManager.Instance.player;
            //保存数据
            SaveManager.Instance.SaveAllInfo(playerController.health,
                playerController.transform.position,
                SceneManager.GetActiveScene().buildIndex,
                SaveManager.Instance.allInfoStr);
            yield return SceneManager.LoadSceneAsync(0);
            //禁用玩家UI
            playerUI.SetActive(false);
            //禁用暂停菜单和boss血条
            pauseMenu.SetActive(false);
            bossHealthSlider.gameObject.SetActive(false);
            //清空敌人列表，防止返回主菜单后敌人列表中依旧存有残留数据
            GameManager.Instance.enemies.Clear();
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            Debug.Log("暂停游戏");
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
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }

        /// <summary>
        /// GameOver菜单
        /// </summary>
        public void GameOverMenu() => gameOverPanel.SetActive(true);

        #endregion
    }
}