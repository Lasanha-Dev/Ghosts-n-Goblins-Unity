using UnityEngine;

namespace Game
{
    public sealed class DisableObjectWhenInvisible : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDisable;

        private void OnBecameInvisible()
        {
            _objectToDisable.SetActive(false);
        }
    }
}
