using System.Collections;
using System.Collections.Generic;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
    [RequireComponent(typeof(GridShifter))]
    public class MatchHandler : MonoBehaviour
    {
        private readonly Queue<Group> _matches = new Queue<Group>();
        private GridShifter _gridShifter;

        public bool MatchFound { get; private set; }

        private void Start()
        {
            _gridShifter = GetComponent<GridShifter>();
        }

        public IEnumerator CheckAndHandleMatches()
        {
            MatchFound = RecordAllMatches();

            if (MatchFound)
            {
                HandleAllMatches();
                yield return RequestShift();
                yield return CheckAndHandleMatches();
                MatchFound = true;
            }
        }

        private void HandleAllMatches()
        {
            HashSet<OffsetCoordinates> hexagonsToExplode = GetHexagonsToExplode();

            foreach (var hexagon in hexagonsToExplode)
            {
                Explode(hexagon);
            }
        }

        private HashSet<OffsetCoordinates> GetHexagonsToExplode()
        {
            HashSet<OffsetCoordinates> hexagonsToExplode = new HashSet<OffsetCoordinates>();

            while (_matches.Count != 0)
            {
                var group = _matches.Dequeue();

                hexagonsToExplode.Add(group.Alpha);
                hexagonsToExplode.Add(group.Bravo);
                hexagonsToExplode.Add(group.Charlie);
            }

            return hexagonsToExplode;
        }

        private bool RecordAllMatches()
        {
            bool matchFound = false;

            foreach (var group in GroupDatabase.Instance.Groups)
            {
                var isMatch = CheckForMatch(group);

                if (isMatch)
                {
                    matchFound = true;
                    RecordMatch(group);
                }
            }

            return matchFound;
        }

        private void RecordMatch(Group group)
        {
            _matches.Enqueue(group);
        }

        private IEnumerator RequestShift()
        {
            yield return _gridShifter.ShiftAndRefillAll();
        }

        private bool CheckForMatch(Group group)
        {
            var (alpha, bravo, charlie) = HexagonDatabase.Instance[group];
            return Utils.IsSameColor(
                alpha.GetComponent<Hexagon>(),
                bravo.GetComponent<Hexagon>(),
                charlie.GetComponent<Hexagon>());
        }

        private void Explode(OffsetCoordinates coords)
        {
            var hex = HexagonDatabase.Instance[coords];
            HexagonDatabase.Instance.MarkAsDestroyed(coords);
            hex.GetComponent<Hexagon>().ExplodeSelf();

            ScoreDatabase.Instance.OnHexagonExploded();
        }
    }
}
