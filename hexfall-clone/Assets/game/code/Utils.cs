using System.Collections.Generic;
using System.Diagnostics;
using starikcetin.hexfallClone.game.databases;
using starikcetin.hexfallClone.game.mechanics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace starikcetin.hexfallClone.game
{
    public static class Utils
    {
        public static GameObject _Debug_Highlight(Vector3 position, Color color)
        {
            var highlighter = Object.Instantiate(PrefabDatabase.Instance.Hexagon);
            highlighter.transform.localScale /= 5f;
            highlighter.GetComponentInChildren<Renderer>().material.color = color;
            highlighter.transform.position = position + new Vector3(0, 0, -1);
            return highlighter;
        }

        public static void Swap<T>(this IList<T> arr, int ia, int ib)
        {
            var a = arr[ia];
            arr[ia] = arr[ib];
            arr[ib] = a;
        }

        /// <summary>
        /// Checks if <paramref name="group"/> contains <paramref name="foundColor"/>.
        /// </summary>
        public static int CountForColor(Group group, Color foundColor)
        {
            int count = 0;

            var (ac, bc, cc) = GetColors(group);

            if (ac == foundColor) count++;
            if (bc == foundColor) count++;
            if (cc == foundColor) count++;

            return count;
        }

        public static (Color alphaColor, Color bravoColor, Color charlieColor) GetColors(Group group)
        {
            var (a, b, c) = HexagonDatabase.Instance[group];
            var (ah, bh, ch) = (a.GetComponent<Hexagon>(), b.GetComponent<Hexagon>(), c.GetComponent<Hexagon>());
            return (ah.Color, bh.Color, ch.Color);
        }

        public static bool Contains(Group g, OffsetCoordinates oc)
        {
            return g.Alpha == oc || g.Bravo == oc || g.Charlie == oc;
        }

        public static bool IsSameColor(Hexagon a, Hexagon b, Hexagon c)
        {
            var ac = a.Color;
            var bc = b.Color;
            var cc = c.Color;

            return ac == bc && bc == cc && ac == cc;
        }

        [Conditional("DEBUG_LOGS")]
        public static void LogConditional(string str)
        {
            Debug.Log(str);
        }
    }
}
