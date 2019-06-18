using System;
using Eflatun.UnityCommon.Utils.CodePatterns;

namespace starikcetin.hexfallClone.game.databases
{
    public class ScoreDatabase : GlobalSingleton<ScoreDatabase>
    {
        public event Action<int> ScoreChanged;
        public event Action BombScoreReached;

        public int Score { get; private set; }

        public void OnHexagonExploded()
        {
            Score += GameParamsDatabase.Instance.ScorePerExplosion;

            if (Score > 0 && Score % GameParamsDatabase.Instance.BombScore == 0)
            {
                BombScoreReached?.Invoke();
            }

            ScoreChanged?.Invoke(Score);
        }

        public void ResetScore()
        {
            Utils.LogConditional(nameof(ResetScore));
            Score = 0;
        }
    }
}
