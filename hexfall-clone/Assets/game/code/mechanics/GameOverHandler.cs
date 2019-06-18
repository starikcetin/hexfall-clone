using starikcetin.hexfallClone.game.databases;
using UnityEngine.SceneManagement;

namespace starikcetin.hexfallClone.game.mechanics
{
    public static class GameOverHandler
    {
        public static void DeclareGameOver()
        {
            Utils.LogConditional("----- Game Over -----");

            SceneManager.LoadScene(SceneDatabase.Instance.GameOverScene.ScenePath);
        }
    }
}
