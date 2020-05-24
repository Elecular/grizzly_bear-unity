using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Elecular.API
{
	/// <summary>
	/// Class for storing user data such as demographics and custom segments
	/// </summary>
	public class UserData
	{
		private int[,] ageSplits = {{18, 24},{25, 34},{35, 44},{45, 54},{55, 64}, {65, -1}};
		
		private Gender gender = Gender.Unknown;

		private Location location = Location.Unknown;
		
		private int birthdayYear = int.MaxValue;

		private List<string> customSegments = new List<string>();

		/// <summary>
		/// Sets gender of the user
		/// If the gender is unknown, simply set it to unknown
		/// </summary>
		public Gender Gender
		{
			set { gender = value; }
		}
		
		/// <summary>
		/// Location of the user.
		/// Elecular supports 11 countries.
		/// If the user lives in a country that we do not support, set Location to Rest Of World.
		/// If the user location is not known, set Location to Unknown
		/// </summary>
		public Location Location
		{
			set { location = value; }
		}
		
		/// <summary>
		/// Sets the birth year of the user. Used for calculating age
		/// </summary>
		public int BirthdayYear
		{
			set
			{
				birthdayYear = value;
			}
		}
		
		/// <summary>
		/// Adds custom segments. You are allowed to have upto 40 unique segments across your whole project at any given time.
		/// For example, If user A has segments ["warrior", "axe"] and use B has segments ["mage"], this will add up to 3 unique segments across your whole project.
		///
		/// Example Usage: You want to segment users based on level progression.
		/// The recommended way to implement this is to split your levels into 3 categories: low, mid, high, and then use these as custom segments.
		/// The bad way to implement this is store the actual level as a segment. This means that if you have 100 levels, your project will have 100 different segments.
		/// Having so many segments is not only expensive but also not very useful. 
		/// </summary>
		/// <param name="segments"></param>
		public void AddCustomSegments(List<string> segments)
		{
			customSegments.AddRange(
				segments
					.FindAll(segment => segment != null)
					.Select(segment => string.Format("custom/{0}",segment))
			);
		}
		
		/// <summary>
		/// Gets a list of segments this user belong to
		/// </summary>
		/// <returns></returns>
		public string[] GetAllSegments()
		{
			var segments = new List<string>();
			segments.Add(string.Format("platform/{0}", Application.platform.ToString()));
			segments.AddRange(customSegments);
			if (gender != Gender.Unknown)
			{
				segments.Add(string.Format("gender/{0}", gender.ToString()));
			}
			if (location != Location.Unknown)
			{
				var locationSegment = Regex.Replace(
					location.ToString(), 
					"(\\B[A-Z])", 
					" $1"
				); 
				segments.Add(string.Format("country/{0}", locationSegment)); 
			}
			if (AgeSegment != null)
			{
				segments.Add(string.Format("age/{0}", AgeSegment));
			}
			return segments.ToArray();
		}
		
		private string AgeSegment
		{
			get
			{
				var age = DateTime.Now.Year - birthdayYear;
				for (var count = 0; count < ageSplits.GetLength(0); count++)
				{
					var minAge = ageSplits[count, 0];
					var maxAge = ageSplits[count, 1];
					if (age >= minAge && (age <= maxAge || maxAge == -1))
					{
						if (maxAge == -1)
						{
							return string.Format("{0}+", minAge); 
						}
						return string.Format("{0} - {1}", minAge, maxAge);
					}
				}
				return null;
			}
		}

		private string Platform
		{
			get
			{
				return Application.platform.ToString();
			}
		}
	}
	
	/// <summary>
	/// Enum to represent gender.
	/// If the gender is known, just set to Unknown
	/// </summary>
	public enum Gender
	{
		Male, Female, Unknown
	}
	
	/// <summary>
	/// Enum to represent Locations.
	/// Elecular only support 11 countries.
	/// If the user lives in a country that we do not support, set Location to Rest Of World
	/// If the user's location is not known, just set it to "Unknown"
	/// </summary>
	public enum Location
	{
		Australia,
		Canada,
		China,
		France,
		Germany,
		Japan,
		Russia,
		SouthKorea,
		Taiwan,
		UnitedKingdom,
		UnitedStates,
		RestOfWorld,
		Unknown
	}
}

