using InputAction = UnityEngine.InputSystem.InputAction;

namespace Game.Entities.Player
{
    public sealed class InputDefinition<TInputValueType> where TInputValueType : struct
    {
        private readonly InputAction _inputAction;

        public bool WasPressedThisFrame => _inputAction.WasPressedThisFrame();

        public bool IsPressed => _inputAction.IsPressed();

        public TInputValueType InputValue => _inputAction.ReadValue<TInputValueType>();

        public InputDefinition(InputAction inputAction)
        {
            _inputAction = inputAction;

            _inputAction.Enable();
        }
    }

    public sealed class InputDefinition
    {
        private readonly InputAction _inputAction;

        public bool WasPressedThisFrame => _inputAction.WasPressedThisFrame();

        public bool IsPressed => _inputAction.IsPressed();

        public InputDefinition(InputAction inputAction)
        {
            _inputAction = inputAction;

            _inputAction.Enable();
        }
    }
}