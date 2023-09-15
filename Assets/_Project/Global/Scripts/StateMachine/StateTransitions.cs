using System.Collections.Generic;

using System.Linq;

using SerializeField = UnityEngine.SerializeField;

namespace Game.StateMachine
{
    [System.Serializable]
    public sealed class StateTransitions
    {
        [UnityEngine.HideInInspector]
        public string name;

        [field: SerializeField] public StateBase TransitionState { get; private set; }

        [field: SerializeField] public List<TransitionConditionBase> TransitionConditions { get; private set; }

        [field: SerializeField] public bool AllConditionsNeedsToMatch { get; private set; }

        public StateTransitions(StateBase transitionState, IEnumerable<TransitionConditionBase> transitionConditions, bool allTransitionsNeedsToMatch)
        {
            TransitionState = transitionState;

            TransitionConditions = transitionConditions.ToList();

            AllConditionsNeedsToMatch = allTransitionsNeedsToMatch;
        }

        public void ValidateInspector()
        {
            if (TransitionState)
            {
                name = $"Transition To - {TransitionState.name}";
            }
        }
    }
}