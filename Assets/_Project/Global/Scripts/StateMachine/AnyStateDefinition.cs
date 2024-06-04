using SerializeField = UnityEngine.SerializeField;

namespace Game.StateMachine
{
    [System.Serializable]
    public sealed class AnyStateDefinition
    {
#if UNITY_EDITOR
        [UnityEngine.HideInInspector]
        public string name;
#endif
        [field: SerializeField] public StateTransition StateTransition { get; set; }

#if UNITY_EDITOR
        public void ValidateInspector()
        {
            if (StateTransition.TransitionState != null)
            {
                name = StateTransition.TransitionState.name;
            }
        }
#endif
    }
}