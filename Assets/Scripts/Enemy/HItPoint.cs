using Player;
using UnityEngine;

namespace Enemy
{
    public class HItPoint : MonoBehaviour
    {
        public float damage;
        public bool bombAvailable;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().GetDamage(damage);
            }

            if (bombAvailable && other.CompareTag("Bomb"))
            {
                var normalized = (other.transform.position - transform.position).normalized;
                other.GetComponent<Rigidbody2D>()
                    .AddForce(new Vector2(normalized.x, normalized.y) * 10, ForceMode2D.Impulse);
            }
        }
    }
}