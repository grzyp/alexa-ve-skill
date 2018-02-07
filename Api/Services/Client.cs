using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Api.Services
{
    public class Client : IClient
    {
        private readonly HttpClient _httpClient;

        public Client()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://volleyballengland.org/")
            };
        }

        public async Task<Stream> GetPage(string path)
        {
            return await _httpClient.GetStreamAsync(path);
        }

    }
}
