using Mentore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mentore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
    }
}