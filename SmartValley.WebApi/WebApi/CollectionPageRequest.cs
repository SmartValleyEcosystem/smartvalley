using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.WebApi
{
    public class CollectionPageRequest
    {
        public CollectionPageRequest()
        {
            Count = 100;
        }

        public int Offset { get; set; }

        //[Range(1, 100)]
        public int Count { get; set; }
    }
}