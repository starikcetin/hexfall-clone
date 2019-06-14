namespace Lean.Transition
{
	/// <summary>This allows you to control where in the game loop animations will update.</summary>
	public enum LeanTime
	{
		UnscaledFixedUpdate = -3,
		UnscaledLateUpdate  = -2,
		UnscaledUpdate      = -1,
		Default             =  0,
		Update              =  1,
		LateUpdate          =  2,
		FixedUpdate         =  3
	}
}