using UnityEngine;

namespace Game.Entities.Player
{
    public sealed class PlayerComponentsReferences : MonoBehaviour
    {
        public Rigidbody2D PlayerRigidbody { get; private set; }

        public PlayerInputsController PlayerInputsController { get; private set; }

        public Animator PlayerAnimator { get; private set; }

        public BoxCollider2D PlayerCollider { get; private set; }

        private void Awake()
        {
            PlayerRigidbody = GetComponent<Rigidbody2D>();

            PlayerInputsController = GetComponent<PlayerInputsController>();

            PlayerAnimator = GetComponentInChildren<Animator>();

            PlayerCollider = GetComponent<BoxCollider2D>();
        }
    }
}