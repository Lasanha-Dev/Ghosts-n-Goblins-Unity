using Game.Entities;
using UnityEngine;

using PlayerComponentsReferences = Game.Entities.Player.PlayerComponentsReferences;

namespace Game.StateMachine.Player
{

    [CreateAssetMenu(fileName = "PlayerJumpState", menuName = "StateMachine/Player/States/PlayerJumpState")]

    public sealed class PlayerJumpState : PlayerStateBase
    {
        [SerializeField] private float _jumpHeight;

        [SerializeField] private Vector2 _groundOffSet;

        private Rigidbody2D _playerRigidbody;

        private Animator _playerAnimator;

        private readonly int PlayerIdleJumpAnimationState = Animator.StringToHash("PlayerIdleJump");

        private readonly int PlayerMovingJumpAnimationState = Animator.StringToHash("PlayerMovingJump");

        private float _jumpForce;

        public override void OnEnter()
        {
            RemovePlayerFromGround();

            PlayJumpAnimation();

            _playerRigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }

        private void PlayJumpAnimation()
        {
            if (_playerRigidbody.velocity.x != 0)
            {
                _playerAnimator.Play(PlayerMovingJumpAnimationState);

                return;
            }

            _playerAnimator.Play(PlayerIdleJumpAnimationState);
        }

        private void RemovePlayerFromGround()
        {
            _playerRigidbody.position += _groundOffSet;
        }

        private float CalculateJumpForce()
        {
            return Mathf.Sqrt(2 * (Physics2D.gravity.magnitude * _playerRigidbody.gravityScale) * _jumpHeight);
        }

        protected override void SetupState(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerRigidbody = playerComponentsReferences.GetEntitieComponent<Rigidbody2D>();

            _playerAnimator = playerComponentsReferences.GetEntitieComponent<Animator>();

            _jumpForce = CalculateJumpForce();
        }
    }
}