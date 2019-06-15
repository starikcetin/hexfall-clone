using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class CountdownController : MonoBehaviour
{
    private void Start()
    {
        var bomb = GetComponentInParent<Bomb>();
        bomb.LifeChange += OnLifeChange;
        OnLifeChange(bomb.LivesLeft);
    }

    private void OnLifeChange(int livesLeft)
    {
        GetComponent<TextMesh>().text = livesLeft.ToString();
    }
}
