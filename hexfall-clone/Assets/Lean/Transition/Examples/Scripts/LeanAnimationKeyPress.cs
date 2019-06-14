using UnityEngine;
using Lean.Transition; // Make sure you add this if you want to use Lean Transition!

// We put this example code in a namespace, so it doesn't clutter the project up
namespace Lean.Transition.Examples
{
	// Create a new component called Repeater
	[HelpURL(LeanTransition.HelpUrlPrefix + "LeanAnimationKeyPress")]
	[AddComponentMenu(LeanTransition.ComponentMenuPrefix + "Lean Animation Key Press")]
	public class LeanAnimationKeyPress : LeanAnimation
	{
		// The key that must be pressed
		public KeyCode RequiredKey;

		// Update is automatically called every game loop
		void Update()
		{
			// Required key was pressed down?
			if (Input.GetKeyDown(RequiredKey) == true)
			{
				// Begin transitions from LeanAnimation
				BeginTransitions();
			}
		}
	}
}