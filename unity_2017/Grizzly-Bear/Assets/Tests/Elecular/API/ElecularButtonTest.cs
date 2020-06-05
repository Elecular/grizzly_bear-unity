using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Elecular.API;
using Elecular.Core;
using Tests.Elecular.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tests.Elecular.API
{
	public class ElecularButtonTest 
	{
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator CanSetVariationOfElecularButton()
		{
			Request.SetMockRequest(null);
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/Button TestScene");
			var button = GameObject.Find("Button (Control Group)").GetComponent<Button>();
			yield return new WaitUntil(() => button.transition == Selectable.Transition.ColorTint);
			
			button = GameObject.Find("Button (Variation 1)").GetComponent<Button>();
			yield return new WaitUntil(() => button.transition == Selectable.Transition.SpriteSwap);
		}

		[UnityTest]
		[Timeout(10000)]
		public IEnumerator ElecularButtonCanBeUpdatedIfFlagIsSet()
		{
			ElecularApi.Instance.CanUpdateVariationsWhileRunning = true;
			//Mocking a variation request to render button under variation 1
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1""
				}"
			);
			Request.SetMockRequest(mockRequest);

			//Loading scene and rendering button
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/Button TestScene 2");
			var button = GameObject.Find("Experiment 1");
			//Checking if button is rendering variation 1
			Assert.AreEqual(button.GetComponent<Button>().transition, Selectable.Transition.ColorTint);

			//Mocking request for creating new session
			mockRequest = new MockRequest(
				mockResponse: @"{
					""_id"": ""session id""
				}"
			);
			Request.SetMockRequest(mockRequest);
			//Initializing Elecular API 
			ElecularApi.Instance.InitializeWithTracking();

			//Creating a new session
			yield return null;
			var sessionNotifier = GameObject.FindObjectOfType<SessionNotifier>();
			sessionNotifier.SetSessionInactiveTimeThreshold(1);
			sessionNotifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(2);
			
			//Setting multiple requests
			//Elecular API will first load the variation 
			//And then it creates a new session.
			Request.SetMultipleMockRequest(new Request[]
			{
				new MockRequest(@"{""variationName"": ""Control Group""}"),
				new MockRequest(@"{""_id"": ""session id 2""}")
			});

			sessionNotifier.OnApplicationFocus(true);
				Assert.AreEqual(button.GetComponent<Button>().transition, Selectable.Transition.SpriteSwap);
		}
		
		[UnityTest]
		[Timeout(10000)]
		public IEnumerator ElecularButtonCannotBeUpdated()
		{
			//Mocking a variation request to render button under variation 1
			var mockRequest = new MockRequest(
				mockResponse: @"{
					""variationName"": ""Variation 1""
				}"
			);
			Request.SetMockRequest(mockRequest);
			
			//Loading scene and rendering button
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/Button TestScene 2");
			var button = GameObject.Find("Experiment 1");
			//Checking if button is rendering variation 1
			Assert.AreEqual(button.GetComponent<Button>().transition, Selectable.Transition.ColorTint);
			
			//Mocking request for creating new session
			mockRequest = new MockRequest(
				mockResponse: @"{
					""_id"": ""session id""
				}"
			);
			Request.SetMockRequest(mockRequest);
			//Initializing Elecular API 
			ElecularApi.Instance.InitializeWithTracking();
			
			//Creating a new session
			yield return null;
			var sessionNotifier = GameObject.FindObjectOfType<SessionNotifier>();
			sessionNotifier.SetSessionInactiveTimeThreshold(1);
			sessionNotifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(2);
			
			//Setting multiple requests
			//Elecular API will first load the variation 
			//And then it creates a new session.
			Request.SetMultipleMockRequest(new Request[]
			{
				new MockRequest(@"{""variationName"": ""Control Group""}"),
				new MockRequest(@"{""_id"": ""session id 2""}")
			});
			
			Request.SetMockRequest(mockRequest);
			sessionNotifier.OnApplicationFocus(true);
			Assert.AreEqual(button.GetComponent<Button>().transition, Selectable.Transition.ColorTint);
		}
		
		[TearDown]
		public void TearDown()
		{
			ElecularApi.Instance.Reset();
		}
	}
}

