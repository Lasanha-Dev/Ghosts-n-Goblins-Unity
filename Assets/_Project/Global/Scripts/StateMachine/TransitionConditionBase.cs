using EntitieComponentsReferences = Game.Entities.EntitieComponentsReferences;

namespace Game.StateMachine
{
    public abstract class TransitionConditionBase : UnityEngine.ScriptableObject
    {
        private bool _isInitialized = false;

        private void OnEnable()
        {
            _isInitialized = false;
        }

        public void InitializeCondition(EntitieComponentsReferences entitieComponentsReferences)
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            SetupCondition(entitieComponentsReferences);
        }

        protected abstract void SetupCondition(EntitieComponentsReferences playerComponentsReferences);

        public abstract bool CheckTransition();
    }
}