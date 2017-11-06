using System.ComponentModel.DataAnnotations;

namespace SmartValley.Test.Rest
{
    public class TestRequest
    {
        [Required]
        public string Value { get; set; }
    }
}