using UnityEngine;

namespace Game.Entities.Player
{
    public static class PlayerInputsController
    {
        private static PlayerInputActions _playerInputActions;

        public static InputDefinition JumpInput { get; private set; }

        public static InputDefinition<float> MovementInput { get; private set; }

        public static InputDefinition<float> LadderInput { get; private set; }

        public static InputDefinition CrouchInput { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitializePlayerInputs()
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Enable();

            JumpInput = new InputDefinition(_playerInputActions.Player.Jump);

            MovementInput = new InputDefinition<float>(_playerInputActions.Player.Movement);

            LadderInput = new InputDefinition<float>(_playerInputActions.Player.Ladder);

            CrouchInput = new InputDefinition(_playerInputActions.Player.Crouch);
        }
    }
}