using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// Attach this component to an Audio Source to add variations
	/// </summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AudioSource))]
	public class ElecularAudioSource : ChangeableElement
	{
		[SerializeField]
		[HideInInspector]
		private List<AudioVariationConfiguration> variations;

		protected override void OnAwake()
		{
			var audioSource = GetComponent<AudioSource>();
			//We stop the audio from playing now because will will play it after the setup is complete
			if(audioSource.playOnAwake)
				audioSource.Stop();
		}

		/// <inheritdoc />
		public override IEnumerable<VariationConfiguration> Configurations
		{
			get { return variations.Cast<VariationConfiguration>(); }
		}
		
		/// <summary>
		/// Defines how each Audio Source variation behaves
		/// </summary>
		[Serializable]
		public class AudioVariationConfiguration : VariationConfiguration
		{
			[SerializeField]
			private AudioClip audioClip;
			
			[SerializeField]
			private float volume;
			
			[SerializeField]
			private float pitch;
			
			[SerializeField] 
			private bool playOnAwake;
			
			[NonSerialized]
			private bool played;
			
			/// <summary>
			/// Initialises all values
			/// </summary>
			/// <param name="audioClip"></param>
			/// <param name="volume"></param>
			/// <param name="pitch"></param>
			/// <param name="playOnAwake"></param>
			public void Init(AudioClip audioClip, float volume, float pitch, bool playOnAwake)
			{
				this.audioClip = audioClip;
				this.volume = volume;
				this.pitch = pitch;
				this.playOnAwake = playOnAwake;
			}

			/// <inheritdoc />
			public override Component GetTarget(GameObject gameObject)
			{
				return gameObject.GetComponent<AudioSource>();
			}

			/// <inheritdoc />
			public override void DisableTarget(GameObject gameObject)
			{
				
			}

			/// <inheritdoc />
			public override void EnableTarget(GameObject gameObject)
			{
				
			}

			/// <inheritdoc />
			public override void SetupTarget(GameObject gameObject)
			{
				var audioSource = gameObject.GetComponent<AudioSource>();
				audioSource.clip = audioClip;
				audioSource.pitch = pitch;
				audioSource.volume = volume;
				audioSource.playOnAwake = playOnAwake;
				if (!played && playOnAwake)
				{
					audioSource.Play();
					if (Application.isPlaying)
					{
						played = true;	
					}
				}
			}
		}
	}	
}
