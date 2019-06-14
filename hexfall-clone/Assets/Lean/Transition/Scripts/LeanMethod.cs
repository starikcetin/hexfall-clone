﻿using UnityEngine;
using Lean.Common;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

namespace Lean.Transition
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanMethod), true)]
	public class LeanMethod_Inspector : LeanInspector<LeanMethod>
	{
		private bool expandAlias;

		private static List<LeanMethod> tempMethods = new List<LeanMethod>();

		private int Order(LeanMethod a)
		{
			a.GetComponents(tempMethods); return tempMethods.IndexOf(a);
		}

		private void DrawTargetAlias()
		{
			var sTarget = serializedObject.FindProperty("Data.Target");
			var sAlias  = serializedObject.FindProperty("Alias");

			if (sTarget != null && sAlias != null)
			{
				EditorGUILayout.Separator();

				var rect  = Reserve();
				var rectW = (rect.width - EditorGUIUtility.labelWidth) * 0.5f;
				var rectL = rect;
				var rectR = rect; rectR.xMin = rectR.xMax - rectW;
				var label = new GUIContent("Target", "This is the target of the transition. For most transition methods this will be the component that will be modified.");
				
				if (string.IsNullOrEmpty(sAlias.stringValue) == false)
				{
					expandAlias = true;
				}

				if (string.IsNullOrEmpty(sAlias.stringValue) == true)
				{
					expandAlias = EditorGUI.Foldout(new Rect(rect.position, new Vector2(25.0f, rect.height)), expandAlias, string.Empty);
				}

				if (expandAlias == true)
				{
					label.text += " : Alias"; rectL.xMax -= rectW;
				}

				EditorGUI.LabelField(rect, label);

				BeginError(sTarget.objectReferenceValue == null && string.IsNullOrEmpty(sAlias.stringValue) == true);
					EditorGUI.PropertyField(rectL, sTarget);
					if (expandAlias == true)
					{
						EditorGUI.PropertyField(rectR, sAlias, GUIContent.none);
					}
				EndError();

				if (string.IsNullOrEmpty(sAlias.stringValue) == false)
				{
					var methodST      = (LeanMethodWithStateAndTarget)Target;
					var expectedType  = methodST.GetTargetType();
					var expectedAlias = sAlias.stringValue;

					foreach (var method in Target.GetComponents<LeanMethodWithStateAndTarget>())
					{
						var methodTargetType = method.GetTargetType();

						if (methodTargetType != expectedType)
						{
							if (method.Alias == expectedAlias)
							{
								if (methodTargetType.IsSubclassOf(typeof(Component)) == true && expectedType.IsSubclassOf(typeof(Component)) == true)
								{
									continue;
								}

								EditorGUILayout.HelpBox("This alias is used by multiple transitions. This only works if they all transition the same type (e.g. Transform.localPosition & Transform.localScale both transition Transform).", MessageType.Error);

								break;
							}
						}
					}
				}
			}
		}

		protected override void DrawInspector()
		{
			var dataProperty = serializedObject.FindProperty("Data");

			if (dataProperty != null)
			{
				Draw("Data.Duration", "The transition will complete after this many seconds.");

				DrawTargetAlias();

				dataProperty.NextVisible(true);

				while (true)
				{
					if (dataProperty.name != "Duration" && dataProperty.name != "Target" && dataProperty.name != "TargetAlias")
					{
						EditorGUILayout.PropertyField(dataProperty);
					}

					if (dataProperty.NextVisible(false) == false)
					{
						break;
					}
				}
			}
			else
			{
				var property = serializedObject.GetIterator(); property.NextVisible(true);

				while (property.NextVisible(false) == true)
				{
					if (property.name == "Target")
					{
						BeginError(property.objectReferenceValue == null);
							EditorGUILayout.PropertyField(property);
						EndError();
					}
					else
					{
						EditorGUILayout.PropertyField(property);
					}
				}
			}
		}
	}
}
#endif

namespace Lean.Transition
{
	/// <summary>This is the base class for all transition methods.</summary>
	public abstract class LeanMethod : MonoBehaviour
	{
		public abstract void Register();

		[ContextMenu("Begin This Transition")]
		public void BeginThisTransition()
		{
			LeanTransition.RequireSubmitted();

			LeanTransition.CurrentAliases.Clear();

			Register();

			LeanTransition.Submit();
		}

		[ContextMenu("Begin All Transitions")]
		public void BeginAllTransitions()
		{
			LeanTransition.CurrentAliases.Clear();

			LeanTransition.BeginAllTransitions(transform);
		}

		/// <summary>This will take the input linear 0..1 value, and return a transformed version based on the specified easing function.</summary>
		public static float Smooth(LeanEase ease, float progress)
		{
			switch (ease)
			{
				case LeanEase.Smooth:
				{
					progress = progress * progress * (3.0f - 2.0f * progress);
				}
				break;

				case LeanEase.Accelerate:
				{
					progress *= progress;
				}
				break;

				case LeanEase.Decelerate:
				{
					progress = 1.0f - progress;
					progress *= progress;
					progress = 1.0f - progress;
				}
				break;

				case LeanEase.Elastic:
				{
					var angle   = progress * Mathf.PI * 4.0f;
					var weightA = 1.0f - Mathf.Pow(progress, 0.125f);
					var weightB = 1.0f - Mathf.Pow(1.0f - progress, 8.0f);

					progress = Mathf.LerpUnclamped(0.0f, 1.0f - Mathf.Cos(angle) * weightA, weightB);
				}
				break;

				case LeanEase.Back:
				{
					progress = 1.0f - progress;
					progress = progress * progress * progress - progress * Mathf.Sin(progress * Mathf.PI);
					progress = 1.0f - progress;
				}
				break;

				case LeanEase.Bounce:
				{
					if (progress < (4f/11f))
					{
						progress = (121f/16f) * progress * progress;
					}
					else if (progress < (8f/11f))
					{
						progress = (121f/16f) * (progress - (6f/11f)) * (progress - (6f/11f)) + 0.75f;
					}
					else if (progress < (10f/11f))
					{
						progress = (121f/16f) * (progress - (9f/11f)) * (progress - (9f/11f)) + (15f/16f);
					}
					else
					{
						progress = (121f/16f) * (progress - (21f/22f)) * (progress - (21f/22f)) + (63f/64f);
					}
				}
				break;
			}

			return progress;
		}
	}

	public abstract class LeanMethodWithState : LeanMethod
	{
		/// <summary>Each time this transition method registers a new state, it will be stored here.</summary>
		public LeanState PreviousState;
	}

	public abstract class LeanMethodWithStateAndTarget : LeanMethodWithState
	{
		public abstract System.Type GetTargetType();

		[UnityEngine.Serialization.FormerlySerializedAs("Data.TargetAlias")]
		public string Alias;

		/// <summary>This allows you to get the current Target value, or an alised override.</summary>
		public T GetAliasedTarget<T>(T current)
			where T : Object
		{
			if (string.IsNullOrEmpty(Alias) == false)
			{
				var target = default(Object);

				if (LeanTransition.CurrentAliases.TryGetValue(Alias, out target) == true)
				{
					if (target is T)
					{
						return (T)target;
					}
					else if (target is GameObject)
					{
						var gameObject = (GameObject)target;

						return gameObject.GetComponent(typeof(T)) as T;
					}
				}
			}

			return current;
		}
	}
}