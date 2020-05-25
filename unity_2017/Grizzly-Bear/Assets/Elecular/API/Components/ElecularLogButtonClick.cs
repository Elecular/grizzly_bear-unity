using UnityEngine;
using UnityEngine.UI;

namespace Elecular.API
{
	/// <summary>
	/// Attach this component to a button in order to log clicks
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class ElecularLogButtonClick : MonoBehaviour
	{
		[SerializeField]
		[Tooltip("Name of the event")]
		private string eventId;
		
		private void Awake ()
		{
			if (eventId == null || eventId.Equals(""))
			{
				Debug.LogError("Event name is cannot be null or empty");
			}
			GetComponent<Button>().onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			ElecularApi.Instance.LogCustomEvent(eventId, 1);
		}
	}	
}
