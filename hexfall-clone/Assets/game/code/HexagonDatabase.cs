using starikcetin.hexfallClone;
using UnityEngine;

public class HexagonDatabase : Singleton<HexagonDatabase>
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

//    public void MarkAsDestroyed(OffsetCoordinates coords)
//    {
//        this[coords] = null;
//    }
}
