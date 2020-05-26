using System;
using Elecular.API;
using UnityEngine;

public class ExampleScript : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		ElecularApi.Instance.Initialize();
	}

	private void LateUpdate()
	{
		GameObject.FindObjectOfType<SessionNotifier>().SetSessionInactiveTimeThreshold(1);
	}

	public void Log()
	{
		for (int count = 0; count < 100; count++)
		{
			ElecularApi.Instance.LogAdImpression("level completion");
			ElecularApi.Instance.LogAdClick("level completion");
			ElecularApi.Instance.LogTransaction("level completion", 0.99f);
			ElecularApi.Instance.LogCustomEvent("level completion", 2);	
		}
	}
}
