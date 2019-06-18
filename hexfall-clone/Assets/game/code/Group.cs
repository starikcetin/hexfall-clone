using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    /// <summary>
    /// Represents a group of 3 hexagons (all 3 share a common corner).
    /// Alpha > Bravo > Charlie.
    /// Clockwise.
    /// Alpha is always the bottom-left (the minimum).
    /// </summary>
    public struct Group
    {
        public OffsetCoordinates Alpha { get; }
        public OffsetCoordinates Bravo { get; }
        public OffsetCoordinates Charlie { get; }

        public GroupOrientation Orientation { get; }

        public Group(OffsetCoordinates alpha, OffsetCoordinates bravo, OffsetCoordinates charlie, GroupOrientation orientation)
        {
            Alpha = alpha;
            Bravo = bravo;
            Charlie = charlie;
            Orientation = orientation;
        }

        public Vector2 Center
        {
            get
            {
                var size = GameParamsDatabase.Instance.Size;
                return (Alpha.ToUnity(size) + Bravo.ToUnity(size) + Charlie.ToUnity(size)) / 3f;
            }
        }
    }
}
