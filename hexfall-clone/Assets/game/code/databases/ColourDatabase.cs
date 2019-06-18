using System.Collections.Generic;
using System.Linq;
using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game.databases
{
    public class ColourDatabase : SceneSingleton<ColourDatabase>
    {
        [SerializeField] private List<Color> _colours;
        public List<Color> Colours => _colours;

        public Color RandomColour()
        {
            int index = Random.Range(0, _colours.Count);
            return _colours[index];
        }

        public Color RandomColour(Color except)
        {
            var filtered = _colours.Except(new[] {except}).ToArray();
            int index = Random.Range(0, filtered.Length);
            return filtered[index];
        }
    }
}
