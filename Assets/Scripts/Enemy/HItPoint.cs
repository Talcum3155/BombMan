using Player;
using UnityEngine;

namespace Enemy
{
    public class HItPoint : MonoBehaviour
    {
        public float damage;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().GetDamage(damage);
            }

            if (other.CompareTag("Bomb"))
            {
                Debug.Log("吹灭炸弹");
            }
        }
    }
}
