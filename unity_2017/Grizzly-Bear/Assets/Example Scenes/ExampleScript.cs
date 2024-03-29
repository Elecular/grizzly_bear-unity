﻿using System;
using Elecular.API;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{

	[SerializeField] 
	private Experiment experiment;
	
	// Use this for initialization
	void Awake ()
	{
		ElecularApi.Instance.InitializeWithTracking();
	}

	private void LateUpdate()
	{
		GameObject.FindObjectOfType<SessionNotifier>().SetSessionInactiveTimeThreshold(1);
	}

	public void Log()
	{
		
			ElecularApi.Instance.LogAdImpression("level completion");
			ElecularApi.Instance.LogAdClick("level completion");
			ElecularApi.Instance.LogTransaction("level completion", 10000.99999513m);
			ElecularApi.Instance.LogCustomEvent("level completion", 2);	
		
	}
}
