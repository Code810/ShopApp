using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Apps.AdminApp.Dtos.ProductDto;
using ShopApp.Data;
using ShopApp.Entities;

namespace ShopApp.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;
        private readonly IMapper _mapper;


        public ProductController(ShopAppContext shopAppContext, IMapper mapper)
        {
            this._shopAppContext = shopAppContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _shopAppContext.Products
                .Where(p => !p.IsDelete)
                .Include(p => p.Category)
                .ThenInclude(c => c.Products)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existProduct == null) return NotFound();

            return Ok(_mapper.Map<ProductReturnDto>(existProduct));
        }
        [HttpGet]
        public async Task<IActionResult> Get(string? search, int page = 1)
        {
            var query = _shopAppContext.Products
                .Where(p => !p.IsDelete);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            ProductListDto productListDto = new();
            productListDto.Page = page;
            productListDto.TotalCount = query.Count();
            productListDto.Items = await query.Skip((page - 1) * 2).Take(2)
                .Select(p => new ProductListItemDto()
                {
                    Id = p.Id,
                    CostPrice = p.CostPrice,
                    SalePrice = p.SalePrice,
                    Name = p.Name,
                    CreatedDate = p.CreatedDate,
                    UpdatedDate = p.UpdatedDate,
                    Category = new()
                    {
                        Name = p.Category.Name,
                        ProductsCount = p.Category.Products.Count,
                    }
                })
                .ToListAsync();

            return Ok(productListDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto productCreateDto)
        {
            var existCategory = await _shopAppContext.Categories.AnyAsync(c => !c.IsDelete && c.Id == productCreateDto.CategoryId);
            if (!existCategory) return StatusCode(StatusCodes.Status409Conflict);
            if (productCreateDto == null) return BadRequest();
            Product product = new()
            {
                CostPrice = productCreateDto.CostPrice,
                SalePrice = productCreateDto.SalePrice,
                Name = productCreateDto.Name,
                CategoryId = productCreateDto.CategoryId,
            };
            await _shopAppContext.Products.AddAsync(product);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, ProductUpdateDto productUpdateDto)
        {
            if (id is null) return StatusCode(StatusCodes.Status400BadRequest);
            var existProduct = await _shopAppContext.Products
                .Where(p => !p.IsDelete)
                .FirstOrDefaultAsync(y => y.Id == id);
            if (existProduct == null) return NotFound();
            var existCategory = await _shopAppContext.Categories.AnyAsync(c => !c.IsDelete && c.Id == productUpdateDto.CategoryId);
            if (!existCategory) return StatusCode(StatusCodes.Status409Conflict);

            existProduct.Name = productUpdateDto.Name;
            existProduct.SalePrice = productUpdateDto.SalePrice;
            existProduct.CostPrice = productUpdateDto.CostPrice;
            existProduct.CategoryId = productUpdateDto.CategoryId;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int? id, bool status)
        {
            if (id == null) return BadRequest();
            var existProduct = await _shopAppContext.Products
                .Where(p => !p.IsDelete)
                .FirstOrDefaultAsync(y => y.Id == id);
            if (existProduct == null) return NotFound();
            existProduct.IsDelete = status;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _shopAppContext.Products.Where(p => !p.IsDelete).FirstOrDefaultAsync(y => y.Id == id);
            if (existProduct == null) return NotFound();
            _shopAppContext.Products.Remove(existProduct);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
