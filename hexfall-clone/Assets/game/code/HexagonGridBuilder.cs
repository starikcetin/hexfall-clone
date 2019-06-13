using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class HexagonGridBuilder : MonoBehaviour
    {
        [SerializeField] private int _columnCount, _rowCount;
        [SerializeField] private float _size;

        /// <summary>
        /// Dim0 = col
        /// Dim1 = row
        /// [col, row] => hexagon
        /// </summary>
        private GameObject[,] _hexagonGrid;

        private List<HexagonGroup> _hexagonGroups;

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

        private void Start()
        {
            _hexagonGrid = BuildHexagonGrid(_size);
            _hexagonGroups = AssembleHexagonGroups().ToList();
        }

        private IEnumerable<HexagonGroup> AssembleHexagonGroups()
        {
            // 2-left
            for (int col = 0; col < _hexagonGrid.GetLength(0) - 1; col++)
            for (int row = 0; row < _hexagonGrid.GetLength(1) - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row + 1);

                yield return new HexagonGroup(alpha, bravo, charlie);
            }

            // 2-right
            for (int col = 0; col < _hexagonGrid.GetLength(0) - 1; col++)
            for (int row = 0; row < _hexagonGrid.GetLength(1) - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col + 1, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row);

                yield return new HexagonGroup(alpha, bravo, charlie);
            }
        }

        private GameObject[,] BuildHexagonGrid(float size)
        {
            GameObject[,] grid = new GameObject[_columnCount, _rowCount];

            var centerOffset = CalculateCenterOffset(size, _columnCount, _rowCount);

            for (var col = 0; col < _columnCount; col++)
            {
                for (var row = 0; row < _rowCount; row++)
                {
                    var offsetCoordinates = new OffsetCoordinates(col, row);
                    grid[col, row] = CreateHexagon(size, offsetCoordinates, centerOffset);
                }
            }

            return grid;
        }

        private GameObject CreateHexagon(float size, OffsetCoordinates offsetCoordinates, Vector2 centerOffset)
        {
            var newHexagon = Instantiate(PrefabDatabase.Instance.Hexagon, transform);
            newHexagon.transform.position = offsetCoordinates.ToUnity(size) - centerOffset;
            newHexagon.GetComponentInChildren<Renderer>().material.color = ColourDatabase.Instance.RandomColour();
            return newHexagon;
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
