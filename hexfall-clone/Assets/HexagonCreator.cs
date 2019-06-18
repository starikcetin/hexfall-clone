using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone.game;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class HexagonCreator : SceneSingleton<HexagonCreator>
    {
        /// <summary>
        /// DOES NOT REGISTER THE NEW HEXAGON WITH <see cref="HexagonDatabase"/>. MAKE IT YOURSELF.
        /// </summary>
        public GameObject CreateHexagon(float size, OffsetCoordinates offsetCoordinates, bool isBomb)
        {
            var prefab = isBomb ? PrefabDatabase.Instance.BombHexagon : PrefabDatabase.Instance.Hexagon;

            var newHexagon = Instantiate(prefab, transform);
            newHexagon.transform.position = offsetCoordinates.ToUnity(size);
            var colour = ColourDatabase.Instance.RandomColour();
            newHexagon.GetComponent<Hexagon>().SetColor(colour);
            return newHexagon;
        }
    }
}
