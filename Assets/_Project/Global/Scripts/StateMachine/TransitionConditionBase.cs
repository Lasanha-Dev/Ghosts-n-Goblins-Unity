using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

namespace Game.StateMachine
{
    public abstract class TransitionConditionBase : UnityEngine.ScriptableObject
    {
        public virtual void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences) { }

        public abstract bool CanTransit();

        public virtual object GetTransitionParameterObject()
        {
            return null;
        }
    }
}