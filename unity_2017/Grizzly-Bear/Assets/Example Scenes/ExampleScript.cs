using System.Collections;
using System.Collections.Generic;
using Elecular.Core;
using UnityEngine;

namespace Example_Scenes
{
	public class ExampleScript : MonoBehaviour
	{

		public void MakeRequest()
		{
			var api = new ExperimentsApi();
			api.GetVariation("test", "5ec39b16750f8d0012e5c027", "Experiment 1", variation =>
			{
				Debug.Log(variation.variationName);
			});
		}
	}
	
}
