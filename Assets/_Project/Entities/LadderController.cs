using EventBus = Game.EventSystem.EventBus;

using IEventListener = Game.EventSystem.IEventListener;

using PlayerStartClimbingLadder = Game.EventSystem.Event.PlayerStartClimbingLadder;

using PlayerFinishedClimbingLadder = Game.EventSystem.Event.PlayerFinishedClimbingLadder;

using UnityEngine;

namespace Game.Entities
{
    [RequireComponent(typeof(EdgeCollider2D))]

    public sealed class LadderController : MonoBehaviour, IEventListener
    {
        [field: SerializeField] public GameObject LadderGroundGameObject { get; private set; }

        public Vector2 LadderExitTopPosition { get; private set; }

        public Vector3 StartBottomPosition { get; private set; }

        public Vector3 StartTopPosition { get; private set; }

        private LadderState _currentLadderState = LadderState.NotBeingClimbed;

        private const string PLAYER_TAG = "Player";

        private const float LADDER_EXIT_OFFSET = 0.93f;

        private const int LADDER_BOTTOM_TRIGGER_INDEX = 0;

        private const int LADDER_TOP_TRIGGER_INDEX = 1;

        private void Awake()
        {
            SetupLadderStartPositions();

            LadderExitTopPosition = LadderGroundGameObject.transform.position + LadderGroundGameObject.transform.up * LADDER_EXIT_OFFSET;
        }

        private void SetupLadderStartPositions()
        {
            EdgeCollider2D ladderEdgeCollider = GetComponent<EdgeCollider2D>();

            StartBottomPosition = transform.TransformPoint(ladderEdgeCollider.points[LADDER_BOTTOM_TRIGGER_INDEX]);

            StartTopPosition = transform.TransformPoint(ladderEdgeCollider.points[LADDER_TOP_TRIGGER_INDEX]);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER_TAG) == false)
            {
                return;
            }

            if (collision.transform.position.y >= LadderGroundGameObject.transform.position.y && _currentLadderState == LadderState.NotBeingClimbed)
            {
                LadderGroundGameObject.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER_TAG))
            {
                SubscribeToEvents();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER_TAG))
            {
                UnsubscribeFromEvents();
            }
        }

        private void OnPlayerStartClimbingLadder()
        {
            LadderGroundGameObject.SetActive(false);

            _currentLadderState = LadderState.BeingClimbed;
        }

        private void OnPlayerFinishedClimbingLadder()
        {
            LadderGroundGameObject.SetActive(true);

            _currentLadderState = LadderState.NotBeingClimbed;
        }

        public void SubscribeToEvents()
        {
            EventBus.Subscribe(OnPlayerStartClimbingLadder, typeof(PlayerStartClimbingLadder));

            EventBus.Subscribe(OnPlayerFinishedClimbingLadder, typeof(PlayerFinishedClimbingLadder));
        }

        public void UnsubscribeFromEvents()
        {
            EventBus.Unsubscribe(OnPlayerStartClimbingLadder, typeof(PlayerStartClimbingLadder));

            EventBus.Unsubscribe(OnPlayerFinishedClimbingLadder, typeof(PlayerFinishedClimbingLadder));
        }

        private enum LadderState
        {
            BeingClimbed,
            NotBeingClimbed
        }
    }
}