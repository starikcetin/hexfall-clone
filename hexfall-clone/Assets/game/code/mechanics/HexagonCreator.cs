using System.Collections.Generic;
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
        public GameObject CreateHexagon(OffsetCoordinates offsetCoordinates, bool isBomb, bool performColorCheck)
        {
            var prefab = isBomb ? PrefabDatabase.Instance.BombHexagon : PrefabDatabase.Instance.Hexagon;

            var newHexagon = Instantiate(prefab, transform);
            newHexagon.transform.position = offsetCoordinates.ToUnity();


            var colour = GetColor(performColorCheck, offsetCoordinates);


            newHexagon.GetComponent<Hexagon>().SetColor(colour);


            return newHexagon;
        }

        private Color GetColor(bool performColorCheck, OffsetCoordinates offsetCoordinates)
        {
            if (performColorCheck)
            {
                var checkA = new OffsetCoordinates(offsetCoordinates.Col-1, offsetCoordinates.Row);
                var checkB = new OffsetCoordinates(offsetCoordinates.Col-1, offsetCoordinates.Row-1);
                var checkC = new OffsetCoordinates(offsetCoordinates.Col, offsetCoordinates.Row-1);

                var colA = HexagonDatabase.Instance[checkA].GetComponent<Hexagon>().Color;
                var colB = HexagonDatabase.Instance[checkB].GetComponent<Hexagon>().Color;
                var colC = HexagonDatabase.Instance[checkC].GetComponent<Hexagon>().Color;

                if (colA == colB)
                {
                    return ColourDatabase.Instance.RandomColour(except: colA);
                }

                if (colB == colC)
                {
                    return ColourDatabase.Instance.RandomColour(except: colB);
                }

                if (colA == colC)
                {
                    return ColourDatabase.Instance.RandomColour(except: colA);
                }
            }

            return ColourDatabase.Instance.RandomColour();
        }
    }
}
