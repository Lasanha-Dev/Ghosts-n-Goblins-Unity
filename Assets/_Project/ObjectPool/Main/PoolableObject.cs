using MonoBehaviour = UnityEngine.MonoBehaviour;

using Object = UnityEngine.Object;

namespace Game.GameManagement.ObjectPooling
{
    public sealed class PoolableObject : MonoBehaviour
    {
        public event System.Action<Object> ReturnToPool;

        private void OnDisable()
        {
            ReturnToPool?.Invoke(this);
        }
    }
}