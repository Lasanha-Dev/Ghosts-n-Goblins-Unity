using System.Collections.Generic;

using SerializeField = UnityEngine.SerializeField;

namespace Game.StateMachine
{
    [System.Serializable]
    public sealed class StateTransition
    {
#if UNITY_EDITOR
        [UnityEngine.HideInInspector]
        public string name;
#endif
        [field: SerializeField] public StateBase TransitionState { get; private set; }

        [field: SerializeField] public List<TransitionConditionBase> TransitionConditions { get; private set; }

        [field: SerializeField] public bool AllConditionsNeedsToMatch { get; private set; }

        public StateTransition(StateBase transitionState, List<TransitionConditionBase> transitionConditions, bool allTransitionsNeedsToMatch)
        {
            TransitionState = transitionState;

            TransitionConditions = transitionConditions;

            AllConditionsNeedsToMatch = allTransitionsNeedsToMatch;
        }

#if UNITY_EDITOR
        public void ValidateInspector()
        {
            if (TransitionState)
            {
                name = $"Transition To - {TransitionState.name}";
            }
        }
#endif
    }
}