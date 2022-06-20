using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;
namespace MarketMedia.src.Controllers
{
    [Route("Category")]
    public class CategoryController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;

        public CategoryController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }

        #region GetCategory(id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _iRepository.GetCategory(id);
            var catmap = _mapper.Map<CategoryOutputDto>(category);

            return Ok(catmap);
        }
        #endregion

        #region GetAllCategories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var category = await _iRepository.GetAllCategories();
           // var catmap = _mapper.Map<List<Category>>(category);
            var n = new List<CategoryOutputDto>();
            foreach (var Item in category) 
            {
                var m = new CategoryOutputDto();
                m.CategoryImg = Convert.ToBase64String(Item.CategoryImg);
                m.Name = Item.Name;
                m.Id = Item.Id;
                n.Add(m);
            }
            
            return Ok(n);
        }
        #endregion

        #region PostCategory
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] CategoryInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name))
                return BadRequest("Invalid input data.");
            if (_dbContext.Categories.Where(t => t.Name.Equals(inputDto.Name)).ToList().Count > 0)
            {
                return BadRequest("Category already exists");
            }

            var category = new Category();
            category.Name = inputDto.Name;
            //var m = Tools.FileUpload(inputDto.CategoryImg);

            category.CategoryImg = inputDto.CategoryImg;
            var cat = _mapper.Map<Category>(category);
            _iRepository.Add(cat);
            await _iRepository.Save();
            return Ok();
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm] Category inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name))
                return BadRequest("Invalid input data.");
            if (_dbContext.Categories.Where(t => t.Name.Equals(inputDto.Name)).ToList().Count > 0)
            {
                return BadRequest("Category already exists");
            }

            var category = new Category();
            category.Name = inputDto.Name;
            category.CategoryImg = inputDto.CategoryImg;
            var cat = _mapper.Map<Category>(category);
            _iRepository.Add(cat);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateCategory
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryInputDto inputDto)
        {
            var category = await _iRepository.GetCategory(id);
            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");
            
            category.Name = inputDto.Name;
            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteCategory
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _iRepository.GetCategory(id);
            _iRepository.Delete(category);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}