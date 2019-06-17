﻿using System.Collections.Generic;
using Eflatun.UnityCommon.Utils.CodePatterns;
using UnityEngine;

public class ColourDatabase : SceneSingleton<ColourDatabase>
{
    [SerializeField] private List<Color> _colours;
    public List<Color> Colours => _colours;

    public Color RandomColour(Color except)
    {
        Color randomColour;

        do
        {
            int index = Random.Range(0, _colours.Count);
            randomColour = _colours[index];
        }
        while (randomColour == except);

        return randomColour;
    }
}
