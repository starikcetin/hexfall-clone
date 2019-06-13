using System.Collections.Generic;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class HexagonGridBuilder : MonoBehaviour
    {
        [SerializeField] private int _columnCount, _rowCount;
        [SerializeField] private float _size;

        private HexagonGrid _hexagonGrid;
        private IEnumerable<HexagonGroup> _hexagonGroups;

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
            _hexagonGroups = AssembleHexagonGroups(_hexagonGrid);
        }

        private HexagonGrid BuildHexagonGrid(float size)
        {
            var hexGrid = new HexagonGrid(_columnCount, _rowCount);

            var centerOffset = CalculateCenterOffset(size, _columnCount, _rowCount);

            for (var col = 0; col < _columnCount; col++)
            {
                for (var row = 0; row < _rowCount; row++)
                {
                    var offsetCoordinates = new OffsetCoordinates(col, row);

                    var newHexagon = Instantiate(PrefabDatabase.Instance.Hexagon, transform);
                    newHexagon.transform.position = offsetCoordinates.ToUnity(size) - centerOffset;

                    hexGrid.Set(offsetCoordinates, newHexagon);
                }
            }

            return hexGrid;
        }

        private IEnumerable<HexagonGroup> AssembleHexagonGroups(HexagonGrid hexagonGrid)
        {
            for (int col = 0; col < hexagonGrid.OffsetGrid.GetLength(0); col++)
            for (int row = 0; row < hexagonGrid.OffsetGrid.GetLength(1); row++)
            {
                var hex = hexagonGrid.OffsetGrid[col, row];

                var alphaCoords = new OffsetCoordinates(col, row);

                if (alphaCoords.Col < _columnCount-1 && alphaCoords.Row < _rowCount-1)
                {
                    var bravoCoords = alphaCoords.WithRow(alphaCoords.Row + 1);
                    var charlieCoords = alphaCoords.WithCol(alphaCoords.Col + 1);

                    yield return new HexagonGroup(alphaCoords, bravoCoords, charlieCoords);
                }
            }
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
