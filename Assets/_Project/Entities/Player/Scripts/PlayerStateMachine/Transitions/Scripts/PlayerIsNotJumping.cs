using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsNotJumping", menuName = "StateMachine/Player/Transitions/PlayerIsNotJumping")]

    public sealed class PlayerIsNotJumping : TransitionConditionBase
    {
        private Rigidbody2D _playerRigibody;

        public override bool CanTransit()
        {
            return Mathf.Approximately(_playerRigibody.velocity.y, 0.0f);
        }

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerRigibody = entityComponentsReferences.GetEntityComponent<Rigidbody2D>();
        }
    }
}