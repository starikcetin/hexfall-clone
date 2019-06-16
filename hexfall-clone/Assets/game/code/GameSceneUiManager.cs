using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUiManager : MonoBehaviour
{
    [SerializeField] private Text _score;

    private void Start()
    {
        ScoreDatabase.Instance.ScoreChanged += ScoreDatabaseOnScoreChanged;
    }

    private void OnDestroy()
    {
        if (ScoreDatabase.Instance)
        {
            ScoreDatabase.Instance.ScoreChanged -= ScoreDatabaseOnScoreChanged;
        }
    }

    private void ScoreDatabaseOnScoreChanged(int score)
    {
        _score.text = score.ToString();
    }
}
