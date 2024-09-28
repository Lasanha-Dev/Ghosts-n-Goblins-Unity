using System;
using System.Collections.Generic;


namespace Game.GameManagement.ObjectPooling
{
    public sealed class WeaponsPoolsGuids
    {
        private static readonly Guid LancePoolGuid = Guid.NewGuid();

        private static readonly Guid DaggerPoolGuid = Guid.NewGuid();

        private static readonly Guid TorchPoolGuid = Guid.NewGuid();

        private static readonly Guid AxePoolGuid = Guid.NewGuid();

        private static readonly Guid ShieldPoolGuid = Guid.NewGuid();


        private static readonly Dictionary<EWeaponType, Guid> _weaponsGuids = new Dictionary<EWeaponType, Guid>()
        {
            [EWeaponType.Lance] = LancePoolGuid,

            [EWeaponType.Dagger] = DaggerPoolGuid,

            [EWeaponType.Torch] = TorchPoolGuid,

            [EWeaponType.Axe] = AxePoolGuid,

            [EWeaponType.Shield] = ShieldPoolGuid,
        };

        public static IReadOnlyDictionary<EWeaponType, Guid> WeaponsGuids => _weaponsGuids;
    }
}
