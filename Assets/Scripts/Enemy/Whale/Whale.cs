using System;
using System.Drawing;
using Bomb;
using Manger;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Whale
{
    public class Whale : AllEnemy.Enemy
    {
        public float scale;
        public Transform bombArea;

        public override void GetDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            if (health == 0)
            {
                Debug.Log($"{gameObject.name}死亡");
                isDead = true;
                animator.SetTrigger(Dead);
                //还原体积
                transform.localScale = Vector3.one;
                //死亡后释放所有吞噬的炸弹
                var size = bombArea.childCount;
                for (var i = 0; i < size; i++)
                {
                    var bomb = bombArea.GetChild(i).GetComponent<BombController>();
                    bomb.gameObject.SetActive(true);
                    bomb.transform.localScale = Vector3.one;
                    var ps = bomb.transform.localPosition;
                    bomb.transform.localPosition = 
                        new Vector2(ps.x + Random.Range(-0.5f, 0.5f), ps.y);
                    bomb.TurnOn();
                }
                GameManager.Instance.RemoveEnemy(this);
                return;
            }

            animator.SetTrigger(GetHit);
        }

        //鲸鱼吞噬炸弹
        public void Swallow()
        {
            try
            {
                targetPoint.GetComponent<BombController>().TurnOff();
                targetPoint.parent = bombArea;
                targetPoint.localPosition = Vector3.zero;
                targetPoint.gameObject.SetActive(false);
                transform.localScale *= scale;
            }
            catch (Exception e)
            {
            }
        }
    }
}