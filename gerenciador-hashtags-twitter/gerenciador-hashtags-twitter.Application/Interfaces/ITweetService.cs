﻿using gerenciador_hashtags_twitter.Application.DTOs.Request;
using gerenciador_hashtags_twitter.Application.DTOs.Response;
using System.Threading.Tasks;

namespace gerenciador_hashtags_twitter.Application.Interfaces
{
    public interface ITweetService
    {
        public Task<GetTweetsResponseData> Get(GetTweetsRequestData request);
    }
}