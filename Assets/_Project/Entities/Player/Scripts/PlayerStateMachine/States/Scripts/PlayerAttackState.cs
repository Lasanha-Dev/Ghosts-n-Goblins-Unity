using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerAttackState", menuName = "StateMachine/Player/States/PlayerAttackState")]
    public sealed class PlayerAttackState : StateBase
    {
        private Animator _playerAnimator;

        private readonly int PLAYER_ATTACK_PARAM_TRIGGER_ID = Animator.StringToHash("Attack");

        public override void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();
        }

        public override void OnEnter()
        {
            _playerAnimator.SetTrigger(PLAYER_ATTACK_PARAM_TRIGGER_ID);
        }
    }
}
