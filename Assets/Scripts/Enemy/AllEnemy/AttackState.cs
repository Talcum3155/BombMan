using UnityEngine;

namespace Enemy.AllEnemy
{
    public class AttackState : EnemyBaseState
    {
        public override void EnterState(global::Enemy.AllEnemy.Enemy enemy)
        {
            // Debug.Log("找到目标");
            enemy.targetPoint = enemy.targets[0].transform;
            //从动画的Layer1进入到Layer2
            enemy.animator.SetInteger(enemy.AnimState, 2);
        }

        public override void OnUpdate(global::Enemy.AllEnemy.Enemy enemy)
        {
            // Debug.Log("攻击状态");
            //没有敌人时回到巡逻状态，animState的值设置回小于2的值，脱离Layer2
            if (enemy.targets.Count == 0)
            {
                Debug.Log("回到巡逻状态");
                enemy.TransitionState(enemy.patrolState);
                return;
            }

            /*
             * 吹灭一个炸弹之后仍会锁定原来的transform，如果List中有多个对象，由于吹炸弹的时候离得很近
             * 所以被吹灭的炸弹的距离小于所有其他炸弹的距离，人物会一直朝着被吹灭的炸弹走
             */
            if (enemy.targets.Count > 1)
            {
                foreach (var t in enemy.targets)
                {
                    //如果列表中有物体的距离比当前追踪物体的距离更近，就追踪该物体
                    if (Mathf.Abs(enemy.transform.position.x - t.transform.position.x) <
                        Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                    {
                        Debug.Log("更换追踪");
                        enemy.targetPoint = t.transform;
                    }
                }
            }

            /*
             * 如果List中有两个目标，解决其中一个之后，List中的对象就只剩下一个，如果没有该if条件，
             * 就无法切换到List中的其他transform，由于原来被解决掉的对象的transform仍然被锁定，
             * 人物会一直向原来的transform行走
             */
            if (enemy.targets.Count == 1)
            {
                Debug.Log("只剩一个目标");
                enemy.targetPoint = enemy.targets[0].transform;
            }
            
            if (enemy.targetPoint.gameObject.CompareTag("Bomb"))
            {
                if (enemy.SkillAction())
                {
                    Debug.Log("成功吹灭，更换目标");
                    if (enemy.targets[0])
                    {
                        enemy.targetPoint = enemy.targets[0].transform;
                    }
                    return;
                }
                Debug.Log("吹灭失败，跑路");
                
            }
            
            if (enemy.targetPoint.gameObject.CompareTag("Player"))
            {
                enemy.AttackAction();
            }
            
            enemy.MoveToTarget();
        }
    }
}