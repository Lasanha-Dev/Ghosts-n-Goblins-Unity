using UnityEngine;

namespace Game.Entities.Player
{
    public sealed class PlayerRotationController : MonoBehaviour
    {
        private PlayerInputsController _playerInputsController;

        private Quaternion _targetRotation = Quaternion.identity;

        private readonly Quaternion _leftSideRotation = Quaternion.Euler(0, 180, 0);

        private readonly Quaternion _rightSideRotation = Quaternion.identity;

        private void Awake()
        {
            _playerInputsController = GetComponent<PlayerInputsController>();
        }

        private void LateUpdate()
        {
            transform.rotation = GetTargetRotation();
        }

        private Quaternion GetTargetRotation()
        {
            if (_playerInputsController.PlayerMovementInputValue > 0)
            {
                return _targetRotation = _rightSideRotation;
            }

            if (_playerInputsController.PlayerMovementInputValue < 0)
            {
                return _targetRotation = _leftSideRotation;
            }

            return _targetRotation;
        }
    }
}
