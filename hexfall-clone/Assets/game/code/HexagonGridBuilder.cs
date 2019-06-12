using System;
using UnityEngine;

namespace starikcetin.hexfallClone
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

        private void Start()
        {
            BuildHexagonGrid(_size);
        }

        private void BuildHexagonGrid(float size)
        {
            var centerOffset = CalculateCenterOffset(size, _columnCount, _rowCount);

            for (var col = 0; col < _columnCount; col++)
            {
                for (var row = 0; row < _rowCount; row++)
                {
                    var offsetCoordinates = new OffsetCoordinates(col, row);

                    var newHexagon = Instantiate(PrefabDatabase.Instance.Hexagon, transform);
                    newHexagon.transform.position = offsetCoordinates.ToUnity(size) - centerOffset;
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
