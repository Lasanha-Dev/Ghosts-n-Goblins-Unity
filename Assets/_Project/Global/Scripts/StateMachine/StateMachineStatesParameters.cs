using Object = System.Object;

using Type = System.Type;

using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using System.Linq;
#endif

namespace Game.StateMachine
{
    [CreateAssetMenu(fileName = "StateMachineStatesParameters", menuName = "StateMachine/StateMachineStatesParameters")]
    public sealed class StateMachineStatesParameters : ScriptableObject
    {
        [SerializeReference] private List<Object> _statesParameters = new List<Object>();

        private readonly Dictionary<Type, Object> _parameters = new Dictionary<Type, Object>();

        private void OnEnable()
        {
            InitializeParameters();
        }

        private void InitializeParameters()
        {
            foreach (Object parameterObject in _statesParameters)
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
            if(_stateMachineStates == null)
            {
                return;
            }

            HashSet<Type> currentObjectsParameters = new HashSet<Type>();

            List<Type> allCurrentParametersObjects = new List<Type>();

            FillHashSet();

            foreach (StateDefinition stateDefinition in _stateMachineStates.StatesDefinitionSetup)
            {
                if (stateDefinition.BaseState == null)
                {
                    continue;
                }

                Object stateParameterObject = stateDefinition.BaseState.GetStateParameterObject();

                if (stateParameterObject is null)
                {
                    continue;
                }

                allCurrentParametersObjects.Add(stateParameterObject.GetType());

                if (currentObjectsParameters.Contains(stateParameterObject.GetType()))
                {
                    continue;
                }

                currentObjectsParameters.Add(stateParameterObject.GetType());

                _statesParameters.Add(stateParameterObject);
            }

            foreach (Object @object in _statesParameters.ToList())
            {
                if (allCurrentParametersObjects.Contains(@object.GetType()))
                {
                    continue;
                }

                _statesParameters.Remove(@object);
            }

            void FillHashSet()
            {
                foreach (Object @object in _statesParameters)
                {
                    currentObjectsParameters.Add(@object.GetType());
                }
            }
        }

        [ContextMenu("Reset Parameters")]
        private void ResetParameters()
        {
            _statesParameters.Clear();

            OnValidate();
        }
#endif
    }
}