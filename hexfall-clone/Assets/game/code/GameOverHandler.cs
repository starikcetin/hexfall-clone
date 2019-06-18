using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace starikcetin.hexfallClone.game
{
    public class GameOverHandler : SceneSingleton<GameOverHandler>
    {
        [SerializeField] private SceneReference _gameOverScene;

        public void DeclareGameOver()
        {
            Utils.LogConditional("----- Game Over -----");

            SceneManager.LoadScene(_gameOverScene.ScenePath);
        }
    }
}
