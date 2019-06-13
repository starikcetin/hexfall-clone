namespace starikcetin.hexfallClone
{
    internal class HexagonGroup
    {
        private readonly OffsetCoordinates _alphaCoords;
        private readonly OffsetCoordinates _bravoCoords;
        private readonly OffsetCoordinates _charlieCoords;

        public HexagonGroup(OffsetCoordinates alphaCoords, OffsetCoordinates bravoCoords, OffsetCoordinates charlieCoords)
        {
            _alphaCoords = alphaCoords;
            _bravoCoords = bravoCoords;
            _charlieCoords = charlieCoords;
        }
    }
}
