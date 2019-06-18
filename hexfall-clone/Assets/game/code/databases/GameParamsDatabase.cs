using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game.databases
{
    public class GameParamsDatabase : SceneSingleton<GameParamsDatabase>
    {
        [SerializeField] private int _bombLife;
        public int BombLife => _bombLife;

        [SerializeField] public int _bombScore;
        public int BombScore => _bombScore;

        [SerializeField] private int _scorePerExplosion;
        public int ScorePerExplosion => _scorePerExplosion;

        public float Size { get; set; }
        public Vector2 CenterOffset { get; set; }
    }
}
