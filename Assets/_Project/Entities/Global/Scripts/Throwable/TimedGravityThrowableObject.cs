using System.Collections;

using UnityEngine;

namespace Game.Entities.Global
{
    public sealed class TimedGravityThrowableObject : ThrowableObject
    {
        [SerializeField] private float _gravityScale;

        [SerializeField] private float _delayBeforeFall;

        private WaitForSeconds _fallDelay;

        
        protected override void Awake()
        {
            base.Awake();

            _fallDelay = new WaitForSeconds(_delayBeforeFall);

            _rigidbody.gravityScale = 0.0f;
        }

        public override void ApplyMovementDirection(Vector3 direction)
        {
            base.ApplyMovementDirection(direction);

            StartCoroutine(StartFallingAfterDelay());
        }

        private IEnumerator StartFallingAfterDelay()
        {
            yield return _fallDelay;

            _rigidbody.gravityScale = _gravityScale;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _rigidbody.gravityScale = 0.0f;
        }

        private void OnValidate()
        {
            _fallDelay = new WaitForSeconds(_delayBeforeFall);
        }
    }
}