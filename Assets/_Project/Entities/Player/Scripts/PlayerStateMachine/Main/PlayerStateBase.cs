using Game.Entities;

namespace Game.StateMachine.Player
{
    public abstract class PlayerStateBase : StateBase
    {
        protected PlayerStateMachineParameters _playerStateMachineParameters;

        public override void InitializeState(StateMachineParametersBase stateMachineParameters, EntitieComponentsReferences entitieComponentsReferences)
        {
            if (_isInitialized == false)
            {
                base.InitializeState(stateMachineParameters, entitieComponentsReferences);

                _playerStateMachineParameters = (PlayerStateMachineParameters)stateMachineParameters;
            }
        }
    }
}