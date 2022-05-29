using System;
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
        public bool isGround;
        
        [Header("Component")] private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private static readonly int Running = Animator.StringToHash("Running");

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            StatusCheck();
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
            var axisH = Input.GetAxisRaw("Horizontal"); //获取的键程最大，只有-1与1

            _rigidbody2D.velocity = new Vector2(axisH * speed, _rigidbody2D.velocity.y);

            if (axisH != 0)
            {
                _animator.SetBool(Running, true);
                transform.localScale = new Vector3(axisH, 1, 1);
                return;
            }

            _animator.SetBool(Running, false);
        }

        private void Jump()
        {
            if (!canJump) return;

            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            _rigidbody2D.gravityScale = jumpGravityScale;
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
                return;
            }

            _rigidbody2D.gravityScale = 4;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundChecker.position, checkRadius);
        }

        #endregion
    }
}