using UnityEngine;

namespace Enemy.Captain
{
    public class Captain : AllEnemy.Enemy
    {
        private SpriteRenderer _spriteRenderer;

        protected override void Init()
        {
            base.Init();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override int SkillAction()
        {
            //距离炸弹足够近就会播放技能动画，一直反跑到技能动画结束为止
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Skill"))
            {
                _spriteRenderer.flipX = true;
                //炸弹在左边，向右跑
                if (transform.position.x > targetPoint.position.x)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        transform.position + Vector3.right,
                        speed * 2f * Time.deltaTime);
                }

                //炸弹在右边，向左跑
                if (transform.position.x < targetPoint.position.x)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        transform.position + Vector3.left,
                        speed * 2f * Time.deltaTime);
                }

                return 3;
            }
            
            
            _spriteRenderer.flipX = false;
            return base.SkillAction();
        }

        //技能动画结束后禁用X翻转
        public void RestoreFlip()
        {
            _spriteRenderer.flipX = false;
        }
    }
}