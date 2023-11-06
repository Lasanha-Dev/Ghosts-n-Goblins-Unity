using Game.Entities;
using Game.Entities.Player;
using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsTryingToJump", menuName = "StateMachine/Player/Transitions/PlayerIsTryingToJump")]

    public sealed class PlayerIsTryingToJump : TransitionConditionBase
    {
        private InputDefinition _playerJumpAction;
        
        public override bool CheckTransition()
        {
            return _playerJumpAction.WasPressedThisFrame;
        }

        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerJumpAction = playerComponentsReferences.GetEntitieComponent<PlayerInputsController>().JumpInput;
        }
    }
}