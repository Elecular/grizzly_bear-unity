using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularButton))]
	public class ElecularButtonEditor : ChangeableElementEditor<ElecularButton.ButtonVariationConfiguration>
	{
		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty variationConfiguration, GameObject gameObject)
		{
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
		}
		
		/// <inheritdoc />
		protected override void Initialize(SerializedProperty variationConfiguration, GameObject gameObject)
		{
			var button = gameObject.GetComponent<Button>();
			variationConfiguration.FindPropertyRelative("transition").enumValueIndex = (int) button.transition;
			if (button.image != null)
			{
				variationConfiguration.FindPropertyRelative("sourceImage").objectReferenceValue = button.image.sprite;
			}

			var serializedColorBlock = variationConfiguration.FindPropertyRelative("colorBlock");
			var serializedSpriteState = variationConfiguration.FindPropertyRelative("spriteState");
			var serializedAnimationTriggers = variationConfiguration.FindPropertyRelative("animationTriggers");

			serializedColorBlock.FindPropertyRelative("m_NormalColor").colorValue = button.colors.normalColor;
			serializedColorBlock.FindPropertyRelative("m_HighlightedColor").colorValue = button.colors.highlightedColor;
			serializedColorBlock.FindPropertyRelative("m_PressedColor").colorValue = button.colors.pressedColor;
			serializedColorBlock.FindPropertyRelative("m_DisabledColor").colorValue = button.colors.disabledColor;
			serializedColorBlock.FindPropertyRelative("m_ColorMultiplier").floatValue = button.colors.colorMultiplier;
			serializedColorBlock.FindPropertyRelative("m_FadeDuration").floatValue = button.colors.fadeDuration;

			serializedSpriteState.FindPropertyRelative("m_HighlightedSprite").objectReferenceValue = button.spriteState.highlightedSprite;
			serializedSpriteState.FindPropertyRelative("m_PressedSprite").objectReferenceValue = button.spriteState.pressedSprite;
			serializedSpriteState.FindPropertyRelative("m_DisabledSprite").objectReferenceValue = button.spriteState.disabledSprite;

			serializedAnimationTriggers.FindPropertyRelative("m_NormalTrigger").stringValue = button.animationTriggers.normalTrigger;
			serializedAnimationTriggers.FindPropertyRelative("m_HighlightedTrigger").stringValue = button.animationTriggers.highlightedTrigger;
			serializedAnimationTriggers.FindPropertyRelative("m_PressedTrigger").stringValue = button.animationTriggers.pressedTrigger;
			serializedAnimationTriggers.FindPropertyRelative("m_DisabledTrigger").stringValue = button.animationTriggers.disabledTrigger;
		}
		
		
	}
}

