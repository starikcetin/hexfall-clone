using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class HexagonGrid //: IEnumerable<(OffsetCoordinates, GameObject)>
    {
        /// <summary>
        /// [Col, Row] => [Hexagon]
        /// </summary>
        public GameObject[,] OffsetGrid { get; }

        public HexagonGrid(int colCount, int rowCount)
        {
            OffsetGrid = new GameObject[colCount, rowCount];
        }

        public void Set(OffsetCoordinates offsetCoordinates, GameObject hexagon)
        {
            OffsetGrid[offsetCoordinates.Col, offsetCoordinates.Row] = hexagon;
        }

        public GameObject Get(OffsetCoordinates offsetCoordinates)
        {
            return OffsetGrid[offsetCoordinates.Col, offsetCoordinates.Row];
        }

//        public IEnumerator<(OffsetCoordinates, GameObject)> GetEnumerator()
//        {
//            for (int col = 0; col < OffsetGrid.GetLength(0); col++)
//            for (int row = 0; row < OffsetGrid.GetLength(1); row++)
//            {
//                var hexagon = OffsetGrid[col, row];
//                yield return (new OffsetCoordinates(col, row), hexagon);
//            }
//        }
//
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }

//        public GameObject this[int col, int row]
//        {
//            get => OffsetGrid[col, row];
//            set => OffsetGrid[col, row] = value;
//        }

        public GameObject this[OffsetCoordinates coords]
        {
            get => OffsetGrid[coords.Col, coords.Row];
            set => OffsetGrid[coords.Col, coords.Row] = value;
        }
    }
}
