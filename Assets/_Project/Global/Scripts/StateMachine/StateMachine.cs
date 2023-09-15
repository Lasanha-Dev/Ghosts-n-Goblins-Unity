using EntitieComponentsReferences = Game.Entities.EntitieComponentsReferences;

using SerializeField = UnityEngine.SerializeField;

using RequireComponent = UnityEngine.RequireComponent;

using System.Collections.Generic;

namespace Game.StateMachine
{
    [RequireComponent(typeof(EntitieComponentsReferences))]

    public sealed class StateMachine : UnityEngine.MonoBehaviour
    {
        [SerializeField] private StateMachineStates _stateMachineStates;

        [SerializeField] private StateMachineParametersBase StateMachineParameters;

        private EntitieComponentsReferences _entitieComponentsReferences;

        private StateBase _currentState;

        private void Awake()
        {
            _entitieComponentsReferences = GetComponent<EntitieComponentsReferences>();

            _stateMachineStates = Instantiate(_stateMachineStates);

            InitializeStates();
        }

        private void Start()
        {
            ResetStateMachine();
        }

        private void InitializeStates()
        {
            foreach (StateDefinition stateDefinition in _stateMachineStates.StatesDefinitions)
            {
                InitializeState(stateDefinition.baseState);

                InitializeStateTransitions(stateDefinition.baseState);
            }
        }

        private void InitializeStateTransitions(StateBase state)
        {
            foreach (StateTransitions transition in state.StateTransitions)
            {
                InitializeState(transition.TransitionState);

                InitializeTransitionConditions(transition.TransitionConditions);
            }
        }

        private void InitializeState(StateBase state)
        {
            state.InitializeState(StateMachineParameters, _entitieComponentsReferences);
        }

        private void InitializeTransitionConditions(IEnumerable<TransitionConditionBase> transitionConditions)
        {
            foreach (TransitionConditionBase condition in transitionConditions)
            {
                condition.InitializeCondition(_entitieComponentsReferences);
            }
        }

        private void ResetStateMachine()
        {
            _currentState = _stateMachineStates.StatesDefinitions.First.Value.baseState;

            SwitchState(_currentState);
        }

        private void Update()
        {
            UpdateStateMachine();
        }

        private void LateUpdate()
        {
            StateBase nextState = CheckCurrentStateTransitions();

            if (nextState != null)
            {
                SwitchState(nextState);
            }
        }

        private void FixedUpdate()
        {
            FixedUpdateStateMachine();
        }

        public void UpdateStateMachine()
        {
            _currentState.OnUpdate();
        }

        public void FixedUpdateStateMachine()
        {
            _currentState.OnFixedUpdate();
        }

        private void SwitchState(StateBase nextState)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = nextState;

            _currentState.OnEnter();
        }

        private StateBase CheckCurrentStateTransitions()
        {
            foreach (StateTransitions transition in _currentState.StateTransitions)
            {
                if (transition.AllConditionsNeedsToMatch == false && AnyTransitionConditionMatched(transition))
                {
                    return transition.TransitionState;
                }

                if (transition.AllConditionsNeedsToMatch && AllTransitionConditionsMatched(transition))
                {
                    return transition.TransitionState;
                }
            }

            bool AllTransitionConditionsMatched(StateTransitions transition)
            {
                foreach (TransitionConditionBase condition in transition.TransitionConditions)
                {
                    if (!condition.CheckTransition())
                    {
                        return false;
                    }
                }

                return true;
            }

            bool AnyTransitionConditionMatched(StateTransitions transitions)
            {
                foreach (TransitionConditionBase condition in transitions.TransitionConditions)
                {
                    if (condition.CheckTransition())
                    {
                        return true;
                    }
                }

                return false;
            }

            return null;
        }
    }
}