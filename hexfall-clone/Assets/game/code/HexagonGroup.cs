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
    }
}
