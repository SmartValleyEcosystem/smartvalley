﻿using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class SocialNetworks
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
    }
}