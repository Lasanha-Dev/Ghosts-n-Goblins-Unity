using UnityEngine;

namespace Game
{
    public sealed class DisableObjectOnCollisionEnter : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDisable;

        [SerializeField] private LayerMask _collisionMask;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsInLayerMask(collision.gameObject.layer, _collisionMask))
            {
                _objectToDisable.SetActive(false);
            }
        }

        public static bool IsInLayerMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;
    }
}
