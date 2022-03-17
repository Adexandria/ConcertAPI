using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RequestTokenAsync();
        }

        public static void RequestTokenAsync()
        {
            var client = new HttpClient();
            var disco =  client.GetDiscoveryDocumentAsync("http://localhost:34085").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "myApi.read"
            }).Result;
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = apiClient.GetAsync("http://localhost:13391/api/identity").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content =  response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
