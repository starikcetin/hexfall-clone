using System;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;

namespace starikcetin.hexfallClone.game.mechanics
{
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
            GameManager.Instance.ActionSequenceCompleted += InstanceOnActionSequenceCompleted;
        }

        private void OnDisable()
        {
            GameManager.Instance.ActionSequenceCompleted -= InstanceOnActionSequenceCompleted;
        }

        private void InstanceOnActionSequenceCompleted()
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
}
