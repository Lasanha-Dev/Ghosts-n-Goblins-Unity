using UnityEngine;

namespace Game.ServiceLocator.Service
{
    public sealed class PlayerWeaponsService : MonoBehaviour, IPlayerWeaponsService
    {
        [SerializeField] private Animator _playerAnimator;

        [SerializeField] private Transform _standingWeaponSpawnTransform;

        [SerializeField] private Transform _crouchedWeaponSpawnTransform;

        public Vector3 DesiredWeaponSpawnPoint => GetDesiredWeaponSpawnPoint();

        public EWeaponType CurrentEquippedWeapon => _currentEquippedWeapon;

        [SerializeField] private EWeaponType _currentEquippedWeapon = EWeaponType.Lance;

        public PlayerWeaponsService()
        {
            GameServiceLocator.RegisterGameService<IPlayerWeaponsService>(this);
        }

        private Vector3 GetDesiredWeaponSpawnPoint()
        {
            bool isCrouched = _playerAnimator.GetBool(PlayerAnimatorHashes.PLAYER_CROUCH_PARAM_BOOL_ID);

            if (isCrouched)
            {
                return _crouchedWeaponSpawnTransform.position;
            }

            return _standingWeaponSpawnTransform.position;
        }

        private void OnDestroy()
        {
            GameServiceLocator.UnregisterGameService<IPlayerWeaponsService>();
        }
    }
}
