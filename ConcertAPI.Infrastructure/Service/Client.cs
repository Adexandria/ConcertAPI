using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace Concert.Infrastructure.Service
{
    public class Client
    {

        public async Task<string> RequestTokenAsync()
        {
            HttpClient client = new HttpClient();
            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync("http://localhost:34085");
            if (disco.IsError)
            {
                return disco.Error;
            }
            
            TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "myApi.read"
            });
            if (tokenResponse.IsError)
            {
                return tokenResponse.Error;
            }
            return tokenResponse.AccessToken;
        }
        
    }
}
