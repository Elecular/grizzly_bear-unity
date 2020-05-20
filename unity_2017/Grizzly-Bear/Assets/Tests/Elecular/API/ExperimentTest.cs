using System.Collections;
using Elecular.API;
using Elecular.Core;
using NUnit.Framework;
using Tests.Elecular.Core;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Elecular.API
{
	public class ExperimentTest 
	{
		[UnityTest]
		public IEnumerator CanGetVariationOfExperiment() 
		{
			Request.SetMockRequest(new MockRequest(
			mockResponse: @"{
				""variationName"": ""Variation 1"",
				""variations"": ""[]""
			}"
			));
			var experiment = ScriptableObject.CreateInstance<Experiment>();
			experiment.GetVariation(variation => 
			{
				Debug.Log(variation.Name);	
			});
			yield return null;
		}
	}
}
