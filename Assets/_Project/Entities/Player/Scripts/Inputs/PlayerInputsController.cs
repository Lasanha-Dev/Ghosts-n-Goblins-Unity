using UnityEngine;

using UnityEngine.InputSystem;

namespace Game.Entities.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputsController : MonoBehaviour
    {
        [field: SerializeField] public InputActionReference PlayerJumpAction { get; private set; }

        public float PlayerMovementInputValue { get; private set; }

        private void OnMovement(InputValue context)
        {
            PlayerMovementInputValue = context.Get<float>();
        }
    }
}