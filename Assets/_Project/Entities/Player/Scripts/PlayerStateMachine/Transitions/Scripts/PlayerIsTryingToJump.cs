using Game.Entities;
using Game.Entities.Player;
using UnityEngine;

using InputActionReference = UnityEngine.InputSystem.InputActionReference;

using PlayerComponentsReferences = Game.Entities.Player.PlayerComponentsReferences;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsTryingToJump", menuName = "StateMachine/Player/Transitions/PlayerIsTryingToJump")]

    public sealed class PlayerIsTryingToJump : TransitionConditionBase
    {
        private InputActionReference _playerJumpAction;
        
        //private CommandEvents.JumpButtonPerformed _jumpButtonPerformed = null;

        public override bool CheckTransition()
        {
            return _playerJumpAction.action.WasPerformedThisFrame();
        }


        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerJumpAction = playerComponentsReferences.GetEntitieComponent<PlayerInputsController>().PlayerJumpAction;
        }
    }
}