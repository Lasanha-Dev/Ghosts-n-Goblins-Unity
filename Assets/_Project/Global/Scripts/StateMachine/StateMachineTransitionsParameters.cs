using Object = System.Object;

using Type = System.Type;

using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
#endif

namespace Game.StateMachine
{
    [CreateAssetMenu(fileName = "StateMachineTransitionsParameters", menuName = "StateMachine/StateMachineTransitionsParameters")]
    public sealed class StateMachineTransitionsParameters : ScriptableObject
    {
        [SerializeReference] private List<Object> _transitionsParameters = new List<Object>();

        private readonly Dictionary<Type, Object> _parameters = new Dictionary<Type, Object>();

        private void OnEnable()
        {
            InitializeParameters();
        }

        private void InitializeParameters()
        {
            foreach (Object parameterObject in _transitionsParameters)
            {
                _parameters.Add(parameterObject.GetType(), parameterObject);
            }
        }

        public T GetParameterObject<T>()
        {
            return (T)_parameters[typeof(T)];
        }

#if UNITY_EDITOR
        [SerializeField] private StateMachineStates _stateMachineStates;

        public void OnValidate()
        {
            if (_stateMachineStates == null)
            {
                return;
            }

            HashSet<Type> currentObjectsParameters = new HashSet<Type>();

            List<Type> allCurrentParametersObjects = new List<Type>();

            FillHashSet();

            foreach (StateDefinition stateDefinition in _stateMachineStates.StatesDefinitionSetup)
            {
                if (stateDefinition.StateTransitions == null)
                {
                    continue;
                }

                foreach (StateTransition stateTransition in stateDefinition.StateTransitions)
                {
                    foreach (TransitionConditionBase transitionConditionBase in stateTransition.TransitionConditions)
                    {
                        if (transitionConditionBase == null)
                        {
                            continue;
                        }

                        Object transitionParameterObject = transitionConditionBase.GetTransitionParameterObject();

                        if (transitionParameterObject is null)
                        {
                            continue;
                        }

                        allCurrentParametersObjects.Add(transitionParameterObject.GetType());

                        if (currentObjectsParameters.Contains(transitionParameterObject.GetType()))
                        {
                            continue;
                        }

                        currentObjectsParameters.Add(transitionParameterObject.GetType());

                        _transitionsParameters.Add(transitionParameterObject);
                    }
                }
            }

            foreach (Object @object in _transitionsParameters.ToList())
            {
                if (allCurrentParametersObjects.Contains(@object.GetType()))
                {
                    continue;
                }

                _transitionsParameters.Remove(@object);
            }

            void FillHashSet()
            {
                foreach (Object @object in _transitionsParameters)
                {
                    currentObjectsParameters.Add(@object.GetType());
                }
            }
        }

        [ContextMenu("Reset Parameters")]
        private void ResetParameters()
        {
            _transitionsParameters.Clear();

            OnValidate();
        }
#endif
    }
}