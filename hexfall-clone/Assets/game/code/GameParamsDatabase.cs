using UnityEngine;

public class GameParamsDatabase : Singleton<GameParamsDatabase>
{
    [SerializeField] private int _bombLife;
    public int BombLife => _bombLife;

    [SerializeField] public int _bombScore;
    public int BombScore => _bombScore;

    public float Size { get; set; }
    public Vector2 CenterOffset { get; set; }
}
