using UnityEngine;

namespace starikcetin.hexfallClone
{
    /// <summary>
    /// Alpha > Bravo > Charlie.
    /// Clockwise.
    /// Alpha is always the bottom-left (the minimum).
    /// </summary>
    public struct HexagonGroup
    {
        public OffsetCoordinates Alpha { get; }
        public OffsetCoordinates Bravo { get; }
        public OffsetCoordinates Charlie { get; }

        public HexagonGroup(OffsetCoordinates alpha, OffsetCoordinates bravo, OffsetCoordinates charlie)
        {
            Alpha = alpha;
            Bravo = bravo;
            Charlie = charlie;
        }

        public Vector2 Center
        {
            get
            {
                var size = GameParamsDatabase.Instance.Size;
                return (Alpha.ToUnity(size) + Bravo.ToUnity(size) + Charlie.ToUnity(size)) / 3f;
            }
        }

        public bool Has(OffsetCoordinates coords)
        {
            return Alpha == coords || Bravo == coords || Charlie == coords;
        }
    }
}
