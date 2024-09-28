using GameObject = UnityEngine.GameObject;

using Component = UnityEngine.Component;

using Transform = UnityEngine.Transform;

using Object = UnityEngine.Object;

using Vector3 = UnityEngine.Vector3;

using Quaternion = UnityEngine.Quaternion;

using Guid = System.Guid;

using System.Collections.Generic;

namespace Game.GameManagement.ObjectPooling
{
    public static class ObjectPool
    {
        private static readonly Dictionary<Guid, Pool> _poolsDictionary = new Dictionary<Guid, Pool>();

        private const int DEFAULT_POOL_SIZE = 10;

        public static void PreAllocatePool(Guid poolGuid, Object poolTemplateObject, Transform defaultParent = null, int poolSize = DEFAULT_POOL_SIZE)
        {
            if (_poolsDictionary.ContainsKey(poolGuid) == false)
            {
                RegisterNewPool(poolGuid, poolTemplateObject, defaultParent, poolSize);
            }
        }

        public static Object PullObject(Guid poolGuid, Object objectTemplate)
        {
            if (_poolsDictionary.ContainsKey(poolGuid))
            {
                return RetrievePooledObject(poolGuid);
            }

            RegisterNewPool(poolGuid, objectTemplate);

            return RetrievePooledObject(poolGuid);
        }

        public static GameObject PullObject(Guid poolGuid, GameObject objectTemplate)
        {
            if (_poolsDictionary.ContainsKey(poolGuid))
            {
                return RetrievePooledObject(poolGuid).GameObject();
            }

            RegisterNewPool(poolGuid, objectTemplate);

            return RetrievePooledObject(poolGuid).GameObject();
        }

        private static Object RetrievePooledObject(Guid poolGuid)
        {
            return _poolsDictionary[poolGuid].PullObject();
        }

        private static void RegisterNewPool(Guid poolGuid, Object poolObject, Transform defaultParent = null, int poolSize = DEFAULT_POOL_SIZE)
        {
            Pool newPoolInstance = new Pool(poolObject, defaultParent, poolSize);

            _poolsDictionary.Add(poolGuid, newPoolInstance);
        }

        private sealed class Pool
        {
            private readonly Queue<Object> _poolQueue = new Queue<Object>();

            private readonly Object _poolObjectType;

            private readonly Dictionary<int, PoolableObject> _poolableObjectsDictionary = new Dictionary<int, PoolableObject>();

            private readonly Transform _poolParentTransform;

            public Pool(Object poolObjectType, Transform defaultParent, int poolSize)
            {
                _poolParentTransform = defaultParent;

                _poolObjectType = poolObjectType;

                InitializePool(poolSize);
            }

            public Object PullObject()
            {
                if (_poolQueue.Count <= 0)
                {
                    InstantiateNewPoolObject();
                }

                Object pooledInstance = null;

                while (pooledInstance == null)
                {
                    Object nextQueueInstance = _poolQueue.Dequeue();

                    if (nextQueueInstance.GameObject().activeInHierarchy)
                    {
                        _poolQueue.Enqueue(nextQueueInstance);

                        continue;
                    }

                    pooledInstance = nextQueueInstance;
                }

                _poolableObjectsDictionary[pooledInstance.GameObject().GetInstanceID()].ReturnToPool += OnReturnToPool;

                return pooledInstance;
            }

            private void OnReturnToPool(Object returnedItem)
            {
                _poolQueue.Enqueue(returnedItem);

                _poolableObjectsDictionary[returnedItem.GameObject().GetInstanceID()].ReturnToPool -= OnReturnToPool;
            }

            private void InitializePool(int poolSize)
            {
                for (int i = 0; i < poolSize; i++)
                {
                    InstantiateNewPoolObject();
                }
            }

            private void InstantiateNewPoolObject()
            {
                Object objectInstance = Object.Instantiate(_poolObjectType, Vector3.zero, Quaternion.identity, _poolParentTransform);

                objectInstance.GameObject().transform.localPosition = Vector3.zero;

                InitializePoolObject(objectInstance);

                objectInstance.GameObject().SetActive(false);

                _poolQueue.Enqueue(objectInstance);
            }

            private void InitializePoolObject(Object objectInstance)
            {
                PoolableObject poolableObject = GetPoolableObjectComponent(objectInstance);

                _poolableObjectsDictionary.Add(objectInstance.GameObject().GetInstanceID(), poolableObject);
            }

            private PoolableObject GetPoolableObjectComponent(Object objectInstance)
            {
                if(objectInstance.GameObject().TryGetComponent<PoolableObject>(out PoolableObject poolableObjectComponent))
                {
                    return poolableObjectComponent;
                }

                return objectInstance.GameObject().AddComponent<PoolableObject>();
            }
        }

        private static GameObject GameObject(this Object @object)
        {
            if (@object is GameObject gameObject)
            {
                return gameObject;
            }

            if (@object is Component component)
            {
                return component.gameObject;
            }

            return null;
        }
    }
}