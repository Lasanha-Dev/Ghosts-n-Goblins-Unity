using UnityEngine;

using System.Collections.Generic;

using Type = System.Type;

namespace Game.Entities
{
    public sealed class EntitieComponentsReferences : MonoBehaviour
    {
        private readonly Dictionary<Type, Component> _entitieCachedComponents = new Dictionary<Type, Component>();

        public T GetEntitieComponent<T>() where T : Component
        {
            System.Type componentType = typeof(T);

            if (!_entitieCachedComponents.ContainsKey(componentType))
            {
                T component = GetComponentInChildren<T>();

                _entitieCachedComponents[componentType] = component;

                return component;
            }

            return (T)_entitieCachedComponents[componentType];
        }
    }
}