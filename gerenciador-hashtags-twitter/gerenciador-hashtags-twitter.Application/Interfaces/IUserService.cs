﻿using gerenciador_hashtags_twitter.Application.DTOs.Request;
using gerenciador_hashtags_twitter.Application.DTOs.Response;
using System.Threading.Tasks;

namespace gerenciador_hashtags_twitter.Application.Interfaces
{
    public interface IUserService
    {
        public Task<CreateUserResponseData> Create(CreateUserRequestData requestData);
        public Task<GetAcessTokenResponseData> GetAcessToken(GetAcessTokenRequestData requestData);
    }
}