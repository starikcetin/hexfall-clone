using System;
using System.Collections;
using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    public class GameManager : SceneSingleton<GameManager>
    {
        public event Action ActionSequenceCompleted;

        [SerializeField] private GroupHighlighter _groupHighlighter;

        private bool _isSelectionActive = false;

        public HexagonGroup SelectedGroup { get; private set; }
        private GameObject _highlightGameObject;

        private InputManager _inputManager;


        private void Start()
        {
            _inputManager = GetComponentInChildren<InputManager>();

            _inputManager.Swiped += InputManagerOnSwiped;
            _inputManager.Tapped += InputManagerOnTapped;

            ScoreDatabase.Instance.ResetScore();
        }

        protected void OnDestroy()
        {
            _inputManager.Swiped -= InputManagerOnSwiped;
            _inputManager.Tapped -= InputManagerOnTapped;
        }

        private void InputManagerOnTapped(Vector3 worldPosition)
        {
            Utils.LogConditional($"{nameof(GameManager)}: {nameof(InputManagerOnTapped)}({nameof(worldPosition)}: {worldPosition})");

            var closestGroup =
                HexagonGroupDatabase.Instance.FindClosestGroup(worldPosition, GameParamsDatabase.Instance.Size);
            Utils.LogConditional("Center of closest group: " + closestGroup.Center);

            SelectGroup(closestGroup);
        }

        private void SelectGroup(HexagonGroup group)
        {
            Utils.LogConditional($"{nameof(GameManager)}: {nameof(SelectGroup)}({nameof(group)}: {group.Center})");
            SelectedGroup = group;
            _isSelectionActive = true;

            Destroy(_highlightGameObject);
            _highlightGameObject =
                Utils._Debug_Highlight((Vector3) SelectedGroup.Center - new Vector3(0, 0, 1), Color.green);

            _groupHighlighter.Highlight(group);
        }

        private void InputManagerOnSwiped(SwipeDirection swipeDirection)
        {
            Utils.LogConditional($"{nameof(GameManager)}: {nameof(InputManagerOnSwiped)}({nameof(swipeDirection)}: {swipeDirection})");

            if (!_isSelectionActive)
            {
                Utils.LogConditional($"{nameof(GameManager)}.{nameof(InputManagerOnSwiped)}: no active selection, ignoring swipe.");
                return;
            }

            StartCoroutine(RotationSequence(swipeDirection));
        }

        private IEnumerator RotationSequence(SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.Right:
                    yield return GetComponent<RotationSequenceHandler>().RotateSequence(RotationDirection.Clockwise);
                    break;

                case SwipeDirection.Left:
                    yield return GetComponent<RotationSequenceHandler>().RotateSequence(RotationDirection.CounterClockwise);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }

            ActionSequenceCompleted?.Invoke();
        }
    }
}
