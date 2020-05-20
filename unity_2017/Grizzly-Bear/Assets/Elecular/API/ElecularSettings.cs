using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// This holds all the settings needed for integrating with Elecular
	/// </summary>
	[CreateAssetMenu(menuName = "Elecular/Settings")]
	public class ElecularSettings : ScriptableObject
	{
		[SerializeField]
		private string projectId;
		
		/// <summary>
		/// Project Id of Elecular
		/// </summary>
		public string ProjectId
		{
			get
			{
				#if UNITY_EDITOR
				if (projectId == null || projectId.Equals(""))
				{
					if (UnityEditor.EditorUtility.DisplayDialog(
						"Missing Elecular Project Id",
						"Please set the project id in the Settings file under 'Elecular/Resources/Elecular'. We will highlight the file in the Project view for you.\n\nYou can get the project id by logging into https://app.elecular.com",
						"ok"
					))
					{
						UnityEditor.EditorApplication.isPlaying = false;
						UnityEditor.EditorGUIUtility.PingObject(this);
					}
				}
				#endif
				return projectId;
			}
		}
		
		/// <summary>
		/// Gets the project id without triggering an editor warning if it is empty.
		/// </summary>
		/// <returns></returns>
		public string GetProjectIdWithoutWarning()
		{
			return projectId;
		}
	}	
}
