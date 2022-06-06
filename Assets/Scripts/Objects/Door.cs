using System;
using Manger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Objects
{
    public enum DoorState
    {
        Entrance,
        Exit
    }

    public class Door : MonoBehaviour
    {
        [Header("Component")] public DoorState doorState;
        public Animator animator;
        public new Collider2D collider;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            collider.enabled = false;

            switch (doorState)
            {
                case DoorState.Entrance:
                    //如果是入口就播放关门动画
                    animator.Play("Door_Close");
                    break;
                case DoorState.Exit:
                    //如果是出口就向GameManager注册自己
                    if (GameManager.Instance)
                    {
                        GameManager.Instance.RegisterDoor(this);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// 敌人消灭完之后就打开门，启用碰撞体，播放开门动画
        /// </summary>
        public void OpenDoor()
        {
            collider.enabled = true;
            animator.Play("Door_Open");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.NextLevel();
            }
        }
    }
}