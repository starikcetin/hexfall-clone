using UnityEngine;

namespace starikcetin.hexfallClone.game.input
{
    public class InputParameterSetter : MonoBehaviour
    {
        private void Start()
        {
            Input.backButtonLeavesApp = true;
            Input.multiTouchEnabled = false;
        }
    }
}
