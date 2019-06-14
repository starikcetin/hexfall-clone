﻿using starikcetin.hexfallClone;
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
}
