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
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/TestScene");
			var button = GameObject.Find("Forced Control Group Experiment");
			yield return new WaitUntil(() => button.GetComponent<Button>().transition == Selectable.Transition.ColorTint);
			
			button = GameObject.Find("Forced Variation 1 Experiment");
			yield return new WaitUntil(() => button.GetComponent<Button>().transition == Selectable.Transition.SpriteSwap);
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
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/TestScene 1");
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
			ElecularApi.Instance.Initialize();
			
			//Creating a new session
			yield return null;
			var sessionNotifier = GameObject.FindObjectOfType<SessionNotifier>();
			sessionNotifier.SetSessionInactiveTimeThreshold(1);
			sessionNotifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(2);
			
			//We are mixing up two requests together. We put the sessions data and variation data in one request
			//This is because Json.FromUtility only picks up variables it is looking for.
			//So when Elecular API tried to create a new session, it will only pick up the _id field
			//And after that when the button tries to get the new variation, it will only pick up the variationName
			//This is hacky, I know!
			mockRequest = new MockRequest(
				mockResponse: @"{
					""_id"": ""session id 2"",
					""variationName"": ""Control Group""
				}"
			);
			Request.SetMockRequest(mockRequest);
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
			yield return SceneManager.LoadSceneAsync("Tests/Elecular/TestScene 1");
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
			ElecularApi.Instance.Initialize();
			
			//Creating a new session
			yield return null;
			var sessionNotifier = GameObject.FindObjectOfType<SessionNotifier>();
			sessionNotifier.SetSessionInactiveTimeThreshold(1);
			sessionNotifier.OnApplicationFocus(false);
			yield return new WaitForSeconds(2);
			
			mockRequest = new MockRequest(
				mockResponse: @"{
					""_id"": ""session id 2"",
					""variationName"": ""Control Group""
				}"
			);
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

