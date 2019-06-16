﻿using System.Collections;
using System.Collections.Generic;
using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class HexagonGridBuilder : SceneSingleton<HexagonGridBuilder>
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
            GameParamsDatabase.Instance.CenterOffset = Vector2.zero; //CalculateCenterOffset(size, _columnCount, _rowCount);
        }

        private void Start()
        {
            StartCoroutine(AfterStart());
        }

        IEnumerator AfterStart()
        {
            //yield return null;

            HexagonDatabase.Instance.HexagonGrid = BuildHexagonGrid(_size);

            foreach (var hexagonGroup in AssembleHexagonGroups())
            {
                HexagonGroupDatabase.Instance.RegisterHexagonGroup(hexagonGroup);
                //yield return null;
            }

            yield return 0;
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

//                var ha = Utils._Debug_Highlight(alpha.ToUnity(_size), Color.white);
//                var hb = Utils._Debug_Highlight(bravo.ToUnity(_size), Color.white);
//                var hc = Utils._Debug_Highlight(charlie.ToUnity(_size), Color.white);

                yield return new HexagonGroup(alpha, bravo, charlie);

//                Destroy(ha);
//                Destroy(hb);
//                Destroy(hc);
            }

            // 2-left even (b)

            for (int col = 0; col < _columnCount - 1; col+=2)
            for (int row = 0; row < _rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row + 1);

//                var ha = Utils._Debug_Highlight(alpha.ToUnity(_size), Color.white);
//                var hb = Utils._Debug_Highlight(bravo.ToUnity(_size), Color.white);
//                var hc = Utils._Debug_Highlight(charlie.ToUnity(_size), Color.white);

                yield return new HexagonGroup(alpha, bravo, charlie);

//                Destroy(ha);
//                Destroy(hb);
//                Destroy(hc);
            }

            // 2-right odd (c)

            for (int col = 1; col < _columnCount - 1; col+=2)
            for (int row = 1; row < _rowCount; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col + 1, row);
                var charlie = new OffsetCoordinates(col + 1, row - 1);

//                var ha = Utils._Debug_Highlight(alpha.ToUnity(_size), Color.black);
//                var hb = Utils._Debug_Highlight(bravo.ToUnity(_size), Color.black);
//                var hc = Utils._Debug_Highlight(charlie.ToUnity(_size), Color.black);

                yield return new HexagonGroup(alpha, bravo, charlie);

//                Destroy(ha);
//                Destroy(hb);
//                Destroy(hc);
            }

            // 2-left odd (d)

            for (int col = 1; col < _columnCount - 1; col+=2)
            for (int row = 0; row < _rowCount - 1; row++)
            {
                var alpha = new OffsetCoordinates(col, row);
                var bravo = new OffsetCoordinates(col, row + 1);
                var charlie = new OffsetCoordinates(col + 1, row);

//                var ha = Utils._Debug_Highlight(alpha.ToUnity(_size), Color.black);
//                var hb = Utils._Debug_Highlight(bravo.ToUnity(_size), Color.black);
//                var hc = Utils._Debug_Highlight(charlie.ToUnity(_size), Color.black);

                yield return new HexagonGroup(alpha, bravo, charlie);

//                Destroy(ha);
//                Destroy(hb);
//                Destroy(hc);
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
                    var hex = CreateHexagon(size, offsetCoordinates, false);
                    hex.name = $"({col}, {row})";
                    grid[col, row] = hex;
                }
            }

            return grid;
        }

        /// <summary>
        /// DOES NOT REGISTER THE NEW HEXAGON WITH <see cref="HexagonDatabase"/>. MAKE IT YOURSELF.
        /// </summary>
        public GameObject CreateHexagon(float size, OffsetCoordinates offsetCoordinates, bool isBomb)
        {
            var prefab = isBomb ? PrefabDatabase.Instance.BombHexagon : PrefabDatabase.Instance.Hexagon;

            var newHexagon = Instantiate(prefab, transform);
            newHexagon.transform.position = offsetCoordinates.ToUnity(size) - GameParamsDatabase.Instance.CenterOffset;
            var colour = ColourDatabase.Instance.RandomColour();
            newHexagon.GetComponent<Hexagon>().SetColor(colour);
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
