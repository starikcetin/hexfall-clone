using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

public class GameParamsDatabase : SceneSingleton<GameParamsDatabase>
{
    [SerializeField] private int _bombLife;
    public int BombLife => _bombLife;

    [SerializeField] public int _bombScore;
    public int BombScore => _bombScore;

    public float Size { get; set; }
    public Vector2 CenterOffset { get; set; }
}
