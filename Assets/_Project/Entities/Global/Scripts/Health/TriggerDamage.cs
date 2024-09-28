using Game.Entities.Global;

using UnityEngine;

namespace Game
{
    public sealed class TriggerDamage : MonoBehaviour
    {
        [SerializeField] private uint _damageAmount;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
            {
                healthComponent.ApplyDamage(_damageAmount);
            }
        }
    }
}
