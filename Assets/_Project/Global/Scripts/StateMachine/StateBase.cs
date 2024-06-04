using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using System.Collections.Generic;

using UnityEngine;

namespace Game.StateMachine
{
    public abstract class StateBase : ScriptableObject
    {
        public IReadOnlyList<StateTransition> StateTransitions { get; private set; }

        public void SetStateTransitions(IReadOnlyList<StateTransition> stateTransitions)
        {
            StateTransitions = stateTransitions;
        }

        public abstract void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences);

        public virtual object GetStateParameterObject()
        {
            return null;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnExit()
        {

        }
    }
}