﻿using UnityEngine;
using System.Collections.Generic;

namespace Lean.Transition.Method
{
	/// <summary>This component allows you to play a sound after the specified duration.</summary>
	[HelpURL(LeanTransition.HelpUrlPrefix + "LeanPlaySound")]
	[AddComponentMenu(LeanTransition.MethodsMenuPrefix + "Play Sound" + LeanTransition.MethodsMenuSuffix)]
	public class LeanPlaySound : LeanMethodWithStateAndTarget
	{
		public override System.Type GetTargetType()
		{
			return typeof(AudioClip);
		}

		public override void Register()
		{
			PreviousState = Register(GetAliasedTarget(Data.Target), Data.Duration, Data.Volume);
		}

		public static LeanState Register(AudioClip target, float duration, float volume = 1.0f)
		{
			var data = LeanTransition.RegisterWithTarget(State.Pool, duration, target);

			data.Volume = volume;

			return data;
		}

		[System.Serializable]
		public class State : LeanStateWithTarget<AudioClip>
		{
			[Range(0.0f, 1.0f)]
			public float Volume = 1.0f;

			public override void AutoFillWithTarget()
			{
				// No object to fill with data
			}

			public override void BeginWithTarget()
			{
				// No state to begin from
			}

			public override void UpdateWithTarget(float progress)
			{
				if (progress == 1.0f)
				{
#if UNITY_EDITOR
					if (Application.isPlaying == false)
					{
						return;
					}
#endif
					var gameObject  = new GameObject(Target.name);
					var audioSource = gameObject.AddComponent<AudioSource>();

					audioSource.clip   = Target;
					audioSource.volume = Volume;

					audioSource.Play();

					Destroy(gameObject, Target.length);
				}
			}

			public static Stack<State> Pool = new Stack<State>(); public override void Despawn() { Pool.Push(this); }
		}

		public State Data;
	}
}