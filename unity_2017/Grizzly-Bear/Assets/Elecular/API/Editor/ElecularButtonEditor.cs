using UnityEditor;
using UnityEngine.UI;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularButton))]
	public class ElecularButtonEditor : ChangeableElementEditor 
	{
		protected override void DrawVariationConfiguration(SerializedProperty variationConfiguration)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(
				variationConfiguration.FindPropertyRelative("variationName").stringValue,
				EditorStyles.boldLabel
			);
			EditorGUI.indentLevel++;

			//Transition
			var serializedTransition = variationConfiguration.FindPropertyRelative("transition");
			var selectedTransition = (Selectable.Transition)EditorGUILayout.EnumPopup("Transition", (Selectable.Transition)serializedTransition.enumValueIndex);
			serializedTransition.enumValueIndex = (int) selectedTransition;

			if (selectedTransition == Selectable.Transition.SpriteSwap ||
			    selectedTransition == Selectable.Transition.ColorTint)
			{
				var serializedTargetGraphic = variationConfiguration.FindPropertyRelative("sourceImage");
				EditorGUILayout.PropertyField(serializedTargetGraphic);
			}
			
			switch (selectedTransition)
			{
				case Selectable.Transition.ColorTint:
					EditorGUILayout.PropertyField(variationConfiguration.FindPropertyRelative("colorBlock"));
					break;
				case Selectable.Transition.SpriteSwap:
					EditorGUILayout.PropertyField(variationConfiguration.FindPropertyRelative("spriteState"));
					break;
				case Selectable.Transition.Animation:
					EditorGUILayout.PropertyField(variationConfiguration.FindPropertyRelative("animationTriggers"));
					break;
			}
			EditorGUI.indentLevel--;
		}
	}
}

