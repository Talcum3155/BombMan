using System;
using UnityEngine;

namespace Manger
{
    public class SaveManager : Singleton<SaveManager>
    {
        public string healthStr = "PlayerHealth";
        public string positionStr = "PlayerPosition";
        public string allInfoStr = "PlayerInfo";

        /// <summary>
        /// 保存数据的媒介
        /// </summary>
        public class PlayerInfo
        {
            public float Health;
            public Vector3 PlayerPosition;

            public PlayerInfo() : this(0, Vector3.zero)
            {
            }

            public PlayerInfo(float health, Vector3 playerPosition)
            {
                Health = health;
                PlayerPosition = playerPosition;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveAllInfo(GameManager.Instance.player.health,
                    GameManager.Instance.player.transform.position,
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

        public void SaveAllInfo(float health, Vector3 position, string key)
        {
            var playerInfo = new PlayerInfo(health, position);
            var json = JsonUtility.ToJson(playerInfo, true);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public void SaveHealth(float health, string key) => SaveAllInfo(health, Vector3.zero, key);
    }
}