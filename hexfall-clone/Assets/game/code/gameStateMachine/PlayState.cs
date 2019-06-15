using System;
using System.Collections;
using ByTheTale.StateMachine;
using Lean.Transition;
using UnityEngine;

namespace starikcetin.hexfallClone.game.code.gameStateMachine
{
    /// <summary>
    /// Handles:
    ///     - Group selection
    ///     - Group rotation
    /// </summary>
    public class PlayState : State
    {
        private bool _isSelectionActive = false;

        private HexagonGroup _selectedGroup;
        private GameObject _highlightGameObject;

        public override void Enter()
        {
            base.Enter();

            InputManager.Instance.Swiped += InputManagerOnSwiped;
            InputManager.Instance.Tapped += InputManagerOnTapped;
        }

        public override void Exit()
        {
            base.Exit();

            InputManager.Instance.Swiped -= InputManagerOnSwiped;
            InputManager.Instance.Tapped -= InputManagerOnTapped;
        }


        private void InputManagerOnTapped(Vector3 worldPosition)
        {
            Debug.Log(
                $"{nameof(GameManager)}: {nameof(InputManagerOnTapped)}({nameof(worldPosition)}: {worldPosition})");

            var closestGroup =
                HexagonGroupDatabase.Instance.FindClosestGroup(worldPosition, GameParamsDatabase.Instance.Size);
            Debug.Log("Center of closest group: " + closestGroup.Center);

            SelectGroup(closestGroup);
        }

        private void SelectGroup(HexagonGroup group)
        {
            Debug.Log($"{nameof(GameManager)}: {nameof(SelectGroup)}({nameof(group)}: {group.Center})");
            _selectedGroup = group;
            _isSelectionActive = true;

            MonoBehaviour.Destroy(_highlightGameObject);
            _highlightGameObject =
                Utils._Debug_Highlight((Vector3) _selectedGroup.Center - new Vector3(0, 0, 1), Color.green);
        }

        private void InputManagerOnSwiped(SwipeDirection swipeDirection)
        {
            Debug.Log(
                $"{nameof(GameManager)}: {nameof(InputManagerOnSwiped)}({nameof(swipeDirection)}: {swipeDirection})");

            if (!_isSelectionActive)
            {
                Debug.Log(
                    $"{nameof(GameManager)}.{nameof(InputManagerOnSwiped)}: no active selection, ignoring swipe.");
                return;
            }

            switch (swipeDirection)
            {
                case SwipeDirection.Right:
                    StartCoroutine(RotateSequence(RotationDirection.Clockwise));
                    break;

                case SwipeDirection.Left:
                    StartCoroutine(RotateSequence(RotationDirection.CounterClockwise));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(swipeDirection), swipeDirection, null);
            }
        }

        private IEnumerator RotateSequence(RotationDirection direction)
        {
            for (int i = 0; i < 3; i++)
            {
                bool calledBack = false;
                var callback = new Action(() => calledBack = true);

                switch (direction)
                {
                    // rotate
                    case RotationDirection.Clockwise:
                        RotateOnce_Clockwise(callback);
                        break;

                    case RotationDirection.CounterClockwise:
                        RotateOnce_CounterClockwise(callback);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }

                // wait
                while (!calledBack)
                {
                    yield return null;
                }

                // check for matches
                var matchFound = CheckAndHandleMatches();

                if (matchFound)
                {
                    Debug.Log(
                        $"{nameof(GameManager)}.{nameof(RotateSequence)}: match found! breaking the rotation sequence.");
                    yield break;
                }
            }
        }

        private void RotateOnce_Clockwise(Action callback)
        {
            var alphaHex = HexagonDatabase.Instance[_selectedGroup.Alpha];
            var bravoHex = HexagonDatabase.Instance[_selectedGroup.Bravo];
            var charlieHex = HexagonDatabase.Instance[_selectedGroup.Charlie];

            // Alpha Hex --> Bravo
            Put(alphaHex, _selectedGroup.Bravo, callback);

            // Bravo Hex --> Charlie
            Put(bravoHex, _selectedGroup.Charlie, null);

            // Charlie Hex --> Alpha
            Put(charlieHex, _selectedGroup.Alpha, null);
        }

        private void RotateOnce_CounterClockwise(Action callback)
        {
            var alphaHex = HexagonDatabase.Instance[_selectedGroup.Alpha];
            var bravoHex = HexagonDatabase.Instance[_selectedGroup.Bravo];
            var charlieHex = HexagonDatabase.Instance[_selectedGroup.Charlie];

            // Alpha Hex --> Charlie
            Put(alphaHex, _selectedGroup.Charlie, callback);

            // Charlie Hex --> Bravo
            Put(charlieHex, _selectedGroup.Bravo, null);

            // Bravo Hex --> Alpha
            Put(bravoHex, _selectedGroup.Alpha, null);
        }

        private void Put(GameObject hex, OffsetCoordinates coords, Action callback)
        {
            // set in hexagon database
            HexagonDatabase.Instance[coords] = hex;

            // sync the position of the GameObject TODO: we might move this to a Hexagon class.
            //hex.transform.position = coords.ToUnity(GameParamsDatabase.Instance.Size);
            hex.transform.positionTransition(coords.ToUnity(GameParamsDatabase.Instance.Size), 0.25f)
                .JoinTransition()
                .EventTransition(callback, 0.1f);
        }

        private bool CheckAndHandleMatches()
        {
            bool matchFound = false;

            foreach (var group in HexagonGroupDatabase.Instance.HexagonGroups)
            {
                var isMatch = CheckForMatch(group);

                if (isMatch)
                {
                    HandleMatch(group);
                    matchFound = true;
                }
            }

            return matchFound;
        }

        private bool CheckForMatch(HexagonGroup group)
        {
            var (alpha, bravo, charlie) = HexagonDatabase.Instance[group];
            return IsSameColor(
                alpha.GetComponent<Hexagon>(),
                bravo.GetComponent<Hexagon>(),
                charlie.GetComponent<Hexagon>());
        }

        private bool IsSameColor(Hexagon a, Hexagon b, Hexagon c)
        {
            var ac = a.Color;
            var bc = b.Color;
            var cc = c.Color;

            return ac == bc && bc == cc && ac == cc;
        }

        private void HandleMatch(HexagonGroup group)
        {
            // explode the objects
            Explode(group.Alpha);
            Explode(group.Bravo);
            Explode(group.Charlie);

            // TODO: points
        }

        private void Explode(OffsetCoordinates coords)
        {
            var hex = HexagonDatabase.Instance[coords];
            hex.GetComponent<Hexagon>().ExplodeSelf();
            //HexagonDatabase.Instance.MarkAsDestroyed(coords);
        }
    }
}