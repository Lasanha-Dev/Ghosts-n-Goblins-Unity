using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;
using Game.Entities.Global;
using Game.ServiceLocator.Service;
using Game.ServiceLocator;
using Game.GameManagement.ObjectPooling;
using System;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerAttackState", menuName = "StateMachine/Player/States/PlayerAttackState")]
    public sealed class PlayerAttackState : StateBase
    {
        private Animator _playerAnimator;

        private readonly int PLAYER_ATTACK_PARAM_TRIGGER_ID = Animator.StringToHash("Attack");

        private PlayerAttackParameters _playerAttackParameters;

        private IPlayerWeaponsService _playerWeaponsService;

        public override void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();

            _playerAttackParameters = stateMachineStatesParameters.GetParameterObject<PlayerAttackParameters>();

            _playerWeaponsService = GameServiceLocator.GetService<IPlayerWeaponsService>();
        }

        public override void OnEnter()
        {
            _playerAnimator.SetTrigger(PLAYER_ATTACK_PARAM_TRIGGER_ID);

            InstantiateWeapon();
        }

        private void InstantiateWeapon()
        {
            Guid currentWeaponGuid = WeaponsPoolsGuids.WeaponsGuids[_playerWeaponsService.CurrentEquippedWeapon];

            GameObject Weapon = ObjectPool.PullObject(currentWeaponGuid, _playerAttackParameters.WeaponsContainer.WeaponDefinitions[_playerWeaponsService.CurrentEquippedWeapon].WeaponPrefab);

            Weapon.transform.SetPositionAndRotation(_playerWeaponsService.DesiredWeaponSpawnPoint, _playerAnimator.transform.rotation);

            Weapon.SetActive(true);

            Weapon.GetComponent<ThrowableObject>().ApplyMovementDirection(_playerAnimator.transform.right);
        }

        public override object GetStateParameterObject()
        {
            return new PlayerAttackParameters();
        }

        [System.Serializable]
        public sealed class PlayerAttackParameters
        {
            [HideInInspector]
            public string name;

            [field: SerializeField] public WeaponsContainer WeaponsContainer { get; private set; }

            public PlayerAttackParameters()
            {
                name = nameof(PlayerAttackParameters);
            }
        }
    }
}
