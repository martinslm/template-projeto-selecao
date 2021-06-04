﻿using System;

namespace gerenciador_hashtags_twitter.Data.InMemoryDb.Models
{
    public sealed class Tweet :
        Domain.Models.Entities.Tweet
    {
        public Tweet(string message, string author, DateTime publishDate, string hashtagContent) 
            : base(message, author, publishDate, hashtagContent)
        {
        }
    }
}
