using System;

namespace Game.ServiceLocator
{
    public sealed class ServiceDefinition
    {
        public ServiceAvailabilityStatus CurrentServiceAvailabilityStatus { get; private set; }

        public bool IsAvailable => CurrentServiceAvailabilityStatus == ServiceAvailabilityStatus.Available;

        public static event Action<string, ServiceAvailabilityStatus> UpdatedServiceAvailabilityStatus;

        private readonly string _serviceName;

        private IService _service;

        public ServiceDefinition(IService service, string serviceName, ServiceAvailabilityStatus serviceAvailabilityStatus)
        {
            _service = service;

            _serviceName = serviceName;

            CurrentServiceAvailabilityStatus = serviceAvailabilityStatus;
        }

        public void OverrideService(IService service)
        {
            _service = service;

            UpdateCurrentServiceAvailabilityStatus();

            UpdatedServiceAvailabilityStatus?.Invoke(_serviceName, CurrentServiceAvailabilityStatus);
        }

        private void UpdateCurrentServiceAvailabilityStatus()
        {
            if (_service == null)
            {
                CurrentServiceAvailabilityStatus = ServiceAvailabilityStatus.Unavailable;

                return;
            }

            CurrentServiceAvailabilityStatus = ServiceAvailabilityStatus.Available;
        }

        public IService GetService()
        {
            return _service;
        }
    }
}