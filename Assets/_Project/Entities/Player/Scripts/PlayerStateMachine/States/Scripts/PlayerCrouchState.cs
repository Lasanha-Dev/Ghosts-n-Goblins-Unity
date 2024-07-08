using Game.Entities;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerCrouchState", menuName = "StateMachine/Player/States/PlayerCrouchState")]
    public sealed class PlayerCrouchState : StateBase
    {
        private readonly int PlayerCrouchAnimationState = Animator.StringToHash("PlayerCrouchedIdleAnimation");

        private readonly int PLAYER_CROUCH_PARAM_BOOL_ID = Animator.StringToHash("IsCrouched");

        private Animator _playerAnimator;

        private Rigidbody2D _playerRigidbody;

        private BoxCollider2D _playerCollider;

        private PlayerCrouchParameters _playerCrouchParameters;

        private float _defaultColliderYOffset;

        private float _defaultColliderYSize;

        public override void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();

            _playerCollider = entityComponentsReferences.GetEntityComponent<BoxCollider2D>();

            _playerRigidbody = entityComponentsReferences.GetEntityComponent<Rigidbody2D>();

            _playerCrouchParameters = stateMachineStatesParameters.GetParameterObject<PlayerCrouchParameters>();
        }

        public override void OnEnter()
        {
            _defaultColliderYOffset = _playerCollider.offset.y;

            _defaultColliderYSize = _playerCollider.size.y;

            _playerCollider.offset = new Vector2(_playerCollider.offset.x, _playerCrouchParameters.ColliderYOffset);

            _playerCollider.size = new Vector2(_playerCollider.size.x, _playerCrouchParameters.ColliderYSize);

            _playerRigidbody.velocity = Vector2.zero;

            _playerAnimator.Play(PlayerCrouchAnimationState);

            _playerAnimator.SetBool(PLAYER_CROUCH_PARAM_BOOL_ID, true);
        }

        public override void OnExit()
        {
            _playerCollider.offset = new Vector2(_playerCollider.offset.x, _defaultColliderYOffset);

            _playerCollider.size = new Vector2(_playerCollider.size.x, _defaultColliderYSize);
        }

        public override object GetStateParameterObject()
        {
            return new PlayerCrouchParameters();
        }

        [System.Serializable]
        public sealed class PlayerCrouchParameters
        {
            [HideInInspector]
            public string name;

            [field: SerializeField] public float ColliderYOffset { get; private set; }

            [field: SerializeField] public float ColliderYSize { get; private set; }

            public PlayerCrouchParameters()
            {
                name = nameof(PlayerCrouchParameters);
            }
        }
    }
}
