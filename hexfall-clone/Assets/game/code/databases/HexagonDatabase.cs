using System;
using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone.game.mechanics;
using UnityEngine;

namespace starikcetin.hexfallClone.game.databases
{
    public class HexagonDatabase : SceneSingleton<HexagonDatabase>
    {
        /// <summary>
        /// Dim0 = col
        /// Dim1 = row
        /// [col, row] => hexagon
        /// </summary>
        public GameObject[,] HexagonGrid { get; private set; }

        private void Start()
        {
            HexagonGrid = new GameObject[GameParamsDatabase.Instance.ColumnCount, GameParamsDatabase.Instance.RowCount];
        }

        public GameObject this[OffsetCoordinates offsetCoordinates]
        {
            get => HexagonGrid[offsetCoordinates.Col, offsetCoordinates.Row];
            set => HexagonGrid[offsetCoordinates.Col, offsetCoordinates.Row] = value;
        }

        public (GameObject alpha, GameObject bravo, GameObject charlie) this[Group group] =>
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
            var temp = Instance.HexagonGrid[col, rowB];

            // b <- a
            Instance.HexagonGrid[col, rowB] = Instance.HexagonGrid[col, rowA];

            // a <- temp
            Instance.HexagonGrid[col, rowA] = temp;
        }
    }
}
