using UnityEngine;

public class PrefabDatabase : Singleton<PrefabDatabase>
{
    [SerializeField] private GameObject _hexagon;
    public GameObject Hexagon => _hexagon;
}
