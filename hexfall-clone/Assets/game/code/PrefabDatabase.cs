using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

public class PrefabDatabase : SceneSingleton<PrefabDatabase>
{
    [SerializeField] private GameObject _hexagon;
    public GameObject Hexagon => _hexagon;

    [SerializeField] private GameObject _bombHexagon;
    public GameObject BombHexagon => _bombHexagon;
}
