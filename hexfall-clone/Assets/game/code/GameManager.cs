using System;
using System.Collections;
using System.Collections.Generic;
using starikcetin.hexfallClone;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private HexagonGroup _selectedGroup;

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
    }

    private void InputManagerOnSwiped(SwipeDirection swipeDirection)
    {
        Debug.Log($"{nameof(GameManager)}: {nameof(InputManagerOnSwiped)}({nameof(swipeDirection)}: {swipeDirection})");
    }
}
