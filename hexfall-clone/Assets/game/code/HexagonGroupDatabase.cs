using System.Collections.Generic;
using MoreLinq;
using starikcetin.hexfallClone;
using UnityEngine;

public class HexagonGroupDatabase : Singleton<HexagonGroupDatabase>
{
    private readonly List<HexagonGroup> _hexagonGroups = new List<HexagonGroup>();
    public IReadOnlyCollection<HexagonGroup> HexagonGroups => _hexagonGroups.AsReadOnly();

    public void RegisterHexagonGroup(HexagonGroup group)
    {
        _hexagonGroups.Add(group);
    }

    public HexagonGroup FindClosestGroup(Vector3 point, float size)
    {
        return _hexagonGroups.MinBy(
            g => Vector3.SqrMagnitude(point - g.GetCenter(size))
        ).First();
    }
}
