using UnityEngine;

using System.Collections.Generic;

using Type = System.Type;

namespace Game.Entities
{
    public sealed class EntityComponentsReferences : MonoBehaviour
    {
        private readonly Dictionary<Type, Component> _entityCachedComponents = new Dictionary<Type, Component>();

        public T GetEntityComponent<T>() where T : Component
        {
            Type componentType = typeof(T);
            
            if (_entityCachedComponents.ContainsKey(componentType) == false)
            {
                T component = GetComponentInChildren<T>(true);

                _entityCachedComponents.Add(componentType, component);

                return component;
            }

            return (T)_entityCachedComponents[componentType];
        }
    }
}