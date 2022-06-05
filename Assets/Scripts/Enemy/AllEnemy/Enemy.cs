using System;
using System.Collections;
using System.Collections.Generic;
using Manger;
using UnityEngine;

namespace Enemy.AllEnemy
{
    public class Enemy : MonoBehaviour, IDamage
    {
        [Header("Parameters")] public float health;
        public bool isDead;
        public bool isBoss;

        [Header("Movement")] public float speed;
        public Transform pointA;
        public Transform pointB;

        [Header("Attack")] public Transform targetPoint;
        public List<GameObject> targets = new List<GameObject>();
        public float attackCutDown;
        protected float nextAttackTime;
        public float skillRange;
        public float attackRange;

        [Header("State")] protected EnemyBaseState CurrentState;
        public PatrolState patrolState = new PatrolState();
        public AttackState AttackState = new AttackState();

        [Header("Component")] public Animator animator;
        private GameObject _alarmSign;
        private Animator _alarmAnimator;

        public readonly int AnimState =
            Animator.StringToHash("AnimState");

        public readonly int Attack =
            Animator.StringToHash("Attack");

        public readonly int Skill =
            Animator.StringToHash("Skill");

        protected static readonly int GetHit =
            Animator.StringToHash("GetHit");

        protected static readonly int Dead =
            Animator.StringToHash("Dead");


        protected virtual void Init()
        {
            animator = GetComponent<Animator>();
            _alarmSign = transform.GetChild(0).gameObject;
            _alarmAnimator = _alarmSign.GetComponent<Animator>();
            GameManager.Instance.AddEnemy(this);
        }

        protected virtual void Awake()
        {
            Init();
        }

        private void Start()
        {
            TransitionState(patrolState);

            if (isBoss)
            {
                UIManager.Instance.bossHealthSlider.maxValue = health;
            }
        }

        protected virtual void Update()
        {
            if (isDead)
            {
                return;
            }

            CurrentState.OnUpdate(this);
        }


        #region 移动相关

        public void MoveToTarget()
        {
            // Debug.Log("巡逻...Move");
            transform.position =
                Vector2.MoveTowards(
                    transform.position,
                    targetPoint.position,
                    speed * Time.deltaTime);
            FlipDirection();
        }

        public void FlipDirection()
        {
            //目标在右边
            if (transform.position.x < targetPoint.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                return;
            }

            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        public void SwitchPoint()
        {
            if (Mathf.Abs(transform.position.x - pointA.position.x) >
                Mathf.Abs(transform.position.x - pointB.position.x))
            {
                targetPoint = pointA;
                return;
            }

            targetPoint = pointB;
        }

        #endregion

        #region 攻击相关

        public virtual void AttackAction()
        {
            //距离够近且冷却已经结束才能进行攻击
            if (!(Vector2.Distance(transform.position,
                targetPoint.position) <= attackRange) || !(Time.time >= nextAttackTime)) return;

            animator.SetTrigger(Attack);
            nextAttackTime = Time.time + attackCutDown;
        }

        public virtual int SkillAction()
        {
            //距离够近且冷却已经结束才能进行攻击
            if (!(Vector2.Distance(transform.position,
                targetPoint.position) <= skillRange))
            {
                return 1; //返回1说明距离太远
            }

            if (!(Time.time >= nextAttackTime))
            {
                return 2; //返回2说明冷却时间不够
            }

            animator.SetTrigger(Skill);
            nextAttackTime = Time.time + attackCutDown;
            return 0;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!targets.Contains(other.gameObject))
            {
                targets.Add(other.gameObject);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (targets.Contains(other.gameObject))
            {
                targets.Remove(other.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDead) return;
            if (other.CompareTag("Player") || other.CompareTag("Bomb"))
            {
                StartCoroutine(ShowAlarm());
            }
        }

        private IEnumerator ShowAlarm()
        {
            _alarmSign.SetActive(true);
            yield return new WaitForSeconds(_alarmAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            _alarmSign.SetActive(false);
        }

        #endregion

        #region 状态机

        public void TransitionState(EnemyBaseState state)
        {
            CurrentState = state;
            state.EnterState(this);
        }

        #endregion

        #region 受伤或死亡

        public virtual void GetDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);

            //如果是boss则更新boss血条
            if (isBoss)
            {
                UIManager.Instance.bossHealthSlider.value = health;
            }
            
            if (health == 0)
            {
                Debug.Log($"{gameObject.name}死亡");
                isDead = true;
                animator.SetTrigger(Dead);
                GameManager.Instance.RemoveEnemy(this);
                return;
            }

            animator.SetTrigger(GetHit);
        }

        #endregion
    }
}