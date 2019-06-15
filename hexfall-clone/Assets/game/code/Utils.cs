using System.Collections.Generic;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public static class Utils
    {
        public static GameObject _Debug_Highlight(Vector3 position, Color color)
        {
            var highlighter = MonoBehaviour.Instantiate(PrefabDatabase.Instance.Hexagon);
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
    }
}
