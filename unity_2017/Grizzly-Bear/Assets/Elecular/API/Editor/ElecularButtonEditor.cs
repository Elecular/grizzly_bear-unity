using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularButton))]
	public class ElecularButtonEditor : ChangeableElementEditor<ElecularButton.ButtonVariationConfiguration>
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
		
		/// <inheritdoc />
		protected override void Initialize(SerializedProperty variationConfiguration)
		{
			var serializedColorBlock = variationConfiguration.FindPropertyRelative("colorBlock");
			serializedColorBlock.FindPropertyRelative("m_NormalColor").colorValue = Color.white;
			serializedColorBlock.FindPropertyRelative("m_HighlightedColor").colorValue = Color.white;
			serializedColorBlock.FindPropertyRelative("m_PressedColor").colorValue = Color.white;
			serializedColorBlock.FindPropertyRelative("m_DisabledColor").colorValue = Color.white;
			serializedColorBlock.FindPropertyRelative("m_ColorMultiplier").floatValue = 1f;
			serializedColorBlock.FindPropertyRelative("m_FadeDuration").floatValue = 0.1f;
		}
	}
}

