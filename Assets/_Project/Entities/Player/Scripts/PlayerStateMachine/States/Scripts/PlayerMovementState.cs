using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using Game.Entities.Player;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementState", menuName = "StateMachine/Player/States/PlayerMovementState")]
    public sealed class PlayerMovementState : StateBase
    {
        private Rigidbody2D _playerRigidbody;

        private InputDefinition<float> _movementInput;

        private Animator _playerAnimator;

        private PlayerMovementParameters _playerMovementParameters;

        private readonly int PLAYER_RUNNING_ANIMATION_STATE = Animator.StringToHash("PlayerRunning");

        public override void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerRigidbody = entityComponentsReferences.GetEntityComponent<Rigidbody2D>();

            _movementInput = PlayerInputsController.MovementInput;

            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();

            _playerMovementParameters = stateMachineStatesParameters.GetParameterObject<PlayerMovementParameters>();
        }

        public override void OnEnter()
        {
            _playerAnimator.Play(PLAYER_RUNNING_ANIMATION_STATE);
        }

        public override void OnFixedUpdate()
        {
            _playerRigidbody.velocity = new Vector2(_movementInput.InputValue * _playerMovementParameters.MovementSpeed * Time.fixedDeltaTime, _playerRigidbody.velocity.y);
        }

        public override object GetStateParameterObject()
        {
            return new PlayerMovementParameters();
        }

        [System.Serializable]
        public sealed class PlayerMovementParameters
        {
            [HideInInspector]
            public string name;

            [field: SerializeField] public float MovementSpeed { get; private set; }

            public PlayerMovementParameters()
            {
                name = nameof(PlayerMovementParameters);
            }
        }
    }
}