using System;
using Eflatun.UnityCommon.Utils.CodePatterns;

public class ScoreDatabase : SceneSingleton<ScoreDatabase>
{
    public event Action BombScoreReached;

    public int Score { get; private set; }

    public void OnHexagonExploded()
    {
        // 5 points per hexagon
        // TODO make a game parameter
        Score += 5;
        UnityEngine.Debug.Log($"Score: {Score}");

        if (Score > 0 && Score % GameParamsDatabase.Instance.BombScore == 0)
        {
            BombScoreReached?.Invoke();
        }
    }
}
