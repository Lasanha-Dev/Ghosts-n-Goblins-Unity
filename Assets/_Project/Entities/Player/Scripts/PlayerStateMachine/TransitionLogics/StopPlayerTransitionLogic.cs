using Game.Entities;

using Game.StateMachine;

using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StopPlayerTransitionLogic", menuName = "StateMachine/Player/TransitionLogics/StopPlayerTransitionLogic")]
    public sealed class StopPlayerTransitionLogic : TransitionLogic
    {
        private Rigidbody2D _playerRigidbody;

        public override void SetupLogic(EntityComponentsReferences entityComponentsReferences)
        {
            _playerRigidbody = entityComponentsReferences.GetEntityComponent<Rigidbody2D>();
        }

        public override void ExecuteLogic()
        {
            _playerRigidbody.velocity = Vector2.zero;
        }
    }
}
