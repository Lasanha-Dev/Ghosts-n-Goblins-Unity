using Array = System.Array;

using PlayerComponentsReferences =  Game.Entities.Player.PlayerComponentsReferences;

using UnityEngine;
using Game.Entities;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerIsGround", menuName = "StateMachine/Player/Transitions/PlayerIsGround")]
    public sealed class PlayerIsGround : TransitionConditionBase
    {
        [SerializeField] private LayerMask _whatLayerIsGround;

        [SerializeField] private float _groundCheckDistance;
        
        private BoxCollider2D _playerCollider;

        private readonly RaycastHit2D[] _raycastHit2D = new RaycastHit2D[1];

        public override bool CheckTransition()
        {
            Array.Clear(_raycastHit2D, 0, _raycastHit2D.Length);

            Physics2D.BoxCastNonAlloc(_playerCollider.bounds.center, _playerCollider.size, 0, Vector2.down, _raycastHit2D, _groundCheckDistance, _whatLayerIsGround);

            return _raycastHit2D[0] != default;
        }

        protected override void SetupCondition(EntitieComponentsReferences playerComponentsReferences)
        {
            _playerCollider = playerComponentsReferences.GetEntitieComponent<BoxCollider2D>();
        }
    }
}
