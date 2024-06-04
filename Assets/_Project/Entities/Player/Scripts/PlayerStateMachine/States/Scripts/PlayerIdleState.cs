using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIdleState", menuName = "StateMachine/Player/States/PlayerIdleState")]
    public sealed class PlayerIdleState : StateBase
    {
        private Rigidbody2D _playerRigidbody;

        private Animator _playerAnimator;

        private readonly int PLAYER_IDLE_ANIMATION_STATE = Animator.StringToHash("PlayerStandingIdle");

        public override void OnEnter()
        {
            _playerRigidbody.velocity = new Vector2(0, _playerRigidbody.velocity.y);

            _playerAnimator.Play(PLAYER_IDLE_ANIMATION_STATE);
        }

        public override void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerRigidbody = entityComponentsReferences.GetEntityComponent<Rigidbody2D>();

            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();
        }

        public override void OnExit()
        {
            _playerRigidbody.velocity = new Vector2(0, _playerRigidbody.velocity.y);
        }
    }
}