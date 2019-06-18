using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : SceneSingleton<GameOverHandler>
{
    [SerializeField] private SceneReference _gameOverScene;

    public void DeclareGameOver()
    {
        Utils.LogConditional("----- Game Over -----");

        SceneManager.LoadScene(_gameOverScene.ScenePath);
    }
}
