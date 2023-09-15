using System.Collections.Generic;

using UnityEngine;

using Type = System.Type;

namespace Game.StateMachine
{
    [CreateAssetMenu(fileName = "StateMachineStates", menuName = "StateMachine/StateMachineStates")]
    public sealed class StateMachineStates : ScriptableObject
    {
        [SerializeField] private List<StateDefinition> _statesDefinition;

        public LinkedList<StateDefinition> StatesDefinitions { get; private set; } = new LinkedList<StateDefinition>();

        private readonly Dictionary<Type, Object> _cachedObjectsInstances = new Dictionary<Type, Object>();

        private void Awake()
        {
            _cachedObjectsInstances.Clear();

            foreach (StateDefinition stateDefinition in _statesDefinition)
            {
                StateDefinition newStateDefinition = new StateDefinition();

                InstantiateBaseState(stateDefinition, newStateDefinition);

                newStateDefinition.baseState.SetStateTransitions(InstantiateStateTransitions(stateDefinition));

                StatesDefinitions.AddLast(newStateDefinition);
            }
        }

        private IReadOnlyCollection<StateTransitions> InstantiateStateTransitions(StateDefinition stateDefinition)
        {
            HashSet<StateTransitions> stateTransitions = new HashSet<StateTransitions>();

            foreach (StateTransitions stateTransition in stateDefinition.stateTransitions)
            {
                stateTransitions.Add(new StateTransitions(GetTransitionStateFromStateTransition(stateTransition), GetTransitionConditionsFromStateTransition(stateTransition), stateTransition.AllConditionsNeedsToMatch));
            }

            return stateTransitions;
        }

        private void InstantiateBaseState(StateDefinition stateDefinition, StateDefinition newStateDefinition)
        {
            if (_cachedObjectsInstances.ContainsKey(stateDefinition.baseState.GetType()) == false)
            {
                newStateDefinition.baseState = Instantiate(stateDefinition.baseState);

                _cachedObjectsInstances.Add(stateDefinition.baseState.GetType(), newStateDefinition.baseState);

                return;
            }

            newStateDefinition.baseState = (StateBase)_cachedObjectsInstances[stateDefinition.baseState.GetType()];
        }

        private StateBase GetTransitionStateFromStateTransition(StateTransitions stateTransition)
        {
            if (_cachedObjectsInstances.ContainsKey(stateTransition.TransitionState.GetType()) == false)
            {
                StateBase transitionState;

                transitionState = Instantiate(stateTransition.TransitionState);

                _cachedObjectsInstances.Add(transitionState.GetType(), transitionState);
            }

            return (StateBase)_cachedObjectsInstances[stateTransition.TransitionState.GetType()];
        }

        private IReadOnlyCollection<TransitionConditionBase> GetTransitionConditionsFromStateTransition(StateTransitions stateTransition)
        {
            HashSet<TransitionConditionBase> transitions = new HashSet<TransitionConditionBase>();

            foreach (TransitionConditionBase transitionCondition in stateTransition.TransitionConditions)
            {
                if (_cachedObjectsInstances.ContainsKey(transitionCondition.GetType()) == false)
                {
                    TransitionConditionBase transitionConditionInstance = Instantiate(transitionCondition);

                    transitions.Add(transitionConditionInstance);

                    _cachedObjectsInstances.Add(transitionCondition.GetType(), transitionConditionInstance);

                    continue;
                }

                transitions.Add((TransitionConditionBase)_cachedObjectsInstances[transitionCondition.GetType()]);
            }

            return transitions;
        }

        private void OnValidate()
        {
            foreach (StateDefinition stateDefinition in _statesDefinition)
            {
                stateDefinition.ValidateInspector();
            }
        }
    }
}