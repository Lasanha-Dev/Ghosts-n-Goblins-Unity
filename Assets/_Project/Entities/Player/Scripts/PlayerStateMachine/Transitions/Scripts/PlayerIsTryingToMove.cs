using UnityEngine;

using Game.Entities.Player;
using Game.Entities;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsTryingToMove", menuName = "StateMachine/Player/Transitions/PlayerIsTryingToMove")]
    public sealed class PlayerIsTryingToMove : TransitionConditionBase
    {
        private PlayerInputsController _playerInputsController;

        public override bool CheckTransition()
        {
            return _playerInputsController.PlayerMovementInputValue != 0;
        }

        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerInputsController = playerComponentsReferences.GetEntitieComponent<PlayerInputsController>();
        }
    }
}