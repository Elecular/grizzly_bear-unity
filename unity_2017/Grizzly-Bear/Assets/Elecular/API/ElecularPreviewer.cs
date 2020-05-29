using System;
using Elecular.API;
using UnityEngine;

namespace Elecular.api
{
	/// <summary>
	/// This is used for previewing different variations of the experiment on a single game object
	/// </summary>
	public class ElecularPreviewer : MonoBehaviour
	{
		[SerializeField]
		private Experiment experiment;

		private void Awake()
		{
			//This component is only meant to be used in editor. Hence we will destroy it when the game starts playing
			Destroy(this);
		}

		/// <summary>
		/// Returns the experiment that is going to be previewed
		/// </summary>
		public Experiment Experiment
		{
			get { return experiment; }
			set { experiment = value; }
		}

		private void Reset()
		{
			foreach (var element in GetComponents<ChangeableElement>())
			{
				if (element.Experiment != null)
				{
					experiment = element.Experiment;
					break;
				}
			}
		}
	}	
}

