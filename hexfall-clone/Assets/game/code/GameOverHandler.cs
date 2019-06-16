using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : SceneSingleton<GameOverHandler>
{
    [SerializeField] private SceneReference _gameOverScene;

    public void DeclareGameOver()
    {
        // TODO
        Debug.Log("----- Game Over -----");

        SceneManager.LoadScene(_gameOverScene.ScenePath);
    }
}
