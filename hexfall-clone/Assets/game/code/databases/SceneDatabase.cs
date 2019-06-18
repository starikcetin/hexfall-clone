using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game.databases
{
    public class SceneDatabase : GlobalSingleton<SceneDatabase>
    {
//        [SerializeField] private SceneReference _titleScene;
//        public SceneReference TitleScene => _titleScene;

        [SerializeField] private SceneReference _gameScene;
        public SceneReference GameScene => _gameScene;

        [SerializeField] private SceneReference _gameOverScene;
        public SceneReference GameOverScene => _gameOverScene;
    }
}
