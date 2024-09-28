using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

namespace Game.StateMachine
{
    public abstract class TransitionLogic : UnityEngine.ScriptableObject
    {
        public virtual void SetupLogic(EntityComponentsReferences entityComponentsReferences) { }

        public abstract void ExecuteLogic();
    }
}
