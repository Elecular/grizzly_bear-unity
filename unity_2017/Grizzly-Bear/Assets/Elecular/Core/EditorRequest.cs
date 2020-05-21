#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Elecular.Core
{
	public class EditorRequest : Request
	{
		private UnityWebRequest webRequest;
		
		private UnityAction<string> onWebResponse;
		
		private UnityAction onWebError;

		public EditorRequest(UnityWebRequest[] requests)
		{
			webRequest = requests[0];
		}

		public override void Send<T>(UnityAction<T> onResponse, UnityAction onError)
		{
			onWebResponse = (res) =>
			{
				if (res.Length > 0 && res[0] == '[' && res[res.Length - 1] == ']')
				{
					res = "{ \"array\": " + res + "}";
				}
				onResponse(JsonUtility.FromJson<T>(res));
			};
			onWebError = onError;
			webRequest.SendWebRequest();
			EditorApplication.update += ProcessRequest;
		}
		
		private void ProcessRequest()
		{
			try
			{
				if (webRequest.isNetworkError)
				{
					throw new Exception(
						"Error while connecting to Elecular Server. Please check your internet connection. If the problem persists, please contact support@elecular.com"
					);
				}
				if (webRequest.isHttpError)
				{
					throw new Exception(!webRequest.downloadHandler.text.Equals("")
						? webRequest.downloadHandler.text 
						: webRequest.error);
				}
				if (webRequest.responseCode == 0) return;
				onWebResponse(webRequest.downloadHandler.text);
				EditorApplication.update -= ProcessRequest;
			}
			catch (Exception e)
			{
				EditorApplication.update -= ProcessRequest;
				onWebError();
			}
		}
	}
}
#endif
