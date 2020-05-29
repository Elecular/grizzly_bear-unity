using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Elecular.API
{
	/// <summary>
	/// A Variation configuration defines how a Changeable element looks like in a certain variation
	/// </summary>
	[Serializable]
	public abstract class VariationConfiguration
	{
		/// <summary>
		/// This variable is only used by the editor to do some checks. 
		/// </summary>
		[SerializeField] 
		protected string experimentName;
		
		[SerializeField] 
		protected string variationName;
		
		/// <summary>
		/// Name of the variation
		/// </summary>
		public string VariationName
		{
			get { return variationName; }
		}
		
		/// <summary>
		/// Name of the experiment this variation is part of
		/// </summary>
		public string ExperimentName
		{
			get { return experimentName; }
		}
		
		/// <summary>
		/// Gets the Object that is going to have the variation applied to it
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public abstract Component GetTarget(GameObject gameObject);
		
		/// <summary>
		/// Disables the Target component.
		/// This is called before SetupTarget.
		/// It used for refreshing the target. This variation first disables the target, calls setup and then enables it back again.
		/// If the target does not need refreshing to reflect new changes, leave this method empty
		/// </summary>
		/// <param name="gameObject"></param>
		public abstract void DisableTarget(GameObject gameObject);
		
		/// <summary>
		/// Enables the Target component
		/// This is called after SetupTarget.
		/// It used for refreshing the target. This variation first disables the target, calls setup and then enables it back again.
		/// If the target does not need refreshing to reflect new changes, leave this method empty
		/// </summary>
		/// <param name="gameObject"></param>
		public abstract void EnableTarget(GameObject gameObject);
		
		/// <summary>
		/// Sets up the target so that it represents this variation
		/// </summary>
		/// <param name="gameObject"></param>
		public abstract void SetupTarget(GameObject gameObject);
	}	
}

