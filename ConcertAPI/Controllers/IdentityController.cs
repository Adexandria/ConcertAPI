using Concert.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConcertAPI.Controllers
{
    [Route("api/AccessToken")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly Client client;
        public IdentityController(Client client)
        {
            this.client = client;
        }
        [HttpGet]
        public async Task<IActionResult> GetAccessToke()
        {
            return Ok(await client.RequestTokenAsync());
        }
    }
}
