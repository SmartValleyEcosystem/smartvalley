using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartValley.Authentication;
using SmartValley.Common;
using SmartValley.Test.Rest;

namespace SmartValley.Test
{
    [Authorize(AuthenticationSchemes = MetamaskAuthenticationOptions.DefaultScheme)]
    public class TestController : ApiController
    {
        [HttpGet]
        public TestResponse Get()
        {
            return new TestResponse {Value = "Get"};
        }
      
        [AllowAnonymous]
        [HttpGet(nameof(GetCustom))]
        public TestResponse GetCustom()
        {
            return new TestResponse {Value = "Custom"};
        }

        [HttpGet("{id}")]
        public TestResponse Get(int id)
        {
            return new TestResponse {Value = id.ToString()};
        }

        [HttpPost]
        public TestResponse Post([FromBody] TestRequest request)
        {
            return new TestResponse {Value = request.Value};
        }
    }
}