using System.Collections.Generic;
using Eflatun.UnityCommon.Utils.CodePatterns;
using MoreLinq;
using starikcetin.hexfallClone.game.mechanics;
using UnityEngine;

namespace starikcetin.hexfallClone.game.databases
{
    public class GroupDatabase : SceneSingleton<GroupDatabase>
    {
        private readonly List<Group> _groups = new List<Group>();
        public IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();

        public void RegisterGroup(Group group)
        {
            _groups.Add(group);

//        var highlighter = Utils._Debug_Highlight(group.Center, Color.black);
//        highlighter.name = "debug_group_highlighter";
        }

        public Group FindClosestGroup(Vector2 point, float size)
        {
            float Selector(Group group)
            {
                var groupCenter = group.Center;
                float distance = Vector3.SqrMagnitude(point - groupCenter);
                return distance;
            }

            IExtremaEnumerable<Group> mins = _groups.MinBy(Selector);
            var closestGroup = mins.First();
            return closestGroup;
        }

    }
}
