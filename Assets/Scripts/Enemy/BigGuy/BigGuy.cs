using System;
using Enemy.AllEnemy;
using Player;
using UnityEngine;

namespace Enemy.BigGuy
{
    public class BigGuy : AllEnemy.Enemy
    {
        public bool hasBomb;
        public Transform pickUpPoint;
        public int power;

        protected override void Awake()
        {
            base.Awake();
            AttackState = new BigGuyPatrol();
        }

        public void PickBomb()
        {
            if (!targetPoint.CompareTag("Bomb")) return;

            hasBomb = true;
            targetPoint.parent = pickUpPoint;
            targetPoint.localPosition=Vector3.zero;
            targetPoint.GetComponent<Rigidbody2D>().isKinematic = true;
        }

        public void ThrowBomb()
        {
            try
            {
                if (!targetPoint.CompareTag("Bomb")) return;
                var player = FindObjectOfType<PlayerController>().transform;
                var normalized = (player.position-targetPoint.position).normalized;
                var bombRigidBody = targetPoint.GetComponent<Rigidbody2D>();
                bombRigidBody.isKinematic = false;
                bombRigidBody.AddForce(new Vector2(normalized.x, normalized.y) * power, ForceMode2D.Impulse);
                targetPoint.parent = transform.parent.parent;
                hasBomb = false;
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
