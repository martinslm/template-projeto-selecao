﻿using gerenciador_hashtags_twitter.Data.InMemoryDb.Models;
using System;
using System.Linq;

namespace gerenciador_hashtags_twitter.Data.InMemoryDb.Extensions.InMemoryDbContextExtensions
{
    public static class HashtagSeed
    {
        public static void SeedHashtag(this InMemoryDbContext context)
        {
            SeedPets(ref context);
            SeedDevelopment(ref context);
            SeedDesign(ref context);
        }

        private static void SeedPets(ref InMemoryDbContext context)
        {
            var userLarissa = context.Users
                                    .Where(c => 
                                    c.Username == "larissamartins")
                                    .FirstOrDefault();

            var pets = new Hashtag("Pets", userLarissa.Id);
            context.Hashtags.Add(pets);
        }

        private static void SeedDevelopment(ref InMemoryDbContext context)
        {
            var userJohn = context.Users
                                  .Where(c => 
                                  c.Username == "John08")
                                  .FirstOrDefault();

            var development = new Hashtag("Development", userJohn.Id);
            context.Hashtags.Add(development);
        }

        private static void SeedDesign(ref InMemoryDbContext context)
        {
            var userJohn = context.Users
                                  .Where(c =>
                                  c.Username == "John08")
                                  .FirstOrDefault();

            var userLarissa = context.Users
                                     .Where(c =>
                                     c.Username == "larissamartins")
                                     .FirstOrDefault();

            var designJohn = new Hashtag("Design", userJohn.Id);
            context.Hashtags.Add(designJohn);

            var designLarissa = new Hashtag("Design", userLarissa.Id);
            context.Hashtags.Add(designLarissa);
        }

    }
}
