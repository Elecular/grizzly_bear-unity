using System.Collections;
using Elecular.API;
using Elecular.Core;
using NUnit.Framework;
using Tests.Elecular.Core;
using UnityEngine.TestTools;

namespace Tests.Elecular.API
{
    public class UserActivityApiTest {

        [UnityTest]
        [Timeout(10000)]
        public IEnumerator CanLogSession()
        {
            var mockRequest = new MockRequest(
                mockResponse: @"{
					""_id"": ""fake_session""
				}"
            );
			Request.SetMockRequest(mockRequest);

			UserActivityApi.Instance.LogSession(
				"5ec39b16750f8d0012e5c027",
				new UserActivityApi.Session(
					"testUser", 
					new string[]{"platform/test"}, 
					"dev"
				),
				(id) =>
				{
					Assert.AreEqual(id, "fake_session");
				},
				() => {}
			);
			
			Assert.AreEqual(
				mockRequest.GetUnityWebRequest().url,
				"https://user-activity.api.elecular.com/projects/5ec39b16750f8d0012e5c027/user-session"
			);
			yield return null;
        }
    }
}