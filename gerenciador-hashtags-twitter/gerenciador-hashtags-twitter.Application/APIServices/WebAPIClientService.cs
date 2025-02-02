﻿using gerenciador_hashtags_twitter.Application.DTOs.Interfaces;
using gerenciador_hashtags_twitter.Application.Exceptions;
using gerenciador_hashtags_twitter.Application.Interfaces;
using gerenciador_hashtags_twitter.Application.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace gerenciador_hashtags_twitter.Application.APIServices
{
    public sealed class WebAPIClientService :
        IWebAPIClientService
    {
        private readonly HttpClient _httpClient;

        public WebAPIClientService(HttpClient httpClient, string authToken)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
        }

        public async Task<U> Get<T, U>(string uriPath, T requestData)
            where T : IRequestData
            where U : IResponseData
        {
            try
            {
                var queryString = SerializeRequestDataIntoQueryString(requestData);
                uriPath = AddQueryString(uriPath, queryString);

                var httpResponse = await _httpClient
                    .GetAsync(uriPath)
                    .ConfigureAwait(false);

                var responseObject = await CreateObjectFromResponse<U>(httpResponse)
                    .ConfigureAwait(false);

                return responseObject;

            }
            catch (ApplicationExternalServiceException)
            {
                throw;
            }
        }

        private Dictionary<string, string> SerializeRequestDataIntoQueryString<T>(T requestData)
        {
            var dictonaryProperties = new Dictionary<string, string>();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                foreach (object attr in prop.GetCustomAttributes(true))
                {
                    dictonaryProperties.Add(prop.Name, (attr as JsonPropertyAttribute).PropertyName);
                }
            }

            var dictionaries = (from property in requestData.GetType().GetProperties()
                                where property.GetValue(requestData, null) != null
                                select new { Key = dictonaryProperties[property.Name], Value = Convert.ToString(property.GetValue(requestData), CultureInfo.InvariantCulture) })
                            .ToDictionary(item => item.Key, item => item.Value);

            return dictionaries;
        }

        /// <summary>
        /// <see cref="https://github.com/aspnet/HttpAbstractions/blob/master/src/Microsoft.AspNetCore.WebUtilities/QueryHelpers.cs#L63"/>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private static string AddQueryString(string uri, Dictionary<string, string> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (queryString == null)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurence.
            if (anchorIndex != -1)
            {
                anchorText = uri[anchorIndex..];
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }

        private async Task<U> CreateObjectFromResponse<U>(HttpResponseMessage httpResponse)
           where U : IResponseData
        {
            if (httpResponse is null)
                throw new ArgumentNullException(nameof(httpResponse));
            else if (httpResponse.IsSuccessStatusCode)
            {
                var responseDataStringfied = await httpResponse
                    .Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                var responseObject = DeserializeResponseData<U>(responseDataStringfied);
                return responseObject;
            }
            else
                throw new ApplicationExternalServiceException(Resources.UnexpectedErrorWhenSearchingTweets);
        }

        private T DeserializeResponseData<T>(string stringfiedDataObject)
        {
           return JsonConvert.DeserializeObject<T>(stringfiedDataObject);
        }
    }
}