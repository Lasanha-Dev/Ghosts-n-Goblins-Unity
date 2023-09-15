using System.Collections.Generic;

namespace Game.StateMachine
{
    [System.Serializable]
    public sealed class StateDefinition
    {
        [UnityEngine.HideInInspector]
        public string name;

        public StateBase baseState;

        public List<StateTransitions> stateTransitions;

        public void ValidateInspector()
        {
            if (baseState != null)
            {
                name = baseState.name;
            }

            foreach (StateTransitions stateTransition in stateTransitions)
            {
                stateTransition.ValidateInspector();
            }
        }
    }
}