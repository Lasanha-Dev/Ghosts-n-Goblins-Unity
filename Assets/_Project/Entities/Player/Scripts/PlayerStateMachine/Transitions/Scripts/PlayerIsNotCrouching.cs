using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using Game.Entities.Player;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsNotCrouching", menuName = "StateMachine/Player/Transitions/PlayerIsNotCrouching")]
    public class PlayerIsNotCrouching : TransitionConditionBase
    {
        private InputDefinition _playerCrouchAction;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerCrouchAction = PlayerInputsController.CrouchInput;
        }

        public override bool CanTransit()
        {
            return _playerCrouchAction.IsPressed == false;
        }
    }
}
