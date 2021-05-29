﻿using gerenciador_hashtags_twitter.Data.InMemoryDb.Models;
using gerenciador_hashtags_twitter.Domain.Models.Contracts;
using gerenciador_hashtags_twitter.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gerenciador_hashtags_twitter.Data.InMemoryDb.Repositories
{
    public sealed class TweetRepository :
        ITweetRepository
    {
        private readonly List<Tweet> _tweetsDbContext;
        public TweetRepository(InMemoryDbContext context)
        {
            _tweetsDbContext = context.Tweets;
        }

        public Task Add(IEnumerable<ITweet> tweets)
        {
            _tweetsDbContext.AddRange(tweets.Cast<Tweet>());
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<ITweet>> Get(IHashtag hashtag)
        {
            var tweets = _tweetsDbContext.Where(t => 
                                            t.HashtagId.Equals(hashtag.Id))
                                            .ToList();

            return Task.FromResult((IReadOnlyCollection<ITweet>)tweets);
        }
    }
}
