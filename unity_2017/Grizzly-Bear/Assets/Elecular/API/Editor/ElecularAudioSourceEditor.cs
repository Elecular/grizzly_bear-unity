using UnityEditor;
using UnityEngine;

namespace Elecular.API
{
	[CustomEditor(typeof(ElecularAudioSource))]
	public class ElecularAudioSourceEditor : ChangeableElementEditor<ElecularAudioSource.AudioVariationConfiguration> 
	{
		
		/// <inheritdoc />
		protected override void DrawVariationConfiguration(SerializedProperty config, GameObject gameObject)
		{
			EditorGUILayout.PropertyField(config.FindPropertyRelative("audioClip"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("volume"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("pitch"));
			EditorGUILayout.PropertyField(config.FindPropertyRelative("playOnAwake"));
		}

		/// <inheritdoc />
		protected override void Initialize(SerializedProperty config, GameObject gameObject)
		{
			var audioSource = gameObject.GetComponent<AudioSource>();
			config.FindPropertyRelative("audioClip").objectReferenceValue = audioSource.clip;
			config.FindPropertyRelative("volume").floatValue = audioSource.volume;
			config.FindPropertyRelative("pitch").floatValue = audioSource.pitch;
			config.FindPropertyRelative("playOnAwake").boolValue = audioSource.playOnAwake;
		}
	}	
}
