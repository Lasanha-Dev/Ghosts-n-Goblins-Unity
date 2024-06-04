using UnityEngine;
using UnityEditor;
using Game.StateMachine;

namespace Game
{
    [CustomEditor(typeof(StateMachineTransitionsParameters))]
    public sealed class StateMachineTransitionParametersCustomEditor : Editor
    {
        private SerializedProperty _transitionParametersList;

        private const string TRANSITION_PARAMETERS_PROPERTY_NAME = "_transitionsParameters";

        private const string STATE_MACHINE_STATES_PROPERTY_NAME = "_stateMachineStates";

        private void OnEnable()
        {
            _transitionParametersList = serializedObject.FindProperty(TRANSITION_PARAMETERS_PROPERTY_NAME);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(STATE_MACHINE_STATES_PROPERTY_NAME));

            EditorGUILayout.Space(10);

            for (int i = 0; i < _transitionParametersList.arraySize; i++)
            {
                SerializedProperty element = _transitionParametersList.GetArrayElementAtIndex(i);

                EditorGUILayout.Space(10);

                EditorGUILayout.PropertyField(element, new GUIContent(element.displayName), true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
