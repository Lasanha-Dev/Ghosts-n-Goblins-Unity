using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using Game.Entities.Player;

using System;

using UnityEngine;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "CanPlayerClimbLadder", menuName = "StateMachine/Player/Transitions/CanPlayerClimbLadder")]
    public sealed class CanPlayerClimbLadder : TransitionConditionBase
    {
        [SerializeReference] private LayerMask _whatLayerIsLadder;

        private BoxCollider2D _playerBoxCollider;

        private readonly Collider2D[] _overlapHitsResults = new Collider2D[1];

        private InputDefinition<float> _ladderInput;

        public override void SetupCondition(StateMachineTransitionsParameters stateMachineTransitionsParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerBoxCollider = entityComponentsReferences.GetEntityComponent<BoxCollider2D>();

            _ladderInput = PlayerInputsController.LadderInput;
        }

        public override bool CanTransit()
        {
            if (_ladderInput.WasPressedThisFrame is false)
            {
                return false;
            }

            Array.Clear(_overlapHitsResults, 0, _overlapHitsResults.Length);

            Physics2D.OverlapBoxNonAlloc(_playerBoxCollider.bounds.center, _playerBoxCollider.size, 0f, _overlapHitsResults, _whatLayerIsLadder);

            if (_overlapHitsResults[0] == null)
            {
                return false;
            }

            if (_ladderInput.InputValue > 0.5f && _playerBoxCollider.transform.position.y >= _overlapHitsResults[0].transform.position.y)
            {
                return false;
            }

            if (_ladderInput.InputValue < -0.5f && _playerBoxCollider.transform.position.y <= _overlapHitsResults[0].transform.position.y)
            {
                return false;
            }

            return true;
        }
    }
}