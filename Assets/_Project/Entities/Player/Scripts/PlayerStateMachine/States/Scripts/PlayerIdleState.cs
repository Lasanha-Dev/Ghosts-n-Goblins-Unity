using PlayerComponentsReferences = Game.Entities.Player.PlayerComponentsReferences;

using UnityEngine;
using Game.Entities;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIdleState", menuName = "StateMachine/Player/States/PlayerIdleState")]
    public sealed class PlayerIdleState : PlayerStateBase
    {
        private Rigidbody2D _playerRigidbody;

        private Animator _playerAnimator;

        private readonly int PLAYER_IDLE_ANIMATION_STATE = Animator.StringToHash("PlayerStandingIdle");

        public override void OnEnter()
        {
            _playerRigidbody.velocity = Vector2.zero;

            _playerAnimator.Play(PLAYER_IDLE_ANIMATION_STATE);
        }

        protected override void SetupState(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerRigidbody = playerComponentsReferences.GetEntitieComponent<Rigidbody2D>();

            _playerAnimator = playerComponentsReferences.GetEntitieComponent<Animator>();
        }
    }
}