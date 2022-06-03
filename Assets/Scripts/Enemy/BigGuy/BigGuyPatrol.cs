using Enemy.AllEnemy;
using UnityEngine;

namespace Enemy.BigGuy
{
    public class BigGuyPatrol : AttackState
    {
        public override void EnterState(AllEnemy.Enemy enemy)
        {
            Debug.Log("大块头的Enter");
            base.EnterState(enemy);
        }

        public override void OnUpdate(AllEnemy.Enemy enemy)
        {
            //不用担心，类型强转的损耗可以忽略不计
            if (((BigGuy) enemy).hasBomb)
            {
                return;
            }
            base.OnUpdate(enemy);
        }
    }
}
