using System.Collections.Generic;
using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

namespace starikcetin.hexfallClone.game
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
    }
}
