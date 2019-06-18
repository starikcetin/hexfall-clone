using UnityEngine;

public class InputParameterSetter : MonoBehaviour
{
    private void Start()
    {
        Input.backButtonLeavesApp = true;
        Input.multiTouchEnabled = false;
    }
}
