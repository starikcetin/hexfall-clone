using System;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    public class HexagonGridBuilder : MonoBehaviour
    {
        [SerializeField] private int _columnCount, _rowCount;
        [SerializeField] private float _size;

        private void Start()
        {
            BuildHexagonGrid(_size);
        }

        private void BuildHexagonGrid(float size)
        {
            for (var col = 0; col < _columnCount; col++)
            {
                for (var row = 0; row < _rowCount; row++)
                {
                    var offsetCoordinates = new OffsetCoordinates(col, row);

                    var newHexagon = Instantiate(PrefabDatabase.Instance.Hexagon, transform);
                    newHexagon.transform.position = offsetCoordinates.ToUnity(size);
                }
            }
        }
    }
}
