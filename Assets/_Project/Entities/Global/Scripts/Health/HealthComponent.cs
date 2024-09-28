using System;

using UnityEngine;

namespace Game.Entities.Global
{
    public sealed class HealthComponent : MonoBehaviour
    {
        [SerializeField] private uint _maxHealth;

        private uint _currentHealth;

        public event Action ReceivedDamage;

        public event Action Died;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void ApplyDamage(uint DamageAmount)
        {
            _currentHealth -= DamageAmount;

            if(_currentHealth <= 0)
            {
                Died?.Invoke();

                return;
            }

            ReceivedDamage?.Invoke();
        }
    }
}