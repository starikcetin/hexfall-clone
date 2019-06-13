using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LeanFingerSwipe _rightSwipeDetector, _leftSwipeDetector;

    private void Start()
    {
        _rightSwipeDetector.OnSwipe.AddListener(OnRightSwipe);
        _leftSwipeDetector.OnSwipe.AddListener(OnLeftSwipe);
    }

    private void OnRightSwipe(LeanFinger finger)
    {
        Debug.Log(nameof(InputManager) + " right swipe");
    }

    private void OnLeftSwipe(LeanFinger finger)
    {
        Debug.Log(nameof(InputManager) + " left swipe");
    }
}
