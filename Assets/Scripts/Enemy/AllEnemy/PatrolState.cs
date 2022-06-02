using UnityEngine;

namespace Enemy.AllEnemy
{
    public class PatrolState : EnemyBaseState
    {
        public override void EnterState(Enemy enemy)
        {
            //切换到巡逻状态，重新定位目标
            enemy.SwitchPoint();
            //从Run动画回到Idle动画
            enemy.animator.SetInteger(enemy.AnimState, 0);
        }

        public override void OnUpdate(Enemy enemy)
        {
            // Debug.Log("巡逻状态");
            if (enemy.targets.Count > 0)
            {
                Debug.Log("进入攻击状态");
                enemy.TransitionState(enemy.AttackState);
                /*
                 * 如果不加return会出现同步问题，下面执的语句会将AnimState设为1，
                 * 覆盖了攻击状态设的2，导致动画在Layer1无法攻击
                 */
                return;
            }
            
            //到达了指定地点，就在原地停留到Idle动画播放完毕再走向下一个指定地点
            if (!enemy.animator.
                GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                // Debug.Log("巡逻...");
                //防止Run动画回到Idle动画
                enemy.animator.SetInteger(enemy.AnimState, 1);
                enemy.MoveToTarget();
            }
            
            if (Mathf.Abs(enemy.transform.position.x - 
                          enemy.targetPoint.position.x) < 0.01f)
            {
                //到达了指定地点，重置状态
                enemy.TransitionState(enemy.patrolState);
            }
        }
    }
}
