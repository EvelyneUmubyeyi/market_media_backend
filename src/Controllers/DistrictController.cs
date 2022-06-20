using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("District")]

    public class DistrictController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public DistrictController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }
        #region GetDistrict
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistrict(int id)
        {
            var District = await _iRepository.GetDistrict(id)
;
            var distmap = _mapper.Map<DistrictOutputDto>(District);
            return Ok(distmap);
        }
        #endregion
        #region GetAllDistrictsByProvince
        [HttpGet("ByProvince{ProvinceId}")]
        public async Task<IActionResult> GetAllDistrictsByProvince(int ProvinceId)
        {
            if ((_dbContext.Districts.Where(t => t.ProvinceId.Equals(ProvinceId)).ToList().Count == 0))
            {
                return NotFound("No district found ");
            }
            var districts = await _iRepository.GetAllDistrictsByProvince(ProvinceId);
            return Ok(districts);
        }
        #endregion
        #region GetAllDistricts
        [HttpGet]
        public async Task<IActionResult> GetAllDistricts()
        {
            var district = await _iRepository.GetAllDistricts();
            var distmap = _mapper.Map<List<DistrictOutputDto>>(district);
            return Ok(distmap);
        }
        #endregion

        #region PostDistrict
        [HttpPost]
        public async Task<IActionResult> PostDistrict([FromBody] DistrictInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.ProvinceId.ToString())) return BadRequest("Invalid input data.");
            if ((_dbContext.Districts.Where(t => t.Name.Equals(inputDto.Name) && t.ProvinceId.Equals(inputDto.ProvinceId)).ToList()).Count > 0)
            {
                return BadRequest("district already exists");
            }

            var dist = _mapper.Map<District>(inputDto);
            _iRepository.Add(dist);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateDistrict
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistrict(int id, [FromBody] DistrictInputDto inputDto)
        {
            var district = await _iRepository.GetDistrict(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.ProvinceId.ToString())) return BadRequest("Invalid input data.");

           
            district.Name = inputDto.Name;
            district.ProvinceId = inputDto.ProvinceId;
            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteDistrict
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistrict(int id)
        {
            var district = await _iRepository.GetDistrict(id)
;            _iRepository.Delete(district);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}