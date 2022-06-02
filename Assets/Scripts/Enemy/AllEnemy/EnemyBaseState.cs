using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBaseState
    {
        public abstract void EnterState(AllEnemy.Enemy enemy);
        public abstract void OnUpdate(AllEnemy.Enemy enemy);
    }
}
