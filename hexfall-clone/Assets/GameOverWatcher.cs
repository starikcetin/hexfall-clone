using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using starikcetin.hexfallClone;
using UnityEngine;

public class GameOverWatcher : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.ActionDone += GameManagerOnActionDone;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.ActionDone -= GameManagerOnActionDone;
        }
    }

    private void GameManagerOnActionDone()
    {
        bool areTherePossibleMoves = AreTherePossibleMoves();

        if (false == areTherePossibleMoves)
        {
            GameOverHandler.Instance.DeclareGameOver();
        }
    }

    private bool AreTherePossibleMoves()
    {
        // To make a red explosion:
        // 1. 2 red must be connected (pair).
        // 2. A 3rd red must exist in the groups neighboring the group that contains the third hexagon of 2 red pair.

        foreach (var group in HexagonGroupDatabase.Instance.HexagonGroups)
        {
            if (HasTwoColourPair(group, out var pairSpots, out var thirdSpot, out var foundColor))
            {
                var (indirectNeighbors, directNeighbors) = FindNeighbors(thirdSpot, otherSpots: pairSpots);

                foreach (var g in indirectNeighbors)
                {
                    // indirect neighbors need at least one in order to qualify.
                    if (CountForColor(g, foundColor) > 0)
                    {
                        return true;
                    }
                }

                foreach (var g in directNeighbors)
                {
                    // direct neighbors already have one due to sharing two spots. they need two in order to qualify.
                    if (CountForColor(g, foundColor) > 1)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if there are any colours that exists two times in the <paramref name="group"/>.
    /// </summary>
    /// <param name="group">The group to check.</param>
    /// <param name="pairSpots">The spots where the color pair is located. (null if method returns false)</param>
    /// <param name="thirdSpot">The third commonSpot, i.e. the commonSpot that has a different color. (Charlie if method returns false)</param>
    /// <param name="foundColor">The colour that exists 2 times in this group. (Color.Clear if method returns false)</param>
    private bool HasTwoColourPair(HexagonGroup group, out OffsetCoordinates[] pairSpots,
        out OffsetCoordinates thirdSpot, out Color foundColor)
    {
        var (ac, bc, cc) = GetColors(group);

        if (ac == bc)
        {
            thirdSpot = group.Charlie;
            foundColor = ac;
            pairSpots = new[] {group.Alpha, group.Bravo};
            return true;
        }

        if (bc == cc)
        {
            thirdSpot = group.Alpha;
            foundColor = bc;
            pairSpots = new[] {group.Bravo, group.Charlie};
            return true;
        }

        if (ac == cc)
        {
            thirdSpot = group.Bravo;
            foundColor = ac;
            pairSpots = new[] {group.Alpha, group.Charlie};
            return true;
        }

        thirdSpot = group.Charlie;
        foundColor = Color.clear;
        pairSpots = null;
        return false;
    }

    /// <summary>
    /// Returns the groups that contains the <paramref name="commonSpot"/>. Excludes <paramref name="otherSpots"/>.
    /// Direct neighbors: neighbors that share 1 spot from <paramref name="otherSpots"/> along with the <paramref name="commonSpot"/>.
    /// Indirect neighbors: neighbors that share only the <paramref name="commonSpot"/>.
    /// </summary>
    private (List<HexagonGroup> indirectNeighbors, List<HexagonGroup> directNeighbors)
        FindNeighbors(OffsetCoordinates commonSpot, OffsetCoordinates[] otherSpots)
    {
        var indirectNeighbors = new List<HexagonGroup>();
        var directNeighbors = new List<HexagonGroup>();

        foreach (var g in HexagonGroupDatabase.Instance.HexagonGroups)
        {
            if (Contains(g, commonSpot))
            {
                var otherSpotCount = otherSpots.Count(s => Contains(g, s));

                if (otherSpotCount == 0)
                {
                    indirectNeighbors.Add(g);
                }
                else if (otherSpotCount == 1)
                {
                    directNeighbors.Add(g);
                }

                // if it contains both of the other spots, it is the group itself, not one of the neighbors.
            }
        }

        return (indirectNeighbors, directNeighbors);
    }

    /// <summary>
    /// Checks if <paramref name="hexagonGroup"/> contains <paramref name="foundColor"/>.
    /// </summary>
    private static int CountForColor(HexagonGroup hexagonGroup, Color foundColor)
    {
        int count = 0;

        var (ac, bc, cc) = GetColors(hexagonGroup);

        if (ac == foundColor) count++;
        if (bc == foundColor) count++;
        if (cc == foundColor) count++;

        return count;
    }

    private static (Color alphaColor, Color bravoColor, Color charlieColor) GetColors(HexagonGroup group)
    {
        var (a, b, c) = HexagonDatabase.Instance[group];
        var (ah, bh, ch) = (a.GetComponent<Hexagon>(), b.GetComponent<Hexagon>(), c.GetComponent<Hexagon>());
        return (ah.Color, bh.Color, ch.Color);
    }

    private static bool Contains(HexagonGroup g, OffsetCoordinates oc)
    {
        return g.Alpha == oc || g.Bravo == oc || g.Charlie == oc;
    }
}
