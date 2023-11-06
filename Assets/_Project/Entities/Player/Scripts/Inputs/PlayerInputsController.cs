using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerInputsController : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        public InputDefinition JumpInput { get; private set; }

        public InputDefinition<float> MovementInput { get; private set; }

        public InputDefinition<float> LadderInput { get; private set; }

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Enable();

            InitializePlayerInputs();
        }

        private void InitializePlayerInputs()
        {
            JumpInput = new InputDefinition(_playerInputActions.Player.Jump);

            MovementInput = new InputDefinition<float>(_playerInputActions.Player.Movement);

            LadderInput = new InputDefinition<float>(_playerInputActions.Player.Ladder);
        }
    }
}