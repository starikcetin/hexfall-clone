﻿using System.Collections.Generic;
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

        var highlighter = Utils._Debug_Highlight(group.Center, Color.black);
        highlighter.name = "debug_group_highlighter";
    }

    public HexagonGroup FindClosestGroup(Vector2 point, float size)
    {
        float Selector(HexagonGroup group)
        {
            var groupCenter = group.Center;
            float distance = Vector3.SqrMagnitude(point - groupCenter);
            return distance;
        }

        IExtremaEnumerable<HexagonGroup> mins = _hexagonGroups.MinBy(Selector);
        var closestGroup = mins.First();
        return closestGroup;
    }

}