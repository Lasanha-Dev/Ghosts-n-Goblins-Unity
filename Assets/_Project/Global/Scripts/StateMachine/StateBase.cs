using EntitieComponentsReferences = Game.Entities.EntitieComponentsReferences;

using System.Collections.Generic;

using UnityEngine;

namespace Game.StateMachine
{
    public abstract class StateBase : ScriptableObject
    {
        public IReadOnlyCollection<StateTransitions> StateTransitions { get; protected set; }

        protected bool _isInitialized = false;

        public void SetStateTransitions(IReadOnlyCollection<StateTransitions> stateTransitions)
        {
            StateTransitions ??= stateTransitions;
        }

        public virtual void InitializeState(StateMachineParametersBase stateMachineParameters, EntitieComponentsReferences entitieComponentsReferences)
        {
            _isInitialized = true;

            SetupState(entitieComponentsReferences);
        }

        protected abstract void SetupState(EntitieComponentsReferences playerComponentsReferences);

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