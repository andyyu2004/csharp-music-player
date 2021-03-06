﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CMusicPlayer.Configuration;
using CMusicPlayer.Data.Network.Types.Exceptions;
using CMusicPlayer.Internal.Types.DataStructures;
using CMusicPlayer.Internal.Types.Functional;

namespace CMusicPlayer.Data.Network
{
    internal class HttpService : IHttpService
    {
        private static readonly NDictionary<string, string> Auth = Config.Settings[Config.Authentication];
        private readonly HttpClient client = new HttpClient();

        public HttpService()
        {
//            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Auth[Config.JwtToken]);
        }

        private static string BaseUrl => Auth[Config.ApiEndpoint] ?? string.Empty;

        public async Task<Try<string>> GetAsync(string uri)
        {
            try
            {
                var res = await client.GetAsync($"{BaseUrl}/{uri}");
                return await ProcessResponse(res);
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public async Task<Try<string>> PostAsync(string uri, string obj)
        {
            try
            {
                var content = new StringContent(obj, Encoding.UTF8, "application/json");
                var res = await client.PostAsync($"{BaseUrl}/{uri}", content);
                return await ProcessResponse(res);
            }
            catch (Exception e)
            {
                return e;
            }
        }

        private static async Task<Try<string>> ProcessResponse(HttpResponseMessage res)
        {
            switch (res.StatusCode)
            {
                case HttpStatusCode.OK:
                    var resString = await res.Content.ReadAsStringAsync();
                    return new Try<string>(resString);
                default:
                    return new HttpException($"Request Status code not OK : {res.StatusCode}", res.StatusCode);
            }
        }
    }
}