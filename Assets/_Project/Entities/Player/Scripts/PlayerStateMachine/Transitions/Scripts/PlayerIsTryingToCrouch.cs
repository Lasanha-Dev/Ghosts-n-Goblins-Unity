using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using Game.Entities.Player;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsTryingToCrouch", menuName = "StateMachine/Player/Transitions/PlayerIsTryingToCrouch")]
    public sealed class PlayerIsTryingToCrouch : TransitionConditionBase
    {
        private InputDefinition _playerCrouchAction;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerCrouchAction = PlayerInputsController.CrouchInput;
        }

        public override bool CanTransit()
        {
           return _playerCrouchAction.IsPressed;
        }
    }
}
