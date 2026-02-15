using Api.DTOs;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            _logger.LogInformation("Received request to fetch all categories.");
            try
            {
                var categories = await _categoryService.GetAllCategories();
                _logger.LogInformation("Returning {Count} categories to the client.", categories?.Count() ?? 0);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request for categories.");
                return StatusCode(500, "Internal server error occurred while retrieving categories.");
            }
        }
    }
}