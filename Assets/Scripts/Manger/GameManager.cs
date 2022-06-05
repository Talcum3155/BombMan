using System.Collections.Generic;
using Objects;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manger
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Component")] public PlayerController player;
        public Door door;

        public List<Enemy.AllEnemy.Enemy> enemies = new();

        /// <summary>
        /// 注册玩家
        /// </summary>
        /// <param name="playerController"></param>
        public void RegisterPlayer(PlayerController playerController)
            => player = playerController;

        /// <summary>
        /// 注册进入下一个场景的门
        /// </summary>
        /// <param name="targetDoor"></param>
        public void RegisterDoor(Door targetDoor) => door = targetDoor;

        /// <summary>
        /// 注册敌人
        /// </summary>
        /// <param name="enemy"></param>
        public void AddEnemy(Enemy.AllEnemy.Enemy enemy) => enemies.Add(enemy);

        /// <summary>
        /// 敌人死亡后将其移除
        /// </summary>
        /// <param name="enemy"></param>
        public void RemoveEnemy(Enemy.AllEnemy.Enemy enemy)
        {
            enemies.Remove(enemy);

            if (enemies.Count == 0)
            {
                door.OpenDoor();
            }
        }

        /// <summary>
        /// 启用游戏结束菜单
        /// </summary>
        public void GameOver() => UIManager.Instance.GameOverMenu();

        /// <summary>
        /// 去到下一个场景
        /// </summary>
        public void NextLevel()
        {
            //只保存血量
            SaveManager.Instance.SaveHealth(player.health, SaveManager.Instance.healthStr);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}