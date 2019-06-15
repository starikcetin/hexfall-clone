using UnityEngine;

public class GameParamsDatabase : Singleton<GameParamsDatabase>
{
    public float Size { get; set; }
    public Vector2 CenterOffset { get; set; }
}
