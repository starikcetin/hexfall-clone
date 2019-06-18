using System;
using System.Collections;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
    public class GridShifter : MonoBehaviour
    {
        private bool _shouldSpawnBomb;

        private void Start()
        {
            ScoreDatabase.Instance.BombScoreReached += OnBombScoreReached;
        }

        private void OnDestroy()
        {
            if (ScoreDatabase.Instance)
            {
                ScoreDatabase.Instance.BombScoreReached -= OnBombScoreReached;
            }
        }

        private void OnBombScoreReached()
        {
            _shouldSpawnBomb = true;
        }

        public IEnumerator ShiftAndRefillAll()
        {
            var jobCounter = new JobCounter(true);

            for (int col = 0; col < HexagonDatabase.Instance.HexagonGrid.GetLength(0); col++)
            {
                // count empty cells
                int shiftCount = 0;

                for (int row = 0; row < HexagonDatabase.Instance.HexagonGrid.GetLength(1); row++)
                {
                    var hex = HexagonDatabase.Instance.HexagonGrid[col, row];

                    if (!hex)
                    {
                        // empty cell: count
                        shiftCount++;
                    }
                    else
                    {
                        if (shiftCount > 0)
                        {
                            // full cell: shift
                            jobCounter.JobStarted();
                            StartCoroutine(Shift(col, row, shiftCount, hex, jobCounter.JobFinished));
                        }
                    }
                }

                var rowLength = HexagonDatabase.Instance.HexagonGrid.GetLength(1);
                var refillSpawnRow = rowLength + 2;

                // refill. The amount is exactly <shiftCount>.
                for (var row = rowLength - shiftCount; row < rowLength; row++)
                {
                    jobCounter.JobStarted();
                    StartCoroutine(Refill(col, row, refillSpawnRow, jobCounter.JobFinished));
                }
            }

            jobCounter.PermitCompletion();

            yield return new WaitUntil(() => jobCounter.IsCompleted);
        }

        private IEnumerator Refill(int col, int row, int refillSpawnRow, Action callback)
        {
            var fillTarget = new OffsetCoordinates(col, row);

            var hex = HexagonCreator.Instance.CreateHexagon(GameParamsDatabase.Instance.Size,
                new OffsetCoordinates(col, refillSpawnRow), _shouldSpawnBomb);

            if (_shouldSpawnBomb)
            {
                _shouldSpawnBomb = false;
            }

            // data shift can be instant, nothing will/should interfere
            HexagonDatabase.Instance[fillTarget] = hex;

            yield return
                hex.GetComponent<Hexagon>().MoveTo(fillTarget.ToUnity(GameParamsDatabase.Instance.Size), 0.5f);

            callback();
        }

        private static IEnumerator Shift(int col, int row, int shiftCount, GameObject hex, Action callback)
        {
            Utils.LogConditional($"{nameof(GridShifter)}.{nameof(Shift)}: shifting... " +
                                 $"pos: [{col}, {row}] shiftCount: {shiftCount} {nameof(hex)}: {hex}");

            // data shift can be instant, nothing will/should interfere
            HexagonDatabase.Swap(col, row, row - shiftCount);

            var newCoords = new OffsetCoordinates(col, row - shiftCount);

            yield return
                hex.GetComponent<Hexagon>().MoveTo(newCoords.ToUnity(GameParamsDatabase.Instance.Size), 0.5f);

            callback();
        }
    }
}
