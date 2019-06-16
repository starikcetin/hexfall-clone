using System;
using System.Collections;
using starikcetin.hexfallClone;
using UnityEngine;

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

    public IEnumerator ShiftAll()
    {
        bool _everyThingIsDone = false;
        var callbackAggregator = new CallbackAggregator((() => _everyThingIsDone = true));

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
                        callbackAggregator.JobStarted();
                        Shift(col, row, shiftCount, hex, callbackAggregator.JobFinished);
                    }
                }
            }

            var rowLength = HexagonDatabase.Instance.HexagonGrid.GetLength(1);
            var refillSpawnRow = rowLength + 2;

            // refill. The amount is exactly <shiftCount>.
            for (var i = rowLength - shiftCount; i < rowLength; i++)
            {
                var fillTarget = new OffsetCoordinates(col, i);

                var hex = HexagonGridBuilder.Instance.CreateHexagon(GameParamsDatabase.Instance.Size,
                    new OffsetCoordinates(col, refillSpawnRow), _shouldSpawnBomb);

                if (_shouldSpawnBomb)
                {
                    _shouldSpawnBomb = false;
                }

                // data shift can be instant, nothing will/should interfere
                HexagonDatabase.Instance[fillTarget] = hex;

                callbackAggregator.JobStarted();
                hex.GetComponent<Hexagon>()
                    .MoveAndCallback(fillTarget.ToUnity(GameParamsDatabase.Instance.Size), 0.5f,
                        callbackAggregator.JobFinished);
            }
        }

        callbackAggregator.PermitCallback();

        yield return new WaitUntil(() => _everyThingIsDone);
    }

    private static void Shift(int col, int row, int shiftCount, GameObject hex, Action callback)
    {
        Debug.Log($"{nameof(GridShifter)}.{nameof(Shift)}: shifting... " +
                  $"pos: [{col}, {row}] shiftCount: {shiftCount} {nameof(hex)}: {hex}");

        // data shift can be instant, nothing will/should interfere
        HexagonDatabase.Swap(col, row, row - shiftCount);

        var newCoords = new OffsetCoordinates(col, row - shiftCount);

        hex.GetComponent<Hexagon>()
            .MoveAndCallback(newCoords.ToUnity(GameParamsDatabase.Instance.Size), 0.5f, callback);
    }
}
