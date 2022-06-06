using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manger
{
    public class SaveManager : Singleton<SaveManager>
    {
        public string healthStr = "PlayerHealth";
        public string positionStr = "PlayerPosition";
        public string sceneStr = "SceneIndex";
        public string allInfoStr = "PlayerInfo";

        /// <summary>
        /// 保存数据的媒介
        /// </summary>
        public class PlayerInfo
        {
            public float Health;
            public Vector3 PlayerPosition;
            public int SceneIndex;

            public PlayerInfo() : this(0, Vector3.zero, 0)
            {
            }

            public PlayerInfo(float health, Vector3 playerPosition, int sceneIndex)
            {
                Health = health;
                PlayerPosition = playerPosition;
                SceneIndex = sceneIndex;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveAllInfo(GameManager.Instance.player.health,
                    GameManager.Instance.player.transform.position,
                    SceneManager.GetActiveScene().buildIndex,
                    allInfoStr);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                var playerInfo = LoadAllInfo(allInfoStr);
                GameManager.Instance.player.health = playerInfo.Health;
                GameManager.Instance.player.transform.position = playerInfo.PlayerPosition;
                UIManager.Instance.UpdateHealth(playerInfo.Health);
            }
        }

        public PlayerInfo LoadAllInfo(string key)
        {
            if (!PlayerPrefs.HasKey(key)) return null;

            var playerInfo = new PlayerInfo();
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), playerInfo);
            return playerInfo;
        }

        public float Loadhealth(string key)
        {
            if (!PlayerPrefs.HasKey(key)) return 3f;

            var playerInfo = new PlayerInfo();
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), playerInfo);
            return playerInfo.Health;
        }

        public int LoadScene(string key)
        {
            if (!PlayerPrefs.HasKey(key)) return 0;

            var playerInfo = new PlayerInfo();
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), playerInfo);
            return playerInfo.SceneIndex;
        }

        public void SaveAllInfo(float health, Vector3 position, int sceneIndex, string key)
        {
            var playerInfo = new PlayerInfo(health, position, sceneIndex);
            var json = JsonUtility.ToJson(playerInfo, true);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public void SaveHealth(float health, string key)
            => SaveAllInfo(health, Vector3.zero, 0, key);

        public void SaveSceneIndex(int index, string key) =>
            SaveAllInfo(0, Vector3.zero, SceneManager.GetActiveScene().buildIndex, key);
    }
}