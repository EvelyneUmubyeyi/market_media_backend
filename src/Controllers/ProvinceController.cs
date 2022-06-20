using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("Province")]

    public class ProvinceController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public ProvinceController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }
        #region GetProvince
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProvince(int id)
        {
            var province = await _iRepository.GetProvince(id)
;
            var promap = _mapper.Map<ProvinceOutputDto>(province);
            return Ok(promap);
        }
        #endregion

        #region GetAllProvinces
        [HttpGet]
        public async Task<IActionResult> GetAllProvinces()
        {
            var province = await _iRepository.GetAllProvinces();
            var promap = _mapper.Map<List<ProvinceOutputDto>>(province);
            return Ok(promap);
        }
        #endregion

        #region PostProvince
        [HttpPost]
        public async Task<IActionResult> PostProvince([FromBody] ProvinceInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");
            if ((_dbContext.Provinces.Where(t => t.Name.Equals(inputDto.Name)).ToList()).Count > 0)
            {
                return BadRequest("Province already exists");
            }

            var pro = _mapper.Map<Province>(inputDto);
            _iRepository.Add(pro);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateProvince
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvince(int id, [FromBody] ProvinceInputDto inputDto)
        {
            var province = await _iRepository.GetProvince(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");

            province.Name = inputDto.Name;
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region DeleteProvince
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvince(int id)
        {
            var province = await _iRepository.GetProvince(id);
            _iRepository.Delete(province);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}