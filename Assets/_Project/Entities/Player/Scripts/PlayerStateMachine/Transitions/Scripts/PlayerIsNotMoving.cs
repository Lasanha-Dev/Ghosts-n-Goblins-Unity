using PlayerInputsController = Game.Entities.Player.PlayerInputsController;

using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using Game.Entities.Player;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsNotMoving", menuName = "StateMachine/Player/Transitions/PlayerIsNotMoving")]
    public sealed class PlayerIsNotMoving : TransitionConditionBase
    {
        private InputDefinition<float> _playerMovementAction;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerMovementAction = PlayerInputsController.MovementInput;
        }

        public override bool CanTransit()
        {
            return Mathf.Approximately(_playerMovementAction.InputValue, 0.0f);
        }
    }
}