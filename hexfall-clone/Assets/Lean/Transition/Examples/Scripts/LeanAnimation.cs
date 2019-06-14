using UnityEngine;
using Lean.Transition; // Make sure you add this if you want to use Lean Transition!

// We put this example code in a namespace, so it doesn't clutter the project up
namespace Lean.Transition.Examples
{
	// Create a new component called LeanAnimation
	[HelpURL(LeanTransition.HelpUrlPrefix + "LeanAnimation")]
	[AddComponentMenu(LeanTransition.ComponentMenuPrefix + "Lean Animation")]
	public class LeanAnimation : MonoBehaviour
	{
		// This stores the transition data
		public LeanPlayer Transitions;

		[ContextMenu("Begin Transitions")]
		public void BeginTransitions()
		{
			// Make sure the transitions exist
			if (Transitions != null)
			{
				// Begin them
				Transitions.Begin();
			}
		}
	}
}