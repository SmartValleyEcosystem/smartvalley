using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Test.Rest;

namespace SmartValley.WebApi.Test
{
    [Route("/api/test")]
    [Authorize(AuthenticationSchemes = EcdsaAuthenticationOptions.DefaultScheme)]
    public class TestController : Controller
    {
        [HttpGet]
        public TestResponse Get()
        {
            return new TestResponse {Value = "Get"};
        }

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