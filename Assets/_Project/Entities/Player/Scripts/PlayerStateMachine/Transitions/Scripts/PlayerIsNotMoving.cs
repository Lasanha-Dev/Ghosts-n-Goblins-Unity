using PlayerComponentsReferences = Game.Entities.Player.PlayerComponentsReferences;

using UnityEngine;

using PlayerInputsController = Game.Entities.Player.PlayerInputsController;
using Game.Entities;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsNotMoving", menuName = "StateMachine/Player/Transitions/PlayerIsNotMoving")]
    public sealed class PlayerIsNotMoving : TransitionConditionBase
    {
        private Rigidbody2D _playerRigidbody;

        private PlayerInputsController _playerInputsController;

        public override bool CheckTransition()
        {
            return _playerInputsController.PlayerMovementInputValue == 0f && Mathf.Approximately(_playerRigidbody.velocity.y, 0.00f) == true;
        }

        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerRigidbody = playerComponentsReferences.GetEntitieComponent<Rigidbody2D>();

            _playerInputsController = playerComponentsReferences.GetEntitieComponent<PlayerInputsController>();
        }
    }
}