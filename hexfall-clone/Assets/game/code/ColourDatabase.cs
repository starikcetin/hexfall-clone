using System.Collections.Generic;
using UnityEngine;

public class ColourDatabase : Singleton<ColourDatabase>
{
    [SerializeField] private List<Color> _colours;
    public List<Color> Colours => _colours;

    public Color RandomColour()
    {
        int index = Random.Range(0, _colours.Count);
        return _colours[index];
    }
}
