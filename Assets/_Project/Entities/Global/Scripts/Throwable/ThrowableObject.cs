using UnityEngine;

namespace Game.Entities.Global
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ThrowableObject : MonoBehaviour
    {
        [SerializeField] private float _throwVelocity;

        protected Rigidbody2D _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public virtual void ApplyMovementDirection(Vector3 direction)
        {
            _rigidbody.velocity = _throwVelocity * Time.fixedDeltaTime * direction;
        }

        protected virtual void OnDisable()
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
