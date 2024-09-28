using System.Collections.Generic;

using UnityEngine;

using Type = System.Type;

using Object = System.Object;

using System.Linq;

namespace Game.StateMachine
{
    [CreateAssetMenu(fileName = "StateMachineStates", menuName = "StateMachine/StateMachineStates")]
    public sealed class StateMachineStates : ScriptableObject
    {
        [field: SerializeField] public List<StateDefinition> StatesDefinitionSetup { get; private set; }

        [field: SerializeField] public List<AnyStateDefinition> AnyStatesDefinitionSetup { get; private set; }

        public LinkedList<StateDefinition> StatesDefinitions { get; private set; } = new LinkedList<StateDefinition>();

        public LinkedList<AnyStateDefinition> AnyStatesDefinitions { get; private set; } = new LinkedList<AnyStateDefinition>();

        private readonly Dictionary<Type, Object> _cachedObjectsInstances = new Dictionary<Type, Object>();

#if UNITY_EDITOR
        [SerializeField] private StateMachineStatesParameters _stateMachineStatesParameters;

        [SerializeField] private StateMachineTransitionsParameters _stateMachineTransitionsParameters;
#endif
        private void OnEnable()
        {
            _cachedObjectsInstances.Clear();

            if(StatesDefinitionSetup == null)
            {
                return;
            }

            foreach (StateDefinition stateDefinition in StatesDefinitionSetup)
            {
                if (stateDefinition.BaseState == null || stateDefinition.StateTransitions == null)
                {
                    continue;
                }

                StateDefinition newStateDefinition = new StateDefinition(InstantiateStateBase(stateDefinition.BaseState));

                newStateDefinition.BaseState.SetStateTransitions(InstantiateStateTransitions(stateDefinition.StateTransitions));

                StatesDefinitions.AddLast(newStateDefinition);
            }

            foreach (AnyStateDefinition anyStateDefinition in AnyStatesDefinitionSetup)
            {
                if (anyStateDefinition.StateTransition == null || anyStateDefinition.StateTransition.TransitionState == null)
                {
                    continue;
                }

                StateBase stateBase = InstantiateStateBase(anyStateDefinition.StateTransition.TransitionState);

                StateTransition stateTransition = InstantiateStateTransition(anyStateDefinition.StateTransition);

                AnyStateDefinition stateDefinition = new AnyStateDefinition();

                stateDefinition.StateTransition = new StateTransition(stateBase, stateTransition.TransitionConditions, stateTransition.AllConditionsNeedsToMatch, stateTransition.TransitionLogics);

                AnyStatesDefinitions.AddLast(stateDefinition);
            }
        }

        private StateTransition InstantiateStateTransition(StateTransition stateTransitionSetup)
        {
            return new StateTransition(GetTransitionStateFromStateTransition(stateTransitionSetup), GetTransitionConditionsFromStateTransition(stateTransitionSetup), stateTransitionSetup.AllConditionsNeedsToMatch, stateTransitionSetup.TransitionLogics);
        }

        private IReadOnlyList<StateTransition> InstantiateStateTransitions(IEnumerable<StateTransition> stateTransitionsSetup)
        {
            HashSet<StateTransition> stateTransitions = new HashSet<StateTransition>();

            foreach (StateTransition stateTransition in stateTransitionsSetup)
            {
                stateTransitions.Add(new StateTransition(GetTransitionStateFromStateTransition(stateTransition), GetTransitionConditionsFromStateTransition(stateTransition), stateTransition.AllConditionsNeedsToMatch, stateTransition.TransitionLogics));
            }

            return stateTransitions.ToList();
        }

        private StateBase InstantiateStateBase(StateBase stateBase)
        {
            Type stateBaseType = stateBase.GetType();

            if (_cachedObjectsInstances.ContainsKey(stateBaseType) == false)
            {
                _cachedObjectsInstances.Add(stateBaseType, stateBase);
            }

            return (StateBase)_cachedObjectsInstances[stateBaseType];
        }

        private StateBase GetTransitionStateFromStateTransition(StateTransition stateTransition)
        {
            if (_cachedObjectsInstances.ContainsKey(stateTransition.TransitionState.GetType()) == false)
            {
                StateBase transitionState = Instantiate(stateTransition.TransitionState);

                _cachedObjectsInstances.Add(transitionState.GetType(), transitionState);
            }

            return (StateBase)_cachedObjectsInstances[stateTransition.TransitionState.GetType()];
        }

        private List<TransitionConditionBase> GetTransitionConditionsFromStateTransition(StateTransition stateTransition)
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

            return transitions.ToList();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (StateDefinition stateDefinition in StatesDefinitionSetup)
            {
                stateDefinition.ValidateInspector();
            }

            foreach (AnyStateDefinition anyStateDefinition in AnyStatesDefinitionSetup)
            {
                anyStateDefinition.ValidateInspector();
            }

            if (_stateMachineStatesParameters != null)
            {
                _stateMachineStatesParameters.OnValidate();
            }

            if(_stateMachineTransitionsParameters != null)
            {
                _stateMachineTransitionsParameters.OnValidate();
            }
        }
#endif
    }
}