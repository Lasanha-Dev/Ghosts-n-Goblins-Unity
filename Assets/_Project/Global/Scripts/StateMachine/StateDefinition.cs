using System.Collections.Generic;

using SerializeField = UnityEngine.SerializeField;

namespace Game.StateMachine
{
    [System.Serializable]
    public sealed class StateDefinition
    {
#if UNITY_EDITOR
        [UnityEngine.HideInInspector]
        public string name;
#endif
        [field: SerializeField] public StateBase BaseState { get; private set; }

        [field: SerializeField] public List<StateTransition> StateTransitions { get; private set; }

        public StateDefinition(StateBase stateBase)
        {
            BaseState = stateBase;
        }

#if UNITY_EDITOR
        public void ValidateInspector()
        {
            if (BaseState != null)
            {
                name = BaseState.name;
            }

            foreach (StateTransition stateTransition in StateTransitions)
            {
                stateTransition.ValidateInspector();
            }
        }
#endif
    }
}