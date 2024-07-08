using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerFinishedAttack", menuName = "StateMachine/Player/Transitions/PlayerFinishedAttack")]
    public sealed class PlayerFinishedAttack : TransitionConditionBase
    {
        private Animator _playerAnimator;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();
        }

        public override bool CanTransit()
        {
           return _playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
    }
}