using System;
using starikcetin.hexfallClone;
using UnityEngine;

public class GridShifter : MonoBehaviour
{
    public void ShiftAll(Action callback)
    {
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
                        Shift(col, row, shiftCount, hex);
                    }
                }
            }

            // TODO : refill. The amount is exactly <shiftCount>.
        }

        // TODO : wait for animations before invoking callback?
        callback?.Invoke();
    }

    private static void Shift(int col, int row, int shiftCount, GameObject hex)
    {
        Debug.Log(nameof(GridShifter) + "." + nameof(Shift) + ": shifting... ");

        // data shift can be instant, nothing will/should interfere
        HexagonDatabase.Swap(col, row, row - shiftCount);

        var newCoords = new OffsetCoordinates(col, row - shiftCount);

        // TODO : callback?
        hex.GetComponent<Hexagon>()
            .MoveAndCallback(newCoords.ToUnity(GameParamsDatabase.Instance.Size), 0.5f, null);
    }
}
