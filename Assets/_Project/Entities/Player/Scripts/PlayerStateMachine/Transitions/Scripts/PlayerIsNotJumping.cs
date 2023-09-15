using Game.Entities;
using UnityEngine;

using PlayerComponentsReferences = Game.Entities.Player.PlayerComponentsReferences;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsNotJumping", menuName = "StateMachine/Player/Transitions/PlayerIsNotJumping")]

    public sealed class PlayerIsNotJumping : TransitionConditionBase
    {
        private Rigidbody2D _playerRigibody;

        public override bool CheckTransition()
        {
            return Mathf.Approximately(_playerRigibody.velocity.y, 0.0f);
        }


        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerRigibody = playerComponentsReferences.GetEntitieComponent<Rigidbody2D>();
        }
    }
}