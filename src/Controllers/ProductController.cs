using Microsoft.AspNetCore.Mvc;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using AutoMapper;

namespace MarketMedia.src.Controllers
{
    [Route("Product")]

    public class ProductController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public ProductController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }

        #region GetProduct
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _iRepository.GetProduct(id)
;
            var prodmap = _mapper.Map<ProductOutputDto>(product);
            return Ok(prodmap);
        }
        #endregion

        // GetAllProductsByCategory
        [HttpGet("ByCategory{CategoryId}")]
        public async Task<IActionResult> GetAllProductsByCategory(int CategoryId)
        {
            if ((_dbContext.Products.Where(t => t.CategoryId.Equals(CategoryId)).ToList().Count == 0))
            {
                return NotFound("No product found in that category");
            }
            var products=  await _iRepository.GetAllProductsByCategory(CategoryId);
            return Ok(products);
        }
        

        #region GetAllProducts
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var product = await _iRepository.GetAllProducts();
            var prodmap = _mapper.Map<List<ProductOutputDto>>(product);
            return Ok(prodmap);
        }
        #endregion

        #region PostProduct
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] ProductInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.CategoryId.ToString())) return BadRequest("Invalid input data.");
            if ((_dbContext.Products.Where(t => t.Name.Equals(inputDto.Name) && t.CategoryId.Equals(inputDto.CategoryId)).ToList()).Count > 0)
            {
                return BadRequest("product already exists");
            }

            var prod = _mapper.Map<Product>(inputDto);
            _iRepository.Add(prod);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateProduct
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductInputDto inputDto)
        {
            var product = await _iRepository.GetProduct(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.CategoryId.ToString())) return BadRequest("Invalid input data.");

            product.Name = inputDto.Name;
            product.Quality = inputDto.Quality; 
            product.CategoryId = inputDto.CategoryId;

            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region DeleteProduct
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _iRepository.GetProduct(id);
            _iRepository.Delete(product);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}
    

