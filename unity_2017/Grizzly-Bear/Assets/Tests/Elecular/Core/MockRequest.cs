using Elecular.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Tests.Elecular.Core
{
	/// <summary>
	/// Mocks a Http Request
	/// </summary>
	public class MockRequest : Request
	{
		private string mockResponse;

		public MockRequest(string mockResponse) : base(null)
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
	}
}
