using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Component")] private Rigidbody2D _rigidbody2D;
        private PlayerController _playerController;
        private Animator _animator;
        
        private static readonly int Jump = Animator.StringToHash("jump");
        private static readonly int Speed = Animator.StringToHash("speed");
        private static readonly int VelocityY = Animator.StringToHash("velocityY");
        private static readonly int Ground = Animator.StringToHash("ground");

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            CheckAnimatorStatus();
        }

        private void CheckAnimatorStatus()
        {
            _animator.SetBool(Ground, _playerController.isGround);
            _animator.SetFloat(VelocityY, _rigidbody2D.velocity.y);
            _animator.SetBool(Jump, _playerController.isJump);
            _animator.SetFloat(Speed, Mathf.Abs(_rigidbody2D.velocity.x));
        }
    }
}
