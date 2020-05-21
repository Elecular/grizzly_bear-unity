using Elecular.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Tests.Elecular.Core
{
	/// <summary>
	/// Mocks a Http Request
	/// </summary>
	public class MockRequest : Request
	{
		private string mockResponse;

		public MockRequest(string mockResponse)
		{
			this.mockResponse = mockResponse;
		}

		/// <inheritdoc />
		public override void Send<T>(UnityAction<T> onResponse, UnityAction onError)
		{
			if (mockResponse == null)
			{
				onError();
			}
			else
			{
				onResponse(JsonUtility.FromJson<T>(mockResponse));	
			}
		}
		
		/// <summary>
		/// Gets the request that was made
		/// </summary>
		/// <returns></returns>
		public UnityWebRequest GetUnityWebRequest()
		{
			if (unityWebRequests == null || unityWebRequests.Length == 0) return null;
			return unityWebRequests[0];
		}
	}
}
