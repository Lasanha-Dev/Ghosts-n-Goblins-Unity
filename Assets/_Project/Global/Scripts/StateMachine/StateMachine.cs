using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using SerializeField = UnityEngine.SerializeField;

using RequireComponent = UnityEngine.RequireComponent;

using System.Collections.Generic;

namespace Game.StateMachine
{
    [RequireComponent(typeof(EntityComponentsReferences))]
    public sealed class StateMachine : UnityEngine.MonoBehaviour
    {
        [SerializeField] private StateMachineStates _stateMachineStates;

        [SerializeField] private StateMachineStatesParameters _stateMachineStatesParameters;

        [SerializeField] private StateMachineTransitionsParameters _stateMachineTransitionsParameters;

        private EntityComponentsReferences _entityComponentsReferences;

        private StateBase _currentState;

        private void Awake()
        {
            _entityComponentsReferences = GetComponent<EntityComponentsReferences>();

            _stateMachineStates = Instantiate(_stateMachineStates);

            InitializeStateMachine();
        }

        private void OnEnable()
        {
            ResetStateMachine();
        }

        private void Update()
        {
            _currentState.OnUpdate();
        }

        private void LateUpdate()
        {
            StateTransition stateTransition = null;

            if (CanTransitToAnyState(out StateBase anyNextState, out stateTransition))
            {
                SwitchState(anyNextState, stateTransition);

                return;
            }

            if (CanTransitToAnotherState(out StateBase nextState, out stateTransition))
            {
                SwitchState(nextState, stateTransition);
            }
        }

        private void FixedUpdate()
        {
            _currentState.OnFixedUpdate();
        }

        private void InitializeStateMachine()
        {
            foreach (StateDefinition stateDefinition in _stateMachineStates.StatesDefinitions)
            {
                InitializeState(stateDefinition.BaseState);

                InitializeStateTransitions(stateDefinition.BaseState);
            }

            foreach (AnyStateDefinition anyStateDefinition in _stateMachineStates.AnyStatesDefinitions)
            {
                InitializeStateTransitions(anyStateDefinition.StateTransition.TransitionState);
            }
        }

        private void InitializeStateTransitions(StateBase state)
        {
            foreach (StateTransition transition in state.StateTransitions)
            {
                InitializeState(transition.TransitionState);

                InitializeTransitionConditions(transition.TransitionConditions);
            }
        }

        private void InitializeState(StateBase state)
        {
            state.SetupState(_stateMachineStatesParameters, _entityComponentsReferences);

            state.SetupTransitionLogics(_entityComponentsReferences);
        }

        private void InitializeTransitionConditions(IEnumerable<TransitionConditionBase> transitionConditions)
        {
            foreach (TransitionConditionBase condition in transitionConditions)
            {
                condition.SetupCondition(_stateMachineTransitionsParameters, _entityComponentsReferences);
            }
        }

        private void ResetStateMachine()
        {
            _currentState = _stateMachineStates.StatesDefinitions.First.Value.BaseState;

            _currentState.OnEnter();
        }

        private void SwitchState(StateBase nextState, StateTransition stateTransition)
        {
            _currentState.OnExit();

            foreach (TransitionLogic transitionLogic in stateTransition.TransitionLogics)
            {
                transitionLogic.ExecuteLogic();
            }

            _currentState = nextState;

            _currentState.OnEnter();
        }

        private bool CanTransitToAnotherState(out StateBase state, out StateTransition stateTransition)
        {
            state = null;

            stateTransition = null;

            foreach (StateTransition transition in _currentState.StateTransitions)
            {
                if (transition.AllConditionsNeedsToMatch == false && AnyTransitionConditionMatched(transition))
                {
                    state = transition.TransitionState;

                    stateTransition = transition;

                    return true;
                }

                if (transition.AllConditionsNeedsToMatch && AllTransitionConditionsMatched(transition))
                {
                    state = transition.TransitionState;

                    stateTransition = transition;

                    return true;
                }
            }

            return false;
        }

        private bool CanTransitToAnyState(out StateBase state, out StateTransition stateTransition)
        {
            state = null;

            stateTransition = null;

            foreach (AnyStateDefinition anyStateDefinition in _stateMachineStates.AnyStatesDefinitions)
            {
                if (anyStateDefinition.StateTransition.AllConditionsNeedsToMatch == false && AnyTransitionConditionMatched(anyStateDefinition.StateTransition))
                {
                    state = anyStateDefinition.StateTransition.TransitionState;

                    stateTransition = anyStateDefinition.StateTransition;

                    return true;
                }

                if (anyStateDefinition.StateTransition.AllConditionsNeedsToMatch && AllTransitionConditionsMatched(anyStateDefinition.StateTransition))
                {
                    state = anyStateDefinition.StateTransition.TransitionState;

                    stateTransition = anyStateDefinition.StateTransition;

                    return true;
                }
            }

            return false;
        }

        private bool AllTransitionConditionsMatched(StateTransition transition)
        {
            foreach (TransitionConditionBase condition in transition.TransitionConditions)
            {
                if (condition.CanTransit() == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool AnyTransitionConditionMatched(StateTransition transitions)
        {
            foreach (TransitionConditionBase condition in transitions.TransitionConditions)
            {
                if (condition.CanTransit())
                {
                    return true;
                }
            }

            return false;
        }
    }
}