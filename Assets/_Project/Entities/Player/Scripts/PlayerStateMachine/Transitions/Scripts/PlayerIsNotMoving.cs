using UnityEngine;

using PlayerInputsController = Game.Entities.Player.PlayerInputsController;
using Game.Entities;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsNotMoving", menuName = "StateMachine/Player/Transitions/PlayerIsNotMoving")]
    public sealed class PlayerIsNotMoving : TransitionConditionBase
    {
        private PlayerInputsController _playerInputsController;

        public override bool CheckTransition()
        {
            return Mathf.Approximately(_playerInputsController.MovementInput.InputValue, 0.0f);
        }

        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerInputsController = playerComponentsReferences.GetEntitieComponent<PlayerInputsController>();
        }
    }
}