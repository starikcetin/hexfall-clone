using System;
using System.Collections;
using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone.game.databases;
using starikcetin.hexfallClone.game.input;
using starikcetin.hexfallClone.game.visual;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
    [RequireComponent(typeof(RotationSequenceHandler))]
    public class GameManager : SceneSingleton<GameManager>
    {
        public event Action ActionSequenceCompleted;

        [SerializeField] private GroupHighlighter _groupHighlighter;

        private bool _isSelectionActive = false;
        private bool _rotationSequenceActive;
//        private GameObject _highlightGameObject;
        private InputManager _inputManager;
        private RotationSequenceHandler _rotationSequenceHandler;

        public Group SelectedGroup { get; private set; }

        private void Start()
        {
            _rotationSequenceHandler = GetComponent<RotationSequenceHandler>();
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
            if (_rotationSequenceActive)
            {
                return;
            }

            Utils.LogConditional($"{nameof(GameManager)}: {nameof(InputManagerOnTapped)}({nameof(worldPosition)}: {worldPosition})");

            var closestGroup =
                GroupDatabase.Instance.FindClosestGroup(worldPosition, GameParamsDatabase.Instance.Size);
            Utils.LogConditional("Center of closest group: " + closestGroup.Center);

            SelectGroup(closestGroup);
        }

        private void SelectGroup(Group group)
        {
            Utils.LogConditional($"{nameof(GameManager)}: {nameof(SelectGroup)}({nameof(group)}: {group.Center})");
            SelectedGroup = group;
            _isSelectionActive = true;

//            Destroy(_highlightGameObject);
//            _highlightGameObject =
//                Utils._Debug_Highlight((Vector3) SelectedGroup.Center - new Vector3(0, 0, 1), Color.green);

            _groupHighlighter.Highlight(group);
        }

        private void InputManagerOnSwiped(SwipeDirection swipeDirection, Vector2 swipeOrigin)
        {
            if (_rotationSequenceActive)
            {
                return;
            }

            Utils.LogConditional(
                $"{nameof(GameManager)}: {nameof(InputManagerOnSwiped)}({nameof(swipeDirection)}: {swipeDirection})");

            if (!_isSelectionActive)
            {
                Utils.LogConditional(
                    $"{nameof(GameManager)}.{nameof(InputManagerOnSwiped)}: no active selection, ignoring swipe.");
                return;
            }

            StartCoroutine(RotationSequence(swipeDirection, swipeOrigin));
        }

        private IEnumerator RotationSequence(SwipeDirection swipeDirection, Vector2 swipeOrigin)
        {
            _rotationSequenceActive = true;
            _groupHighlighter.gameObject.SetActive(false);

            var rotationDirection = CalculateRotationDirection(swipeDirection, SelectedGroup.Center, swipeOrigin);

            yield return _rotationSequenceHandler.RotateSequence(rotationDirection);

            ActionSequenceCompleted?.Invoke();
            _groupHighlighter.gameObject.SetActive(true);
            _rotationSequenceActive = false;
        }

        private RotationDirection CalculateRotationDirection(SwipeDirection swipeDirection, Vector2 selectionPosition,
            Vector2 swipeOrigin)
        {
            var swipeOriginIsAboveSelection = swipeOrigin.y > selectionPosition.y;

            switch (swipeDirection)
            {
                case SwipeDirection.Left:
                    return swipeOriginIsAboveSelection
                        ? RotationDirection.CounterClockwise
                        : RotationDirection.Clockwise;

                case SwipeDirection.Right:
                    return swipeOriginIsAboveSelection
                        ? RotationDirection.Clockwise
                        : RotationDirection.CounterClockwise;

                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
        }
    }
}
