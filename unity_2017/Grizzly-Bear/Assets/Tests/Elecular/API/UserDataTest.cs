using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Elecular.API;
using UnityEngine;

namespace Tests.Elecular.API
{
	public class UserDataTest {

		[Test]
		public void UserDataCanHaveGender() {
			var data = new UserData();
			data.Gender = Gender.Female;
			Assert.Contains("gender/Female", data.GetAllSegments());
		}
		
		[Test]
		public void UserDataCanNotHaveGender() {
			var data = new UserData();
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Contains("gender")));
		}
		
		[Test]
		public void UserCanHaveLocation() {
			var data = new UserData();
			data.Location = Location.SouthKorea;
			Assert.Contains("country/South Korea", data.GetAllSegments());
		}
		
		[Test]
		public void UserCanNotHaveLocation() {
			var data = new UserData();
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Contains("country")));
		}
		
		[Test]
		public void UserCanNotHaveAge() {
			var data = new UserData();
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Contains("age")));
		}
		
		[Test]
		public void UserCanHaveAgeInRange() {
			var data = new UserData();
			
			data.BirthdayYear = DateTime.Now.Year - 18;
			Assert.Contains("age/18 - 24", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 24;
			Assert.Contains("age/18 - 24", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 20;
			Assert.Contains("age/18 - 24", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 30;
			Assert.Contains("age/25 - 34", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 25;
			Assert.Contains("age/25 - 34", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 34;
			Assert.Contains("age/25 - 34", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 64;
			Assert.Contains("age/55 - 64", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 65;
			Assert.Contains("age/65+", data.GetAllSegments());
			
			data.BirthdayYear = DateTime.Now.Year - 600;
			Assert.Contains("age/65+", data.GetAllSegments());
		}
		
		[Test]
		public void UserHasPlatform() {
			var data = new UserData();
			Assert.NotNull(data.GetAllSegments().FirstOrDefault(segment => segment.Contains("platform")));
		}
		
		[Test]
		public void UserCanHaveCustomSegments() {
			var data = new UserData();
			data.AddCustomSegments(new List<string>(){"custom1", null, "custom2"});
			Assert.Contains("custom/custom1", data.GetAllSegments());
			Assert.Contains("custom/custom2", data.GetAllSegments());
		}
		
		[Test]
		public void CanNotHaveRetention() {
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, "");
			var data = new UserData();
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Contains("retention")));
			
			var date = DateTime.UtcNow.Subtract(TimeSpan.FromHours(14));
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, date.Ticks.ToString());
			data = new UserData();
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Contains("retention")));

		}
		
		[Test]
		public void CanHaveDayOneRetention() {
			var date = DateTime.UtcNow.Subtract(TimeSpan.FromHours(30));
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, date.Ticks.ToString());
			var data = new UserData();
			Assert.Contains("retention/Day 1", data.GetAllSegments());
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Equals("retention/Day 7")));
			
			date = DateTime.UtcNow.Subtract(TimeSpan.FromDays(3));
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, date.Ticks.ToString());
			data = new UserData();
			Assert.Contains("retention/Day 1", data.GetAllSegments());
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Equals("retention/Day 7")));
		}
		
		[Test]
		public void CanHaveDaySevenRetention() {
			var date = DateTime.UtcNow.Subtract(TimeSpan.FromDays(8));
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, date.Ticks.ToString());
			var data = new UserData();
			Assert.Contains("retention/Day 1", data.GetAllSegments());
			Assert.Contains("retention/Day 7", data.GetAllSegments());
			Assert.Null(data.GetAllSegments().FirstOrDefault(segment => segment.Equals("retention/Day 30")));
		}
		
		[Test]
		public void CanHaveDayThirtyRetention() {
			var date = DateTime.UtcNow.Subtract(TimeSpan.FromDays(34));
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, date.Ticks.ToString());
			var data = new UserData();
			Assert.Contains("retention/Day 1", data.GetAllSegments());
			Assert.Contains("retention/Day 7", data.GetAllSegments());
			Assert.Contains("retention/Day 30", data.GetAllSegments());
		}
		
		[Test]
		public void CalculatesRetentionCorrectly() {
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, "");
			Assert.AreEqual(new UserData().Retention, 0);

			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(13)).Ticks.ToString());
			Assert.AreEqual(new UserData().Retention, 0);
			
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(24)).Ticks.ToString());
			Assert.AreEqual(new UserData().Retention, 1);
			
			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(30)).Ticks.ToString());
			Assert.AreEqual(new UserData().Retention, 1);

			PlayerPrefs.SetString(UserData.PlayerPrefsInstallTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(500)).Ticks.ToString());
			Assert.AreEqual(new UserData().Retention, 20);
		}
	}	
}

