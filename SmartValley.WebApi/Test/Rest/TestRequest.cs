using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Test.Rest
{
    public class TestRequest
    {
        [Required]
        public string Value { get; set; }
    }
}