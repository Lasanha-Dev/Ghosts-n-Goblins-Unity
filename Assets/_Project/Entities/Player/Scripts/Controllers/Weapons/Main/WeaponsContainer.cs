using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Weapons Container", menuName = "Weapons")]
    public sealed class WeaponsContainer : ScriptableObject
    {
        [SerializeField] private WeaponDefinition[] _weaponsDefinition;

        public IReadOnlyDictionary<EWeaponType, WeaponDefinition> WeaponDefinitions;

        private readonly Dictionary<EWeaponType, WeaponDefinition> _weapons = new Dictionary<EWeaponType, WeaponDefinition>();

        private void OnEnable()
        {
            if(_weaponsDefinition == null)
            {
                return;
            }

            foreach (WeaponDefinition weapon in _weaponsDefinition)
            {
                _weapons.Add(weapon.WeaponType, weapon);
            }

            WeaponDefinitions = _weapons;
        }

        [Serializable]
        public sealed class WeaponDefinition
        {
            [field: SerializeField] public EWeaponType WeaponType { get; private set; }

            [field: SerializeField] public GameObject WeaponPrefab { get; private set; }
        }

    }

    //TODO: Move This From Here
    public enum EWeaponType
    {
        Lance,
        Dagger,
        Torch,
        Axe,
        Shield
    }
}
