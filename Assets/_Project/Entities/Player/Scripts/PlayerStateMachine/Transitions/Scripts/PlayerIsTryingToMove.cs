using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;

using Game.Entities.Player;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsTryingToMove", menuName = "StateMachine/Player/Transitions/PlayerIsTryingToMove")]
    public sealed class PlayerIsTryingToMove : TransitionConditionBase
    {
        private InputDefinition<float> _playerMovementAction;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerMovementAction = PlayerInputsController.MovementInput;
        }

        public override bool CanTransit()
        {
            return _playerMovementAction.InputValue != 0;
        }
    }
}