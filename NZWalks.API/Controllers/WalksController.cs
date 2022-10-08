using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;

        public WalksController(IWalkRepository walkRepository)
        {
            this.walkRepository = walkRepository;
        }
        public async Task<IActionResult> GetAllWalksAsync()
        {
            return await walkRepository.GetAllAsync();
        }
    }
}
