using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    /// <summary>
    /// Parameter is the lives left.
    /// </summary>
    public event Action<int> LifeChange;

    public int LivesLeft { get; private set; }

    private void Start()
    {
        LivesLeft = GameParamsDatabase.Instance.BombLife;
        GameManager.Instance.ActionDone += InstanceOnActionDone;
    }

    private void OnDisable()
    {
        GameManager.Instance.ActionDone -= InstanceOnActionDone;
    }

    private void InstanceOnActionDone()
    {
        LivesLeft--;
        
        LifeChange?.Invoke(LivesLeft);

        if (LivesLeft <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameOverHandler.Instance.DeclareGameOver();
    }
}
