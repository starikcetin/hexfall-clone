using Eflatun.UnityCommon.Utils.CodePatterns;
using starikcetin.hexfallClone.game.databases;
using UnityEngine.SceneManagement;

namespace starikcetin.hexfallClone.game.mechanics
{
    public class GameOverHandler : SceneSingleton<GameOverHandler>
    {
        public void DeclareGameOver()
        {
            Utils.LogConditional("----- Game Over -----");

            SceneManager.LoadScene(SceneDatabase.Instance.GameOverScene.ScenePath);
        }
    }
}
