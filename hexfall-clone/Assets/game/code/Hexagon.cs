using System;
using System.Collections;
using DG.Tweening;
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
                .EventTransition(() =>
                {
                    if (gameObject)
                    {
                        Destroy(gameObject);
                    }
                }, 0f);
        }

        public IEnumerator MoveTo(Vector3 target, float time)
        {
            GetComponent<Rigidbody2D>().DOMove(target, time);

            //transform.positionTransition(target, time);
            yield return new WaitForSeconds(time);
        }
    }
}
