using System;
using UnityEngine;

namespace Bomb
{
    public class BombController : MonoBehaviour
    {
        [Header("Parameters")] public float bombForce;
        public float startTime;
        public float waitTime;

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
            if (Time.time >= startTime + waitTime)
            {
                _animator.Play("Explosion");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, radius);
        }

        #region 动画事件

        public void Explosion()
        {
            _collider2D.enabled = false; //防止将自己也加入检测的碰撞体之中而把自己炸飞
            var overlapCircle = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

            _rigidbody2D.gravityScale = 0; //防止取消碰撞体后掉出世界

            foreach (var collider2D1 in overlapCircle)
            {
                var dir = collider2D1.transform.position - transform.position;
                //向相反方向且向上 产生推力
                collider2D1.GetComponent<Rigidbody2D>().AddForce((dir.normalized + Vector3.up) * bombForce, ForceMode2D.Impulse);
            }
        }

        public void DestroyThis()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}