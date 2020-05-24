using System;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// Enum to represent development environments
	/// </summary>
	public enum Environment {
		Dev,Stage,Prod
	}

	static class EnvironmentMethods
	{
		/// <summary>
		/// Gets string representation of Environment
		/// </summary>
		/// <param name="environment"></param>
		/// <returns></returns>
		public static string GetString(this Environment environment)
		{
			return environment.ToString().ToLower();
		}
	}
}

