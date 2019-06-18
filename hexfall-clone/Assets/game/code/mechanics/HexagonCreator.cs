using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
    public class HexagonCreator : SceneSingleton<HexagonCreator>
    {
        /// <summary>
        /// DOES NOT REGISTER THE NEW HEXAGON WITH <see cref="HexagonDatabase"/>. MAKE IT YOURSELF.
        /// </summary>
        public GameObject CreateHexagon(OffsetCoordinates offsetCoordinates, bool isBomb)
        {
            var prefab = isBomb ? PrefabDatabase.Instance.BombHexagon : PrefabDatabase.Instance.Hexagon;

            var newHexagon = Instantiate(prefab, transform);
            newHexagon.transform.position = offsetCoordinates.ToUnity();
            var colour = ColourDatabase.Instance.RandomColour();
            newHexagon.GetComponent<Hexagon>().SetColor(colour);
            return newHexagon;
        }
    }
}
