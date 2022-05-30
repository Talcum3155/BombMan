using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Parameters")] public float speed;
        public float jumpForce;
        [Tooltip("跳跃到空中时的重力")] public float jumpGravityScale;

        [Header("GroundCheck")] public LayerMask groundMask;
        public float checkRadius;
        public Transform groundChecker;

        [Header("StatusCheck")] public bool canJump;
        public bool isJump;
        public bool isGround;

        [Header("Component")] private Rigidbody2D _rigidbody2D;

        [Header("FX")] public GameObject jumpFX;
        public GameObject landFX;

        [Header("Attack")] public float nextAttackTime;
        public GameObject projectile;
        public float attackCutDown;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            StatusCheck();
            Attack();
        }

        private void FixedUpdate()
        {
            CheckGround();
            Movement();
            Jump();
        }

        #region 移动相关

        private void Movement()
        {
            var axisH = Input.GetAxis("Horizontal"); //获取的键程最大，只有-1与1

            _rigidbody2D.velocity = new Vector2(axisH * speed, _rigidbody2D.velocity.y);

            transform.eulerAngles = axisH switch
            {
                > 0 => new Vector3(0, 0, 0),
                < 0 => new Vector3(0, 180, 0),
                _ => transform.eulerAngles
            };
        }

        /// <summary>
        /// 跳跃的时候将重力增大，可以增加手感
        /// </summary>
        private void Jump()
        {
            if (!canJump) return;

            isJump = true;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            _rigidbody2D.gravityScale = jumpGravityScale; //增大重力
            canJump = false;
        }

        private void StatusCheck()
        {
            if (Input.GetButtonDown("Jump") && isGround)
                canJump = true;
        }

        private void CheckGround()
        {
            isGround = Physics2D.OverlapCircle(groundChecker.position, checkRadius, groundMask);

            if (isGround)
            {
                _rigidbody2D.gravityScale = 1;
                isJump = false;
                return;
            }

            _rigidbody2D.gravityScale = 4;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundChecker.position, checkRadius);
        }

        #endregion

        #region 攻击相关

        private void Attack()
        {
            if (!(Time.time >= nextAttackTime && Input.GetButtonDown("Fire1"))) return;
            Instantiate(projectile,transform.position,transform.rotation);
            nextAttackTime = Time.time + attackCutDown;
        }

        #endregion

        #region 动画事件

        public void ShowJumpFX()
        {
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0.15f, -0.5f, 0);
        }

        public void ShowLandFX()
        {
            landFX.SetActive(true);
            landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
        }

        public void ShowRunFX()
        {
            var runFx = ObjectPools.Instance.GetRunFXObject();
            runFx.transform.parent = transform.parent;
            runFx.transform.localEulerAngles = transform.localEulerAngles;
            
            runFx.transform.localPosition = runFx.transform.localEulerAngles.y switch
            {
                <180 => transform.localPosition + new Vector3(-0.5f, -0.75f, 0),
                >=180 => transform.localPosition + new Vector3(0.6f, -0.75f, 0)
            };
            runFx.SetActive(true);
        }

        #endregion
    }
}