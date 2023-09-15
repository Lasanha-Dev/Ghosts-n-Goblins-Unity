using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerStateMachineParameters", menuName = "StateMachine/Player/Parameters")]
    public sealed class PlayerStateMachineParameters : StateMachineParametersBase
    {
        [field: SerializeField, Min(0)] public float MovementSpeed { get; private set; }
    }
}