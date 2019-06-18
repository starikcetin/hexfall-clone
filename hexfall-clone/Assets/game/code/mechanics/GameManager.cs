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

        public Group SelectedGroup { get; private set; }
//        private GameObject _highlightGameObject;

        private InputManager _inputManager;
        private RotationSequenceHandler _rotationSequenceHandler;
        private bool _rotationSequenceActive;

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

        private void InputManagerOnSwiped(SwipeDirection swipeDirection)
        {
            if (_rotationSequenceActive)
            {
                return;
            }

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
            _rotationSequenceActive = true;
            _groupHighlighter.gameObject.SetActive(false);

            switch (swipeDirection)
            {
                case SwipeDirection.Right:
                    yield return _rotationSequenceHandler.RotateSequence(RotationDirection.Clockwise);
                    break;

                case SwipeDirection.Left:
                    yield return _rotationSequenceHandler.RotateSequence(RotationDirection.CounterClockwise);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }

            ActionSequenceCompleted?.Invoke();
            _groupHighlighter.gameObject.SetActive(true);
            _rotationSequenceActive = false;
        }
    }
}
