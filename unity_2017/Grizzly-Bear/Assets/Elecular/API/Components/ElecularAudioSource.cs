using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	/// <summary>
	/// Attach this component to an Audio Source to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AudioSource))]
	public class ElecularAudioSource : ChangeableElement<ElecularAudioSource.AudioVariationConfiguration>
	{
		[SerializeField]
		[HideInInspector]
		private List<AudioVariationConfiguration> variations;

		[NonSerialized]
		private bool played = false;

		protected override void OnAwake()
		{
			var audioSource = GetComponent<AudioSource>();
			//We stop the audio from playing now because will will play it after the setup is complete
			if(audioSource.playOnAwake)
				audioSource.Stop();
		}

		/// <inheritdoc />
		protected override void Setup(AudioVariationConfiguration variationConfiguration)
		{
			var audioSource = GetComponent<AudioSource>();
			audioSource.clip = variationConfiguration.audioClip;
			audioSource.pitch = variationConfiguration.pitch;
			audioSource.volume = variationConfiguration.volume;
			audioSource.playOnAwake = variationConfiguration.playOnAwake;
			if (!played && variationConfiguration.playOnAwake)
			{
				audioSource.Play();
				if (Application.isPlaying)
				{
					played = true;	
				}
			}
		}

		/// <inheritdoc />
		public override IEnumerable<AudioVariationConfiguration> Configurations
		{
			get { return variations; }
		}
		
		/// <summary>
		/// Defines how each Audio Source variation behaves
		/// </summary>
		[Serializable]
		public class AudioVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			public AudioClip audioClip;
			
			[SerializeField]
			public float volume;
			
			[SerializeField]
			public float pitch;
			
			[SerializeField] 
			public bool playOnAwake;

			/// <inheritdoc />
			public override Object GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<AudioSource>();
			}
		}
	}	
}
