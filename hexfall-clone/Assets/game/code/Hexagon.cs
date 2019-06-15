using System;
using Lean.Transition;
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

        public void ExplodeSelf()
        {
            var originalScale = transform.localScale;
            var targetScale = originalScale * 0.01f;

            transform.localScaleTransition(targetScale, 0.5f)
                .JoinTransition()
                .EventTransition(() => Destroy(gameObject), 0f);
        }
    }
}