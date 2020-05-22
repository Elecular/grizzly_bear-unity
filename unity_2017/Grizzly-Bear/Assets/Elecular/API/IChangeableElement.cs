using System.Collections.Generic;

namespace Elecular.API
{
	/// <summary>
	/// A Changeable element is a element that can change itself based on a variation.
	/// </summary>
	public interface IChangeableElement
	{
		/// <summary>
		/// The experiment this element is part of
		/// </summary>
		Experiment Experiment { get; }
		
		/// <summary>
		/// The variation configurations for this changeable element
		/// </summary>
		IEnumerable<VariationConfiguration> Configurations { get; }
	}
}

