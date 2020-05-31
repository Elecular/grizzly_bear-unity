using System.Collections.Generic;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// This is used for showing debug information on the game screen
	/// </summary>
	public class DebugWindow : MonoBehaviour
	{
		[SerializeField]
		private GUISkin windowSkin;

		private Queue<string> activity = new Queue<string>();
		
		private void Awake()
		{
			gameObject.name = "Elecular - Debug Window";
			DontDestroyOnLoad(this);
			ElecularApi.Instance.RegisterOnActivityLog(OnActivityLog);
		}

		private void OnActivityLog(string userAction)
		{
			activity.Enqueue(userAction);
			if (activity.Count > 10)
			{
				activity.Dequeue();
			}
			Debug.Log(userAction);
		}

		private void OnGUI()
		{
			var prevSkin = GUI.skin;
			GUI.skin = windowSkin;
			GUILayout.Window(0, new Rect(10, 10, 275, 25), DrawWindowContents, "Elecular");
			GUI.skin = prevSkin;
		}

		private void DrawWindowContents(int windowId)
		{
			windowSkin.label.fontStyle = FontStyle.Bold;
			GUILayout.Space(10);
			if (ElecularApi.Instance.IsTracking)
			{
				BoldLabel("Session");
				Label("Session ID: " + ElecularApi.Instance.SessionId);
				Label("Elapsed Time: " + Mathf.Round(ElecularApi.Instance.ElapsedSessionTime));
				GUILayout.Space(10);
				BoldLabel("Activity");
				foreach (var userAction in activity)
				{
					Label(userAction);
				}
			}
			else
			{
				Label("Elecular Api is not tracking user activity.");
			}
		}

		private void BoldLabel(string label)
		{
			windowSkin.label.fontStyle = FontStyle.Bold;
			GUILayout.Label(label);
		}

		private void Label(string label)
		{
			windowSkin.label.fontStyle = FontStyle.Normal;
			GUILayout.Label(label);
		}
	}
	
}