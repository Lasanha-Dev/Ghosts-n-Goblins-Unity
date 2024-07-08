using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;
using Game.Entities.Player;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsAttacking", menuName = "StateMachine/Player/Transitions/PlayerIsAttacking")]
    public sealed class PlayerIsAttacking : TransitionConditionBase
    {
        private InputDefinition _playerAttackAction;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerAttackAction = PlayerInputsController.AttackInput;
        }

        public override bool CanTransit()
        {
            return _playerAttackAction.WasPressedThisFrame;
        }
    }
}
