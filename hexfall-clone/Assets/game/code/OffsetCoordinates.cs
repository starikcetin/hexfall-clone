using System;
using UnityEngine;

namespace starikcetin.hexfallClone
{
    /// <summary>
    /// Representation of hexagons on the hexagonal odd-q offset coordinate system.
    /// Odd-Q: Odd columns are offset.
    /// Flat-Top.
    /// </summary>
    public struct OffsetCoordinates : IEquatable<OffsetCoordinates>
    {
        public int Row { get; }
        public int Col { get; }

        public OffsetCoordinates(int col, int row)
        {
            Row = row;
            Col = col;
        }

        public OffsetCoordinates WithRow(int newRow)
        {
            return new OffsetCoordinates(Col, newRow);
        }

        public OffsetCoordinates WithCol(int newCol)
        {
            return new OffsetCoordinates(newCol, Row);
        }

        public CubeCoordinates ToCube()
        {
            /*
                function oddq_to_cube(hex):
                    var x = hex.col
                    var z = hex.row - (hex.col - (hex.col&1)) / 2
                    var y = -x-z
                    return Cube(x, y, z)
            */

            var x = Col;
            var z = Row - (Col + (Col & 1)) / 2;
            var y = -x - z;
            return new CubeCoordinates(x, y, z);
        }

        public Vector2 ToUnity(float size)
        {
            // this is not due to laziness, I just want to reduce the possible points of failure (sweet lies)
            return ToCube().ToUnity(size);
        }

        public bool Equals(OffsetCoordinates other)
        {
            return Row == other.Row && Col == other.Col;
        }

        public override bool Equals(object obj)
        {
            return obj is OffsetCoordinates other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Row * 397) ^ Col;
            }
        }

        public static bool operator ==(OffsetCoordinates left, OffsetCoordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OffsetCoordinates left, OffsetCoordinates right)
        {
            return !left.Equals(right);
        }
    }
}
