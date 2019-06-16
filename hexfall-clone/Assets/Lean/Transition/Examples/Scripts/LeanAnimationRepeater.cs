using UnityEngine;
using Lean.Transition; // Make sure you add this if you want to use Lean Transition!

// We put this example code in a namespace, so it doesn't clutter the project up
namespace Lean.Transition.Examples
{
	// Create a new component called Repeater
	[HelpURL(LeanTransition.HelpUrlPrefix + "LeanAnimationRepeater")]
	[AddComponentMenu(LeanTransition.ComponentMenuPrefix + "Lean Animation Repeater")]
	public class LeanAnimationRepeater : LeanAnimation
	{
		// When this reaches 0, the transitions will begin
		public float RemainingTime = 1.0f;

		// When RemainingTime reaches 0, it will be reset to TimeInterval
		public float TimeInterval = 3.0f;

		// Update is automatically called every game loop
		void Update()
		{
			// Decrease time
			RemainingTime -= Time.deltaTime;

			// Ready to repeat?
			if (RemainingTime <= 0.0f)
			{
				// Reset time
				RemainingTime = TimeInterval;

				// Begin transitions from LeanAnimation
				BeginTransitions();
			}
		}
	}
}