using System;
using System.Collections;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
    [RequireComponent(typeof(MatchHandler))]
    public class RotationSequenceHandler : MonoBehaviour
    {
        private MatchHandler _matchHandler;

        private void Start()
        {
            _matchHandler = GetComponent<MatchHandler>();
        }

        public IEnumerator RotateSequence(RotationDirection direction)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (direction)
                {
                    // rotate
                    case RotationDirection.Clockwise:
                        yield return RotateOnce_Clockwise();
                        break;

                    case RotationDirection.CounterClockwise:
                        yield return RotateOnce_CounterClockwise();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }

                // check for matches
                yield return _matchHandler.CheckAndHandleMatches();

                if (GetComponent<MatchHandler>().MatchFound)
                {
                    Utils.LogConditional(
                        $"{nameof(GameManager.Instance)}.{nameof(RotateSequence)}: match found! breaking the rotation sequence.");

                    yield break;
                }
            }
        }

        private IEnumerator RotateOnce_Clockwise()
        {
            var alphaHex = HexagonDatabase.Instance[GameManager.Instance.SelectedGroup.Alpha];
            var bravoHex = HexagonDatabase.Instance[GameManager.Instance.SelectedGroup.Bravo];
            var charlieHex = HexagonDatabase.Instance[GameManager.Instance.SelectedGroup.Charlie];

            // Alpha Hex --> Bravo
            StartCoroutine(Put(alphaHex, GameManager.Instance.SelectedGroup.Bravo));

            // Bravo Hex --> Charlie
            StartCoroutine(Put(bravoHex, GameManager.Instance.SelectedGroup.Charlie));

            // Charlie Hex --> Alpha
            yield return Put(charlieHex, GameManager.Instance.SelectedGroup.Alpha);

            // we only yield on one of them since they need to happen in parallel.
        }

        private IEnumerator RotateOnce_CounterClockwise()
        {
            var alphaHex = HexagonDatabase.Instance[GameManager.Instance.SelectedGroup.Alpha];
            var bravoHex = HexagonDatabase.Instance[GameManager.Instance.SelectedGroup.Bravo];
            var charlieHex = HexagonDatabase.Instance[GameManager.Instance.SelectedGroup.Charlie];

            // Alpha Hex --> Charlie
            StartCoroutine(Put(alphaHex, GameManager.Instance.SelectedGroup.Charlie));

            // Charlie Hex --> Bravo
            StartCoroutine(Put(charlieHex, GameManager.Instance.SelectedGroup.Bravo));

            // Bravo Hex --> Alpha
            yield return Put(bravoHex, GameManager.Instance.SelectedGroup.Alpha);

            // we only yield on one of them since they need to happen in parallel.
        }

        private IEnumerator Put(GameObject hex, OffsetCoordinates coords)
        {
            // set in hexagon database
            HexagonDatabase.Instance[coords] = hex;

            // sync the position of the GameObject
            yield return
                hex.GetComponent<Hexagon>().MoveTo(coords.ToUnity(), 0.25f);
        }
    }
}
