using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.WebApi
{
    public class SocialNetworkRequest
    {
        [Url, MaxLength(500)]
        public string Facebook { get; set; }

        [Url, MaxLength(500)]
        public string Linkedin { get; set; }

        [Url, MaxLength(500)]
        public string BitcoinTalk { get; set; }

        [Url, MaxLength(500)]
        public string Medium { get; set; }

        [Url, MaxLength(500)]
        public string Reddit { get; set; }

        [Url, MaxLength(500)]
        public string Telegram { get; set; }

        [Url, MaxLength(500)]
        public string Twitter { get; set; }

        [Url, MaxLength(500)]
        public string Github { get; set; }

        public static SocialNetworks ToDomain(SocialNetworkRequest request)
        {
            return new SocialNetworks
                   {
                       BitcoinTalk = request.BitcoinTalk,
                       Facebook = request.Facebook,
                       Github = request.Github,
                       Linkedin = request.Linkedin,
                       Reddit = request.Reddit,
                       Medium = request.Medium,
                       Telegram = request.Telegram,
                       Twitter = request.Twitter
                   };
        }
    }
}