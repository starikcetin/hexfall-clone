using System;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LeanFingerSwipe _rightSwipeDetector, _leftSwipeDetector;
    [SerializeField] private LeanFingerTap _tapDetector;

    private void Start()
    {
        _rightSwipeDetector.OnSwipe.AddListener(OnRightSwipe);
        _leftSwipeDetector.OnSwipe.AddListener(OnLeftSwipe);
        _tapDetector.OnTap.AddListener(OnTap);
    }

    private void OnRightSwipe(LeanFinger finger)
    {
        Debug.Log(nameof(InputManager) + " right swipe");
    }

    private void OnLeftSwipe(LeanFinger finger)
    {
        Debug.Log(nameof(InputManager) + " left swipe");
    }

    private void OnTap(LeanFinger finger)
    {
        var screenPos = finger.ScreenPosition;
        var worldPos = finger.GetWorldPosition(10, Camera.current);

        Debug.Log($"{nameof(InputManager)} + tap " +
                  $"| {nameof(screenPos)} = {screenPos} " +
                  $"| {nameof(worldPos)} = {worldPos}");
    }
}
