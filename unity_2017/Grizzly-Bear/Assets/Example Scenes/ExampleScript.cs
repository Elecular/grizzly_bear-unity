using System.Collections;
using System.Collections.Generic;
using Elecular.API;
using UnityEngine;

namespace Example_Scenes
{
	public class ExampleScript : MonoBehaviour
	{
		[SerializeField]
		private Experiment experiment;
		
		public void MakeRequest()
		{
			experiment.GetVariation(variation =>
			{
				Debug.Log(variation.Settings);
			});
			ElecularApi.Instance.GetAllVariations("Experiment 1", variations =>
			{
				Debug.Log(variations);
			});
			ElecularApi.Instance.GetSetting("Experiment 1", "Button Color", value =>
			{
				Debug.Log(value);
			});
		}
	}
	
}
