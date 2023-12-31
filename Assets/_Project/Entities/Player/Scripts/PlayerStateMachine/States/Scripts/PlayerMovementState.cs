using UnityEngine;

using Game.Entities.Player;
using Game.Entities;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementState", menuName = "StateMachine/Player/States/PlayerMovementState")]
    public sealed class PlayerMovementState : PlayerStateBase
    {
        private Rigidbody2D _playerRigidbody;

        private PlayerInputsController _playerInputsController;

        private Animator _playerAnimator;

        private readonly int PLAYER_RUNNING_ANIMATION_STATE = Animator.StringToHash("PlayerRunning");

        public override void OnEnter()
        {
            _playerAnimator.Play(PLAYER_RUNNING_ANIMATION_STATE);
        }

        public override void OnFixedUpdate()
        {
            _playerRigidbody.velocity = new Vector2(_playerInputsController.MovementInput.InputValue * _playerStateMachineParameters.MovementSpeed * Time.fixedDeltaTime, _playerRigidbody.velocity.y);
        }

        protected override void SetupState(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerRigidbody = playerComponentsReferences.GetEntitieComponent<Rigidbody2D>();

            _playerInputsController = playerComponentsReferences.GetEntitieComponent<PlayerInputsController>();

            _playerAnimator = playerComponentsReferences.GetEntitieComponent<Animator>();
        }
    }
}