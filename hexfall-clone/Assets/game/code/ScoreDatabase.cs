using System;
using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    public class ScoreDatabase : GlobalSingleton<ScoreDatabase>
    {
        public event Action<int> ScoreChanged;
        public event Action BombScoreReached;

        public int Score { get; private set; }

        public void OnHexagonExploded()
        {
            // 5 points per hexagon
            // TODO make a game parameter
            Score += 5;

            if (Score > 0 && Score % GameParamsDatabase.Instance.BombScore == 0)
            {
                BombScoreReached?.Invoke();
            }

            ScoreChanged?.Invoke(Score);
        }

        public void ResetScore()
        {
            Debug.Log(nameof(ResetScore));
            Score = 0;
        }
    }
}
