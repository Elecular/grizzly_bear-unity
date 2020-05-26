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
		[Test]
		public void CanGetVariationOfExperiment()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1""
				}"
			);
			Request.SetMockRequest(mockRequest);

			var experiment = Resources.Load<Experiment>("Elecular/Tests/Experiment 1");
			experiment.GetVariation(variation => 
			{
				Assert.AreEqual(
					variation.Name,
					"Variation 1"
				);	
			});
			
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=" + SystemInfo.deviceUniqueIdentifier
			);
		}
		
		[Test]
		public void CannotGetVariationWithInvalidExperiment()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(null);
			Request.SetMockRequest(mockRequest);

			var experiment = Resources.Load<Experiment>("Elecular/Tests/Experiment 1");
			var error = false;
			experiment.GetVariation(variation => 
			{
				Assert.Fail();
			}, () =>
			{
				error = true;
			});
			
			Assert.True(error);
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=" + SystemInfo.deviceUniqueIdentifier
			);
		}
		
		[Test]
		public void CannotGetSettingWithInvalidExperiment()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(null);
			Request.SetMockRequest(mockRequest);

			var experiment = Resources.Load<Experiment>("Elecular/Tests/Experiment 1");
			var error = false;
			experiment.GetSetting("Button Color", variation => 
			{
				Assert.Fail();
			}, () =>
			{
				error = true;
			});
			
			Assert.True(error);
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=" + SystemInfo.deviceUniqueIdentifier
			);
		}

		[Test]
		public void CanGetSettingOfExperiment()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1"",
					""variables"": [{
						""variableName"": ""setting 1"",
						""variableValue"": ""blue""
					}, {
						""variableName"": ""setting 2"",
						""variableValue"": ""red""
					}]
				}"
			);
			Request.SetMockRequest(mockRequest);

			var experiment = Resources.Load<Experiment>("Elecular/Tests/Experiment 1");
			experiment.GetSetting("setting 1", setting => 
			{
				Assert.AreEqual(
					setting,
					"blue"
				);	
			}, null);
			experiment.GetSetting("setting 2", setting => 
			{
				Assert.AreEqual(
					setting,
					"red"
				);	
			}, null);
			
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=" + SystemInfo.deviceUniqueIdentifier
			);
		}
		
		[Test]
		public void ExperimentsAreCached()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1"",
					""variables"": [{
						""variableName"": ""setting 1"",
						""variableValue"": ""blue""
					}, {
						""variableName"": ""setting 2"",
						""variableValue"": ""red""
					}]
				}"
			);
			Request.SetMockRequest(mockRequest);

			var experiment = Resources.Load<Experiment>("Elecular/Tests/Experiment 1");
			experiment.GetSetting("setting 1", setting => 
			{
				Assert.AreEqual(
					setting,
					"blue"
				);	
			}, null);
			
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=" + SystemInfo.deviceUniqueIdentifier
			);
			
			var mockRequest2 = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1"",
					""variables"": [{
						""variableName"": ""setting 1"",
						""variableValue"": ""green""
					}, {
						""variableName"": ""setting 2"",
						""variableValue"": ""purple""
					}]
				}"
			);
			Request.SetMockRequest(mockRequest2);
			experiment.GetSetting("setting 1", setting => 
			{
				Assert.AreEqual(
					setting,
					"blue"
				);	
			}, null);
			
			Assert.AreEqual(mockRequest2.GetUnityWebRequest(), null);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanGetSpecificVariationOfExperiment()
		{
			Request.SetMockRequest(null);
			ExperimentsApi.Instance.ClearCache();
			var response = 0;
			
			var experiment = Resources.Load<Experiment>("Elecular/Tests/Forced Control Group Experiment");
			experiment.GetVariation(variation => 
			{
				Assert.AreEqual(
					variation.Name,
					"Control Group"
				);
				response++;
			});
			
			experiment = Resources.Load<Experiment>("Elecular/Tests/Forced Variation 1 Experiment");
			experiment.GetVariation(variation => 
			{
				Assert.AreEqual(
					variation.Name,
					"Variation 1"
				);
				response++;
			});
			yield return new WaitUntil(() => response == 2);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanGetSpecificSettingOfExperiment()
		{
			Request.SetMockRequest(null);
			ExperimentsApi.Instance.ClearCache();
			var response = 0;
			
			var experiment = Resources.Load<Experiment>("Elecular/Tests/Forced Control Group Experiment");
			experiment.GetSetting("Button Color", setting => 
			{
				Assert.AreEqual(
					setting,
					"red"
				);
				response++;
			});
			
			experiment = Resources.Load<Experiment>("Elecular/Tests/Forced Variation 1 Experiment");
			experiment.GetSetting("Button Color", setting => 
			{
				Assert.AreEqual(
					setting,
					"blue"
				);
				response++;
			});
			yield return new WaitUntil(() => response == 2);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanGetAllVariations()
		{
			Request.SetMockRequest(null);
			ExperimentsApi.Instance.ClearCache();
			var response = 0;
			
			var experiment = Resources.Load<Experiment>("Elecular/Tests/Experiment 1");
			experiment.GetAllVariations( variations => 
			{
				Assert.AreEqual(
					variations.Length,
					2
				);
				response++;
			});
			yield return new WaitUntil(() => response == 1);
		}
		
		[TearDown]
		public void TearDown()
		{
			ElecularApi.Instance.Reset();
			Request.SetMockRequest(null);
		}
	}
}
