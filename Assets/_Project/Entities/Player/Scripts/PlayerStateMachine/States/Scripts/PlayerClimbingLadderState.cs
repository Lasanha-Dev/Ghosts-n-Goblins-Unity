using PlayerInputsController = Game.Entities.Player.PlayerInputsController;

using EntityComponentsReferences = Game.Entities.EntityComponentsReferences;

using PlayerRotationController = Game.Entities.Player.PlayerRotationController;

using Type = System.Type;

using PlayerStartClimbingLadder = Game.EventSystem.Event.PlayerStartClimbingLadder;

using PlayerFinishedClimbingLadder = Game.EventSystem.Event.PlayerFinishedClimbingLadder;

using Game.EventSystem;

using UnityEngine;
using Game.Entities;
using Game.Entities.Player;

namespace Game.StateMachine.Player
{
    [CreateAssetMenu(fileName = "PlayerClimbingLadderState", menuName = "StateMachine/Player/States/PlayerClimbingLadderState")]
    public sealed class PlayerClimbingLadderState : StateBase, IEventInvoker
    {
        [SerializeField] private LayerMask _whatLayerIsLadder;

        [SerializeField] private LayerMask _whatLayerIsGround;

        [SerializeField] private float _climbAnimationSpeed;

        [SerializeField] private float _climbSpeed;

        [SerializeField] private float _animationAdjustmentStepAmount;

        private Animator _playerAnimator;

        private BoxCollider2D _playerCollider;

        private Rigidbody2D _playerRigidbody;

        private PlayerRotationController _playerRotationController;

        private LadderController _currentLadder;

        private InputDefinition<float> _ladderInput;

        private float _defaultGravityScale;

        private float _defaultClimbAnimationTimeProgress;

        private float _finalClimbTopExitAnimationProgress01;

        private float _finalClimbTopExitAnimationProgress02;

        private const int CLIMB_LADDER_ANIMATION_LAYER = 0;

        private const float DEFAULT_ANIMATION_DESIRED_LENGTH = 1f;

        private const float INVERSE_ANIMATION_DESIRED_LENGTH = 0f;

        private const int INVERSE_ANIMATION_STEP_MULTIPLIER = -1;

        private readonly int PlayerClimbLadderAnimationState = Animator.StringToHash("PlayerClimbLadder");

        private readonly int PlayerClimbLadderFinal01AnimationState = Animator.StringToHash("PlayerFinalClimb01");

        private readonly int PlayerClimbLadderFinal02AnimationState = Animator.StringToHash("PlayerFinalClimb02");

        private readonly int PlayerIdleAnimationState = Animator.StringToHash("PlayerStandingIdle");

        public override void SetupState(StateMachineStatesParameters stateMachineStatesParameters, EntityComponentsReferences entityComponentsReferences)
        {
            _playerAnimator = entityComponentsReferences.GetEntityComponent<Animator>();

            _playerCollider = entityComponentsReferences.GetEntityComponent<BoxCollider2D>();

            _playerRigidbody = entityComponentsReferences.GetEntityComponent<Rigidbody2D>();

            _ladderInput = PlayerInputsController.LadderInput;

            _playerRotationController = entityComponentsReferences.GetEntityComponent<PlayerRotationController>();

            _defaultGravityScale = _playerRigidbody.gravityScale;
        }

        public override void OnEnter()
        {
            _playerRotationController.enabled = false;

            _currentLadder = Physics2D.OverlapBox(_playerCollider.bounds.center, _playerCollider.size, 0, _whatLayerIsLadder).GetComponent<LadderController>();

            AdjustPlayerRigidbody();

            ResetAnimationValues();

            AdjustPlayerOnLadder();

            InvokeEvent(typeof(PlayerStartClimbingLadder));
        }

        public override void OnUpdate()
        {
            UpdateClimbingAnimation();

            if(PlayerHasReachedTopPoint() && _finalClimbTopExitAnimationProgress02 >= DEFAULT_ANIMATION_DESIRED_LENGTH)
            {
                FinishClimbing();
            }
        }

        public override void OnFixedUpdate()
        {
            if(IsAllowedToMove())
            {
                MovePlayerOnLadder();
            }
        }

        private void UpdateClimbingAnimation()
        {
            if(_ladderInput.IsPressed is false)
            {
                return;
            }

            if(PlayerHasReachedTopPoint() is false && IsAllowedToMove())
            {
                HandleDefaultClimbAnimation();

                return;
            }

            if(_ladderInput.InputValue > 0)
            {
                HandleTopClimbAnimation();

                return;
            }

            HandleInverseTopClimbAnimation();
        }

        private bool PlayerHasReachedTopPoint()
        {
            return _playerRigidbody.position.y >= _currentLadder.StartTopPosition.y;
        }

        private bool IsAllowedToMove()
        {
            int currentAnimationShortNameHash = _playerAnimator.GetCurrentAnimatorStateInfo(CLIMB_LADDER_ANIMATION_LAYER).shortNameHash;

            return currentAnimationShortNameHash != PlayerClimbLadderFinal02AnimationState && _finalClimbTopExitAnimationProgress01 <= INVERSE_ANIMATION_DESIRED_LENGTH;
        }

        private void HandleDefaultClimbAnimation()
        {
            if(IsNextAnimationStepProgressGreaterThanDesiredLength(ref _defaultClimbAnimationTimeProgress, DEFAULT_ANIMATION_DESIRED_LENGTH))
            {
                _defaultClimbAnimationTimeProgress = 0f;
            }

            PlayAnimation(PlayerClimbLadderAnimationState, _defaultClimbAnimationTimeProgress);
        }

        private void HandleTopClimbAnimation()
        {
            if(IsNextAnimationStepProgressGreaterThanDesiredLength(ref _finalClimbTopExitAnimationProgress01, DEFAULT_ANIMATION_DESIRED_LENGTH) is false)
            {
                PlayAnimation(PlayerClimbLadderFinal01AnimationState, _finalClimbTopExitAnimationProgress01);

                return;
            }

            if(IsNextAnimationStepProgressGreaterThanDesiredLength(ref _finalClimbTopExitAnimationProgress02, DEFAULT_ANIMATION_DESIRED_LENGTH) is false)
            {
                PlayAnimation(PlayerClimbLadderFinal02AnimationState, _finalClimbTopExitAnimationProgress02);
            }
        }   

        private void HandleInverseTopClimbAnimation()
        {
            if (IsNextAnimationInverseStepProgressLessThanDesiredLength(ref _finalClimbTopExitAnimationProgress02, INVERSE_ANIMATION_DESIRED_LENGTH) is false)
            {
                PlayAnimation(PlayerClimbLadderFinal02AnimationState, _finalClimbTopExitAnimationProgress01);

                return;
            }

            if (IsNextAnimationInverseStepProgressLessThanDesiredLength(ref _finalClimbTopExitAnimationProgress01, INVERSE_ANIMATION_DESIRED_LENGTH) is false)
            {
                PlayAnimation(PlayerClimbLadderFinal01AnimationState, _finalClimbTopExitAnimationProgress02);
            }
        }

        private void FinishClimbing()
        {
            PlayAnimation(PlayerIdleAnimationState);

            _playerRigidbody.transform.position = _currentLadder.LadderExitTopPosition;

            InvokeEvent(typeof(PlayerFinishedClimbingLadder));
        }

        private void MovePlayerOnLadder()
        {
            if (_ladderInput.InputValue > 0f && PlayerHasReachedTopPoint() is false)
            {
                _playerRigidbody.position = Vector3.MoveTowards(_playerRigidbody.position, _playerRigidbody.position + Vector2.up, _climbSpeed * Time.fixedDeltaTime);

                return;
            }

            if (_ladderInput.InputValue < 0f)
            {
                _playerRigidbody.position = Vector3.MoveTowards(_playerRigidbody.position, _playerRigidbody.position + Vector2.down, _climbSpeed * Time.fixedDeltaTime);
            }
        }

        private bool IsNextAnimationStepProgressGreaterThanDesiredLength(ref float currentAnimationTimeProgress, float desiredProgressLength)
        {
            currentAnimationTimeProgress = AdjustAndClampAnimationStep(currentAnimationTimeProgress, _animationAdjustmentStepAmount);

            return currentAnimationTimeProgress >= desiredProgressLength;
        }

        private bool IsNextAnimationInverseStepProgressLessThanDesiredLength(ref float currentAnimationTimeProgress, float desiredProgressLength)
        {
            currentAnimationTimeProgress = AdjustAndClampAnimationStep(currentAnimationTimeProgress, _animationAdjustmentStepAmount, INVERSE_ANIMATION_STEP_MULTIPLIER);

            return currentAnimationTimeProgress <= desiredProgressLength;
        }

        private float AdjustAndClampAnimationStep(float param, float stepAmount, int multiplier = 1)
        {
            param += stepAmount * Time.deltaTime * _climbAnimationSpeed * multiplier;

            param = Mathf.Clamp(param, 0f, 1f);

            return param;
        }

        private void AdjustPlayerRigidbody()
        {
            _defaultGravityScale = _playerRigidbody.gravityScale;

            _playerRigidbody.gravityScale = 0f;

            _playerRigidbody.velocity = Vector2.zero;
        }

        private void ResetAnimationValues()
        {
            _defaultClimbAnimationTimeProgress = 0;

            _finalClimbTopExitAnimationProgress01 = 0;

            _finalClimbTopExitAnimationProgress02 = 0;
        }

        private void AdjustPlayerOnLadder()
        {
            if(PlayerIsAtTheBottomOfTheLadder())
            {
                PlayAnimation(PlayerClimbLadderAnimationState, _defaultClimbAnimationTimeProgress);

                _playerRigidbody.position = _currentLadder.StartBottomPosition;

                return;
            }

            _finalClimbTopExitAnimationProgress01 = 1;

            _finalClimbTopExitAnimationProgress02 = 1;

            PlayAnimation(PlayerClimbLadderFinal02AnimationState);

            _playerRigidbody.position = _currentLadder.StartTopPosition;
        }

        private bool PlayerIsAtTheBottomOfTheLadder()
        {
            return _playerRigidbody.position.y <= _currentLadder.transform.position.y;
        }

        private void PlayAnimation(int animationStateHash, float normalizedTime = 0f)
        {
            _playerAnimator.Play(animationStateHash, CLIMB_LADDER_ANIMATION_LAYER, normalizedTime);
        }

        public override void OnExit()
        {
            _playerRigidbody.gravityScale = _defaultGravityScale;

            _playerRotationController.enabled = true;
        }

        public void InvokeEvent(Type eventType, IEventParameter eventArg = null)
        {
            EventBus.Invoke(eventType, this, eventArg);
        }
    }
}