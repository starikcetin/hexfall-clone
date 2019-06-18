using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game.databases
{
    public class GameParamsDatabase : SceneSingleton<GameParamsDatabase>
    {
        [SerializeField] private int _bombLife;
        public int BombLife => _bombLife;

        [SerializeField] public int _bombScore;
        public int BombScore => _bombScore;

        [SerializeField] private int _scorePerExplosion;
        public int ScorePerExplosion => _scorePerExplosion;

        [SerializeField] private int _columnCount;
        public int ColumnCount => _columnCount;

        [SerializeField] private int _rowCount;
        public int RowCount => _rowCount;

        [SerializeField] private float _size;
        public float Size => _size;

        public Vector2 CenterOffset { get; private set; }

        private float HexWidth => Size * 2;
        private float HexHeight => Size * Mathf.Sqrt(3);

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
            CenterOffset = CalculateCenterOffset(Size, ColumnCount, RowCount);
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
