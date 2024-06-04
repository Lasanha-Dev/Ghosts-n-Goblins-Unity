using Array = System.Array;

using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsGround", menuName = "StateMachine/Player/Transitions/PlayerIsGround")]
    public sealed class PlayerIsGround : TransitionConditionBase
    {
        private PlayerIsGroundConditionParameters _playerIsGroundConditionParameters;

        private BoxCollider2D _playerCollider;

        private readonly RaycastHit2D[] _raycastHit2D = new RaycastHit2D[1];

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerIsGroundConditionParameters = stateMachineTransitionsParameters.GetParameterObject<PlayerIsGroundConditionParameters>();

            _playerCollider = entityComponentsReferences.GetEntityComponent<BoxCollider2D>();
        }

        public override bool CanTransit()
        {
            Array.Clear(_raycastHit2D, 0, _raycastHit2D.Length);

            Physics2D.BoxCastNonAlloc(_playerCollider.bounds.center, _playerCollider.size, 0, Vector2.down, _raycastHit2D, _playerIsGroundConditionParameters.GroundCheckDistance, _playerIsGroundConditionParameters.GroundLayer);

            return _raycastHit2D[0] != default;
        }

        public override object GetTransitionParameterObject()
        {
            return new PlayerIsGroundConditionParameters();
        }

        public sealed class PlayerIsGroundConditionParameters
        {
            [HideInInspector]
            public string name;

            [field: SerializeField] public LayerMask GroundLayer { get; private set; }

            [field: SerializeField] public float GroundCheckDistance { get; private set; }

            public PlayerIsGroundConditionParameters()
            {
                name = nameof(PlayerIsGroundConditionParameters);
            }
        }
    }
}