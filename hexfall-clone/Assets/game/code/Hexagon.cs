using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class Hexagon : MonoBehaviour
    {
        public Color Color { get; private set; }

        public void SetColor(Color color)
        {
            Color = color;
            GetComponentInChildren<Renderer>().material.color = color;
        }
    }
}
