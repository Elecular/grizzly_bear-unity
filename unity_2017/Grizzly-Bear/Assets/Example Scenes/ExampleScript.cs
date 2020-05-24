using System.Collections;
using System.Collections.Generic;
using Elecular.API;
using UnityEngine;

public class ExampleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ElecularApi.Instance.Initialize();
	}

	public void Log()
	{
		ElecularApi.Instance.LogAdImpression("video");
	}
}
