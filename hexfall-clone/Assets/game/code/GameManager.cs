using System;
using System.Collections;
using System.Collections.Generic;
using starikcetin.hexfallClone;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _isSelectionActive = false;

    private HexagonGroup _selectedGroup;
    private GameObject _highlightGameObject;

    private void Start()
    {
        var inputManager = GetComponentInChildren<InputManager>();
        inputManager.Swiped += InputManagerOnSwiped;
        inputManager.Tapped += InputManagerOnTapped;
    }

    private void InputManagerOnTapped(Vector3 worldPosition)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(InputManagerOnTapped)}({nameof(worldPosition)}: {worldPosition})");

        var closestGroup = HexagonGroupDatabase.Instance.FindClosestGroup(worldPosition, GameParamsDatabase.Instance.Size);
        Debug.Log("Center of closest group: " + closestGroup.Center);

        SelectGroup(closestGroup);
    }

    private void SelectGroup(HexagonGroup group)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(SelectGroup)}({nameof(group)}: {group.Center})");
        _selectedGroup = group;
        _isSelectionActive = true;

        Destroy(_highlightGameObject);
        _highlightGameObject = Utils._Debug_Highlight((Vector3)_selectedGroup.Center - new Vector3(0,0,1), Color.green);
    }

    private void InputManagerOnSwiped(SwipeDirection swipeDirection)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(InputManagerOnSwiped)}({nameof(swipeDirection)}: {swipeDirection})");

        if (!_isSelectionActive)
        {
            Debug.Log($"{nameof(GameManager)}.{nameof(InputManagerOnSwiped)}: no active selection, ignoring swipe.");
            return;
        }

        if (swipeDirection == SwipeDirection.Right)
        {
            RotateClockwise(_selectedGroup);
        }
        else
        {
            RotateCounterClockwise(_selectedGroup);
        }
    }

    private void RotateClockwise(HexagonGroup selectedGroup)
    {
        var alphaHex = HexagonDatabase.Instance[selectedGroup.Alpha];
        var bravoHex = HexagonDatabase.Instance[selectedGroup.Bravo];
        var charlieHex = HexagonDatabase.Instance[selectedGroup.Charlie];

        // Alpha Hex --> Bravo
        Put(alphaHex, selectedGroup.Bravo);

        // Bravo Hex --> Charlie
        Put(bravoHex, selectedGroup.Charlie);

        // Charlie Hex --> Alpha
        Put(charlieHex, selectedGroup.Alpha);
    }

    private void RotateCounterClockwise(HexagonGroup selectedGroup)
    {
        var alphaHex = HexagonDatabase.Instance[selectedGroup.Alpha];
        var bravoHex = HexagonDatabase.Instance[selectedGroup.Bravo];
        var charlieHex = HexagonDatabase.Instance[selectedGroup.Charlie];

        // Alpha Hex --> Charlie
        Put(alphaHex, selectedGroup.Charlie);

        // Charlie Hex --> Bravo
        Put(charlieHex, selectedGroup.Bravo);

        // Bravo Hex --> Alpha
        Put(bravoHex, selectedGroup.Alpha);
    }

    private void Put(GameObject hex, OffsetCoordinates coords)
    {
        // set in hexagon database
        HexagonDatabase.Instance[coords] = hex;

        // sync the position of the GameObject TODO: we might move this to a Hexagon class.
        hex.transform.position = coords.ToUnity(GameParamsDatabase.Instance.Size);
    }
}
