﻿using System;
using Eflatun.UnityCommon.Inspector;
using Lean.Touch;
using UnityEngine;

namespace starikcetin.hexfallClone.game
{
    public class InputManager : MonoBehaviour
    {
        public event Action<SwipeDirection> Swiped;

        /// <summary>
        /// parameter is the world position of the tap.
        /// </summary>
        public event Action<Vector3> Tapped;

        [SerializeField] private LeanFingerSwipe _rightSwipeDetector, _leftSwipeDetector;
        [SerializeField] private LeanFingerTap _tapDetector;

        [SerializeField] private LayerWrapper _selectableLayer;

        private void Start()
        {
            _rightSwipeDetector.OnSwipe.AddListener(OnRightSwipe);
            _leftSwipeDetector.OnSwipe.AddListener(OnLeftSwipe);
            _tapDetector.OnTap.AddListener(OnTap);
        }

        private void OnDestroy()
        {
            _rightSwipeDetector.OnSwipe.RemoveListener(OnRightSwipe);
            _leftSwipeDetector.OnSwipe.RemoveListener(OnLeftSwipe);
            _tapDetector.OnTap.RemoveListener(OnTap);
        }

        private void OnRightSwipe(LeanFinger finger)
        {
            Utils.LogConditional(nameof(InputManager) + " right swipe");

            Swiped?.Invoke(SwipeDirection.Right);
        }

        private void OnLeftSwipe(LeanFinger finger)
        {
            Utils.LogConditional(nameof(InputManager) + " left swipe");

            Swiped?.Invoke(SwipeDirection.Left);
        }

        private void OnTap(LeanFinger finger)
        {
            var screenPos = finger.ScreenPosition;
            var worldPos = finger.GetWorldPosition(10, Camera.current);

            Utils.LogConditional($"{nameof(InputManager)} + tap " +
                                 $"| {nameof(screenPos)} = {screenPos} " +
                                 $"| {nameof(worldPos)} = {worldPos}");

            if (Physics2D.OverlapPoint(worldPos, _selectableLayer.AsMask))
            {
                Tapped?.Invoke(worldPos);
            }
        }
    }
}
