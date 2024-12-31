using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
    public sealed class DisableObjectOnDistanceTravelled : MonoBehaviour
    {
        [SerializeField] private Transform _objectTransform;

        [SerializeField] private float _maxDistance;

        private Vector3 _startDistance;

        private void OnEnable()
        {
            _startDistance = _objectTransform.position;
        }

        private void Update()
        {
            float squaredDistance = (_startDistance - _objectTransform.position).sqrMagnitude;

            if(squaredDistance >= _maxDistance * _maxDistance)
            {
                _objectTransform.gameObject.SetActive(false);
            }
        }
    }
}
