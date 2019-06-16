using System;
using System.Collections.Generic;
using System.Linq;
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
        GameManager.Instance.ActionDone -= GameManagerOnActionDone;
    }

    private void GameManagerOnActionDone()
    {
        var areTherePossibleMoves = AreTherePossibleMoves();

        if (areTherePossibleMoves == false)
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
            // skip if this group doesn't have a two color group.
            if (false == HasTwoColourPair(group, out var thirdSpot, out var foundColor))
            {
                continue;
            }

            // check the neighbor groups containing the third spot.
            var neighbors = GetGroupsContainingSpot(thirdSpot, except: group);

            if (neighbors.Any(g => ContainsColor(g, foundColor)))
            {
                // we got a move! :)
                return true;
            }
        }

        // we could not find any possible moves :(
        return false;
    }

    /// <summary>
    /// Checks if there are any colours that exists two times in the <paramref name="group"/>.
    /// </summary>
    /// <param name="group">The group to check.</param>
    /// <param name="thirdSpot">The third spot, i.e. the spot that has a different color. (Charlie if method returns false)</param>
    /// <param name="foundColor">The colour that exists 2 times in this group. (Color.Clear if method returns false)</param>
    private bool HasTwoColourPair(HexagonGroup group, out OffsetCoordinates thirdSpot, out Color foundColor)
    {
        var (ac, bc, cc) = GetColors(@group);

        if (ac == bc)
        {
            thirdSpot = group.Charlie;
            foundColor = ac;
            return true;
        }

        if (bc == cc)
        {
            thirdSpot = group.Alpha;
            foundColor = bc;
            return true;
        }

        if (ac == cc)
        {
            thirdSpot = group.Alpha;
            foundColor = ac;
            return true;
        }

        thirdSpot = group.Charlie;
        foundColor = Color.clear;
        return false;
    }

    /// <summary>
    /// Returns the groups that contains the <paramref name="spot"/>. Excludes <paramref name="except"/>.
    /// </summary>
    private IEnumerable<HexagonGroup> GetGroupsContainingSpot(OffsetCoordinates spot, HexagonGroup except)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if <paramref name="hexagonGroup"/> contains <paramref name="foundColor"/>.
    /// </summary>
    private bool ContainsColor(HexagonGroup hexagonGroup, Color foundColor)
    {
        var (ac, bc, cc) = GetColors(hexagonGroup);
        
        return ac == foundColor || bc == foundColor || cc == foundColor;
    }

    private static (Color alphaColor, Color bravoColor, Color charlieColor) GetColors(HexagonGroup group)
    {
        var (a, b, c) = HexagonDatabase.Instance[group];
        var (ah, bh, ch) = (a.GetComponent<Hexagon>(), b.GetComponent<Hexagon>(), c.GetComponent<Hexagon>());
        return (ah.Color, bh.Color, ch.Color);
    }
}
