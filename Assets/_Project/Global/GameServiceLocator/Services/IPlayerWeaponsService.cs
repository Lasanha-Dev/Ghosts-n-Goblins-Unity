using UnityEngine;

namespace Game.ServiceLocator.Service
{
    public interface IPlayerWeaponsService : IService
    {
        Vector3 DesiredWeaponSpawnPoint { get; }

        EWeaponType CurrentEquippedWeapon { get; }
    }
}