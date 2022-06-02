using System;
using Enemy;
using UnityEngine;

namespace Bomb
{
    public class BombController : MonoBehaviour
    {
        [Header("Parameters")] public float bombForce;
        public float startTime;
        public float waitTime;
        public bool isClosed;
        public float bombDamage;

        [Header("Check")] public float radius;
        [Tooltip("会被炸的层级")] public LayerMask targetLayer;

        [Header("Component")] private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            startTime = Time.time;
        }

        private void Update()
        {
            //炸弹被熄灭时就不再执行代码
            if (isClosed)
            {
                return;
            }

            if (Time.time >= startTime + waitTime)
            {
                _animator.Play("Explosion");
            }
        }

        // private void OnDrawGizmos() => Gizmos.DrawSphere(transform.position, radius);


        /// <summary>
        ///     有些怪的技能可以熄灭炸弹
        /// </summary>
        public void TurnOff()
        {
            //爆炸的一瞬间可能会被熄灭，导致该炸弹的碰撞体被取消无法再次点燃
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
            {
                return;
            }

            //改变炸弹Layer，防止炸弹熄灭后继续被敌人锁定
            gameObject.layer = LayerMask.NameToLayer("NPC");
            _animator.Play("Bomb_Off");
            isClosed = true;
        }

        /// <summary>
        ///     其他炸弹炸到该炸弹时可以被重新点燃
        /// </summary>
        public void TurnOn()
        {
            //重置炸弹的开始爆炸时间
            startTime = Time.time;
            gameObject.layer = LayerMask.NameToLayer("Bomb");
            _animator.Play("Bomb");
            isClosed = false;
        }

        #region 动画事件

        public void Explosion()
        {
            _collider2D.enabled = false; //防止将自己也加入检测的碰撞体之中而把自己炸飞
            var overlapCircle =
                Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

            _rigidbody2D.gravityScale = 0; //防止取消碰撞体后掉出世界

            foreach (var collider2D1 in overlapCircle)
            {
                var dir =
                    collider2D1.transform.position - transform.position;
                //向相反方向且向上 产生推力
                collider2D1.GetComponent<Rigidbody2D>()
                    .AddForce((dir.normalized + Vector3.up) * bombForce, ForceMode2D.Impulse);

                //如果该碰撞体是炸弹，且处于被熄灭状态，就将其重新点燃
                if (collider2D1.CompareTag("Bomb"))
                {
                    var anotherBomb = collider2D1.GetComponent<BombController>();
                    if (anotherBomb.isClosed)
                    {
                        anotherBomb.TurnOn();
                    }
                }

                //是玩家或者敌人就给予伤害
                if (collider2D1.CompareTag("Player")
                    || collider2D1.CompareTag("Enemy"))
                {
                    collider2D1.GetComponent<IDamage>()?.GetDamage(bombDamage);
                }
            }
        }

        public void DestroyThis()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}