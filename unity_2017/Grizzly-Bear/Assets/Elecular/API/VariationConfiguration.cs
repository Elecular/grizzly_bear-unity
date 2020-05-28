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
		public abstract Object GetTarget(GameObject gameObject);
	}	
}

