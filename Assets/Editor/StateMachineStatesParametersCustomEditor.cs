using UnityEngine;
using UnityEditor;
using Game.StateMachine;

[CustomEditor(typeof(StateMachineStatesParameters))]
public sealed class StateMachineStatesParametersCustomEditor : Editor
{
    private SerializedProperty _statesParametersList;

    private const string STATES_PARAMETERS_PROPERTY_NAME = "_statesParameters";

    private const string STATE_MACHINE_STATES_PROPERTY_NAME = "_stateMachineStates";

    private void OnEnable()
    {
        _statesParametersList = serializedObject.FindProperty(STATES_PARAMETERS_PROPERTY_NAME);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty(STATE_MACHINE_STATES_PROPERTY_NAME));

        EditorGUILayout.Space(10);

        for (int i = 0; i < _statesParametersList.arraySize; i++)
        {
            SerializedProperty element = _statesParametersList.GetArrayElementAtIndex(i);

            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(element, new GUIContent(element.displayName), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}