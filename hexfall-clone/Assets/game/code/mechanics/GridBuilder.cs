using System.Collections.Generic;
using starikcetin.hexfallClone.game.databases;
using starikcetin.hexfallClone.game.visual;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
    public class GridBuilder : MonoBehaviour
    {
        private void Start()
        {
            var columnCount = GameParamsDatabase.Instance.ColumnCount;
            var rowCount = GameParamsDatabase.Instance.RowCount;

            HexagonDatabase.Instance.HexagonGrid = BuildHexagonGrid(columnCount, rowCount);

            foreach (var hexagonGroup in AssembleHexagonGroups(columnCount, rowCount))
            {
                GroupDatabase.Instance.RegisterGroup(hexagonGroup);
            }
        }

        private IEnumerable<Group> AssembleHexagonGroups(int columnCount, int rowCount)
        {
            // 2-right even (a)

            for (int col = 0; col < columnCount - 1; col+=2)
            for (int row = 0; row < rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col + 1, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row);

                yield return new Group(alpha, bravo, charlie, GroupOrientation.TwoRight);
            }

            // 2-left even (b)

            for (int col = 0; col < columnCount - 1; col+=2)
            for (int row = 0; row < rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row + 1);

                yield return new Group(alpha, bravo, charlie, GroupOrientation.TwoLeft);
            }

            // 2-right odd (c)

            for (int col = 1; col < columnCount - 1; col+=2)
            for (int row = 1; row < rowCount; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col + 1, row);
                var charlie = new OffsetCoordinates(col + 1, row - 1);

                yield return new Group(alpha, bravo, charlie, GroupOrientation.TwoRight);
            }

            // 2-left odd (d)

            for (int col = 1; col < columnCount - 1; col+=2)
            for (int row = 0; row < rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row);

                yield return new Group(alpha, bravo, charlie, GroupOrientation.TwoLeft);
            }
        }

        private GameObject[,] BuildHexagonGrid(int columnCount, int rowCount)
        {
            GameObject[,] grid = new GameObject[columnCount, rowCount];

            for (var col = 0; col < columnCount; col++)
            {
                for (var row = 0; row < rowCount; row++)
                {
                    var offsetCoordinates = new OffsetCoordinates(col, row);
                    var hex = HexagonCreator.Instance.CreateHexagon(offsetCoordinates, false);
                    hex.name = $"({col}, {row})";
                    grid[col, row] = hex;
                }
            }

            return grid;
        }


    }
}
