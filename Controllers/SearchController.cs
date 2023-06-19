using System.Threading.Tasks;
using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Mentore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        // api/search?keyword=
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var rs = await _searchService.SearchItem(keyword);
            if (rs == null)
                return BadRequest("Can't not find item with the key word !");

            return Ok(rs);
        }
    }
}
