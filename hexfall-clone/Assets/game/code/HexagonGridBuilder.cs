using System.Collections.Generic;
using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    public class HexagonGridBuilder : MonoBehaviour
    {
        [SerializeField] private int _columnCount, _rowCount;
        [SerializeField] private float _size;

        private float HexWidth => _size * 2;
        private float HexHeight => _size * Mathf.Sqrt(3);

        /// <summary>
        /// Horizontal distance between two adjacent hexagon centers in the same row.
        /// </summary>
        private float HexHorizontalDistance => HexWidth * 3f / 4f;

        /// <summary>
        /// Vertical distance between two adjacent hexagon centers in the same column.
        /// </summary>
        private float HexVerticalDistance => HexHeight;

        protected void Awake()
        {
            // Write Game Params
            GameParamsDatabase.Instance.Size = _size;
            GameParamsDatabase.Instance.CenterOffset = CalculateCenterOffset(_size, _columnCount, _rowCount);
        }

        private void Start()
        {
            HexagonDatabase.Instance.HexagonGrid = BuildHexagonGrid(_size);

            foreach (var hexagonGroup in AssembleHexagonGroups())
            {
                HexagonGroupDatabase.Instance.RegisterHexagonGroup(hexagonGroup);
            }
        }

        private IEnumerable<HexagonGroup> AssembleHexagonGroups()
        {
            // 2-right even (a)

            for (int col = 0; col < _columnCount - 1; col+=2)
            for (int row = 0; row < _rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col + 1, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row);

                yield return new HexagonGroup(alpha, bravo, charlie, GroupOrientation.TwoRight);
            }

            // 2-left even (b)

            for (int col = 0; col < _columnCount - 1; col+=2)
            for (int row = 0; row < _rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row + 1);

                yield return new HexagonGroup(alpha, bravo, charlie, GroupOrientation.TwoLeft);
            }

            // 2-right odd (c)

            for (int col = 1; col < _columnCount - 1; col+=2)
            for (int row = 1; row < _rowCount; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col + 1, row);
                var charlie = new OffsetCoordinates(col + 1, row - 1);

                yield return new HexagonGroup(alpha, bravo, charlie, GroupOrientation.TwoRight);
            }

            // 2-left odd (d)

            for (int col = 1; col < _columnCount - 1; col+=2)
            for (int row = 0; row < _rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row);

                yield return new HexagonGroup(alpha, bravo, charlie, GroupOrientation.TwoLeft);
            }
        }

        private GameObject[,] BuildHexagonGrid(float size)
        {
            GameObject[,] grid = new GameObject[_columnCount, _rowCount];

            for (var col = 0; col < _columnCount; col++)
            {
                for (var row = 0; row < _rowCount; row++)
                {
                    var offsetCoordinates = new OffsetCoordinates(col, row);
                    var hex = HexagonCreator.Instance.CreateHexagon(size, offsetCoordinates, false);
                    hex.name = $"({col}, {row})";
                    grid[col, row] = hex;
                }
            }

            return grid;
        }
        
        private Vector2 CalculateCenterOffset(float size, int colCount, int rowCount)
        {
            var totalWidth = HexHorizontalDistance * (colCount - 1);
            var totalHeight = HexVerticalDistance * (rowCount - 1);

            if (colCount > 1 || colCount < -1)
            {
                totalHeight += HexHeight / 2;
            }

            return new Vector2(totalWidth / 2, totalHeight / 2);
        }
    }
}
