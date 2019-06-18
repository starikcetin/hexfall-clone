using starikcetin.hexfallClone.game.databases;
using starikcetin.hexfallClone.game.visual;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
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

        public Vector2 Center => (Alpha.ToUnity() + Bravo.ToUnity() + Charlie.ToUnity()) / 3f;
    }
}
