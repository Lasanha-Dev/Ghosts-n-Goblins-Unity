using System;

using System.Collections.Generic;

using RuntimeInitializeOnLoadMethod = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;

using RuntimeInitializeLoadType = UnityEngine.RuntimeInitializeLoadType;

namespace Game.ServiceLocator
{
    public static class GameServiceLocator
    {
        private static readonly Dictionary<string, ServiceDefinition> _gameServices = new Dictionary<string, ServiceDefinition>();

        private static readonly Dictionary<string, Delegate> _serviceAvailabilityCallbacks = new Dictionary<string, Delegate>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeServiceEventListener()
        {
            ServiceDefinition.UpdatedServiceAvailabilityStatus += NotifyUsersAboutServiceAvailability;
        }

        public static void RegisterGameService<T>(T service) where T : IService
        {
            string serviceName = nameof(T);

            if (_gameServices.TryGetValue(serviceName, out ServiceDefinition serviceDefinition))
            {
                serviceDefinition.OverrideService(service);

                return;
            }

            ServiceDefinition newServiceDefinition = new ServiceDefinition(service, serviceName, ServiceAvailabilityStatus.Available);

            _gameServices.Add(serviceName, newServiceDefinition);

            NotifyUsersAboutServiceAvailability(serviceName, ServiceAvailabilityStatus.Available);
        }

        public static void UnregisterGameService<T>() where T : IService
        {
            if (_gameServices.TryGetValue(nameof(T), out ServiceDefinition serviceDefinition))
            {
                serviceDefinition.OverrideService(null);
            }
        }

        public static void AddOnChangeServiceAvailabilityStatusListener<T>(Action<ServiceAvailabilityStatus> listener) where T : IService
        {
            string serviceName = nameof(T);

            AddServiceAvailabilityCallbackListener(serviceName, listener);

            if (_gameServices.TryGetValue(serviceName, out ServiceDefinition service) && service.IsAvailable)
            {
                listener.Invoke(ServiceAvailabilityStatus.Available);

                return;
            }
        }

        private static void AddServiceAvailabilityCallbackListener(string serviceName, Action<ServiceAvailabilityStatus> listener)
        {
            if (_serviceAvailabilityCallbacks.TryGetValue(serviceName, out Delegate value))
            {
                _serviceAvailabilityCallbacks[serviceName] = Delegate.Combine(value, listener);

                return;
            }

            _serviceAvailabilityCallbacks.Add(serviceName, listener);
        }

        public static void RemoveOnChangeServiceAvailabilityStatusListener<T>(Action<ServiceAvailabilityStatus> listener) where T : IService
        {
            if (_serviceAvailabilityCallbacks.TryGetValue(nameof(T), out Delegate value))
            {
                _serviceAvailabilityCallbacks[nameof(T)] = Delegate.Remove(value, listener);

                return;
            }
        }

        public static T GetService<T>() where T : IService
        {
            return (T)_gameServices[nameof(T)].GetService();
        }

        private static void NotifyUsersAboutServiceAvailability(string serviceName, ServiceAvailabilityStatus serviceAvailabilityStatus)
        {
            if (_serviceAvailabilityCallbacks.TryGetValue(serviceName, out Delegate serviceAvailabilityCallback))
            {
                serviceAvailabilityCallback?.DynamicInvoke(serviceAvailabilityStatus);
            }
        }
    }
}