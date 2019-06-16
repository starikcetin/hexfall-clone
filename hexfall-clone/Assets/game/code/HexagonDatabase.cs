using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone;
using UnityEngine;

public class HexagonDatabase : SceneSingleton<HexagonDatabase>
{
    /// <summary>
    /// Dim0 = col
    /// Dim1 = row
    /// [col, row] => hexagon
    /// </summary>
    public GameObject[,] HexagonGrid { get; set; }

    public GameObject this[OffsetCoordinates offsetCoordinates]
    {
        get => HexagonGrid[offsetCoordinates.Col, offsetCoordinates.Row];
        set => HexagonGrid[offsetCoordinates.Col, offsetCoordinates.Row] = value;
    }

    public (GameObject alpha, GameObject bravo, GameObject charlie) this[HexagonGroup group] =>
    (
        this[group.Alpha],
        this[group.Bravo],
        this[group.Charlie]
    );

    public void MarkAsDestroyed(OffsetCoordinates coords)
    {
        this[coords] = null;
    }

    /// <summary>
    /// Swaps the [col, rowA] with [col, rowB].
    /// </summary>
    public static void Swap(int col, int rowA, int rowB)
    {
        // temp <- b
        var temp = HexagonDatabase.Instance.HexagonGrid[col, rowB];

        // b <- a
        HexagonDatabase.Instance.HexagonGrid[col, rowB] = HexagonDatabase.Instance.HexagonGrid[col, rowA];

        // a <- temp
        HexagonDatabase.Instance.HexagonGrid[col, rowA] = temp;
    }
}
