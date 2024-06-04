using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using Game.Entities.Player;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsTryingToJump", menuName = "StateMachine/Player/Transitions/PlayerIsTryingToJump")]

    public sealed class PlayerIsTryingToJump : TransitionConditionBase
    {
        private InputDefinition _playerJumpAction;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerJumpAction = PlayerInputsController.JumpInput;
        }

        public override bool CanTransit()
        {
            return _playerJumpAction.WasPressedThisFrame;
        }
    }
}