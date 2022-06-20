using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("Sector")]

    public class SectorController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public SectorController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }
        #region GetSector
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSector(int id)
        {
            var sector = await _iRepository.GetSector(id);
            var sectmap = _mapper.Map<SectorOutputDto>(sector);
            return Ok(sectmap);
        }
        #endregion
        #region GetAllSectorsByDistrict
        [HttpGet("ByDistrict{districtId}")]
        public async Task<IActionResult> GetAllSectorsByDistrict(int districtId)
        {
            if ((_dbContext.Sectors.Where(t => t.DistrictId.Equals(districtId)).ToList().Count == 0))
            {
                return NotFound("No sectors found ");
            }
            var sectors = await _iRepository.GetAllSectorsByDistrict(districtId);
            return Ok(sectors);
        }
        #endregion
        #region GetAllSectors
        [HttpGet]
        public async Task<IActionResult> GetAllSectors()
        {
            var sector = await _iRepository.GetAllSectors();
            var sectmap = _mapper.Map<List<SectorOutputDto>>(sector);
            return Ok(sectmap);
        }
        #endregion

        #region PostSector
        [HttpPost]
        public async Task<IActionResult> PostSector([FromBody] SectorInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.DistrictId.ToString())) return BadRequest("Invalid input data.");
            if ((_dbContext.Sectors.Where(t => t.Name.Equals(inputDto.Name) && t.DistrictId.Equals(inputDto.DistrictId)).ToList()).Count > 0)
            {
                return BadRequest("Sector already exists");
            }

            var sect = _mapper.Map<Sector>(inputDto);
            _iRepository.Add(sect);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateSector
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSector(int id, [FromBody] SectorInputDto inputDto)
        {
            var sector = await _iRepository.GetSector(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.DistrictId.ToString())) return BadRequest("Invalid input data.");


            sector.Name = inputDto.Name;
            sector.DistrictId = inputDto.DistrictId;
            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteSector
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSector(int id)
        {
            var sector = await _iRepository.GetSector(id);
            _iRepository.Delete(sector);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}