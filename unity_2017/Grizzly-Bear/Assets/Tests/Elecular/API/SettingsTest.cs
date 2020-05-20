using Elecular.API;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Elecular.API
{
    public class SettingsTest 
    {
        [Test]
        public void TestIfSettingsIsDefault()
        {
            var settings = Resources.Load<ElecularSettings>("Elecular/Settings");
            Assert.AreEqual(settings.GetProjectIdWithoutWarning(), "");
        }
    }
}