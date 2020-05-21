﻿using System.Collections;
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
		public void CanGetVariationOfExperimentWithoutUsername()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1"",
					""variables"": []
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
		public void CannotGetExperimentWithInvalidExperiment()
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
		public void CanGetVariationOfExperimentWithUsername()
		{
			ExperimentsApi.Instance.ClearCache();
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1"",
					""variables"": []
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
			}, null, "testUser");
			
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=testUser"
			);
		}
		
		[Test]
		public void CanGetSettingOfExperimentWithoutUsername()
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
			}, null, "testUser");
			experiment.GetSetting("setting 2", setting => 
			{
				Assert.AreEqual(
					setting,
					"red"
				);	
			}, null, "testUser");
			
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://experiments.api.elecular.com/projects/5ec39b16750f8d0012e5c027/experiments/Experiment%201/variations?userId=testUser"
			);
		}
		
		[Test]
		public void CanGetSettingOfExperimentWithUsername()
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
	}
}
