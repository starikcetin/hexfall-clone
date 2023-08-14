using System.Diagnostics.Contracts;
using starikcetin.hexfallClone.game.databases;
using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    /// <summary>
    /// Representation of hexagons on the hexagonal cube coordinate system.
    /// Values are integers.
    /// Always: (Q + S + R = 0).
    /// Flat-Top.
    /// </summary>
    public struct CubeCoordinates
    {
        private readonly Vector3Int _values;

        public int Q => _values.x;
        public int S => _values.y;
        public int R => _values.z;

        public int X => Q;
        public int Y => S;
        public int Z => R;

        /// <param name="q">x</param>
        /// <param name="s">y</param>
        /// <param name="r">z</param>
        public CubeCoordinates(int q, int s, int r)
        {
            _values = new Vector3Int(q, s, r);
        }

        /// <summary>
        /// q == x
        /// </summary>
        /// <param name="newQ">x</param>
        public CubeCoordinates WithQ(int newQ)
        {
            return new CubeCoordinates(newQ, S, R);
        }

        /// <summary>
        /// s == y
        /// </summary>
        /// <param name="newS">y</param>
        public CubeCoordinates WithS(int newS)
        {
            return new CubeCoordinates(Q, newS, R);
        }

        /// <summary>
        /// r == z
        /// </summary>
        /// <param name="newR">z</param>
        public CubeCoordinates WithR(int newR)
        {
            return new CubeCoordinates(Q, S, newR);
        }

        public OffsetCoordinates ToOffset()
        {
            /*
                function cube_to_oddq(cube):
                    var col = cube.x
                    var row = cube.z + (cube.x - (cube.x&1)) / 2
                    return OffsetCoord(col, row)
             */

            var col = X;
            var row = Z + (X - (X & 1)) / 2;
            return new OffsetCoordinates(col, row);
        }

        public Vector2 ToUnity()
        {
            var size = GameParamsDatabase.Instance.Size;

            /*
                function flat_hex_to_pixel(hex):
                    var x = size * (     3./2 * hex.q                    )
                    var y = size * (sqrt(3)/2 * hex.q  +  sqrt(3) * hex.r)
                    return Point(x, y)
             */

            var x = size * (3f / 2 * Q);
            var y = size * (Mathf.Sqrt(3) / 2 * Q + Mathf.Sqrt(3) * R);
            var result = new Vector2(x, y) - GameParamsDatabase.Instance.CenterOffset;
            return GameParamsDatabase.Instance.GameAreaScaleMatrix.MultiplyPoint3x4(result);
        }

        [ContractInvariantMethod]
        private void _ContractInvariant()
        {
            Contract.Assert(Q + S + R == 0);
        }
    }
}
