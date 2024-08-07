using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Apps.AdminApp.Dtos.CategoryDto;
using ShopApp.Data;
using ShopApp.Entities;
using ShopApp.Extensions;

namespace ShopApp.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;
        private readonly IMapper _mapper;

        public CategoryController(ShopAppContext shopAppContext, IMapper mapper)
        {
            this._shopAppContext = shopAppContext;
            _mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) return BadRequest();
            var existCategory = await _shopAppContext.Categories
                .Include(c => c.Products)
                .Where(p => !p.IsDelete)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existCategory == null) return NotFound();
            return Ok(_mapper.Map<CategoryReturnDto>(existCategory));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var isExistCategory = await _shopAppContext.Categories.AnyAsync(c => c.Name.ToLower() == categoryCreateDto.Name.ToLower());
            if (isExistCategory) return StatusCode(409);
            var file = categoryCreateDto.Photo;
            if (file == null) return BadRequest();

            Category category = new()
            {
                Name = categoryCreateDto.Name,
                Image = await file.SaveFile(),
            };
            await _shopAppContext.Categories.AddAsync(category);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(201);
        }
    }
}
