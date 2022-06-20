using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("Village")]

    public class VillageController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public VillageController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }
        #region GetVillage
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVillage(int id)
        {
            var village = await _iRepository.GetVillage(id)
;
            var villmap = _mapper.Map<VillageOutputDto>(village);
            return Ok(villmap);
        }
        #endregion

        #region GetAllVillagessByCell
        [HttpGet("ByCell{CellId}")]
        public async Task<IActionResult> GetAllVillagessByCell(int CellId)
        {
            if ((_dbContext.Villages.Where(t => t.cellId.Equals(CellId)).ToList().Count == 0))
            {
                return NotFound("No Villages found ");
            }
            var cells = await _iRepository.GetAllVillageByCell(CellId);
            return Ok(cells);
        }
        #endregion
        #region GetAllVillages
        [HttpGet]
        public async Task<IActionResult> GetAllVillages()
        {
            var village = await _iRepository.GetAllVillages();
            var villmap = _mapper.Map<List<VillageOutputDto>>(village);
            return Ok(villmap);
        }
        #endregion

        #region PostVillage
        [HttpPost]
        public async Task<IActionResult> PostVillage([FromBody] VillageInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.cellId.ToString())) return BadRequest("Invalid input data.");
            if ((_dbContext.Villages.Where(t => t.Name.Equals(inputDto.Name) && t.cellId.Equals(inputDto.cellId)).ToList()).Count > 0)
            {
                return BadRequest("Village already exists");
            }

            var vill = _mapper.Map<Village>(inputDto);
            _iRepository.Add(vill);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateVillage
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVillage(int id, [FromBody] VillageInputDto inputDto)
        {
            var village = await _iRepository.GetVillage(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.cellId.ToString())) return BadRequest("Invalid input data.");


            village.Name = inputDto.Name;
            village.cellId = inputDto.cellId;
            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteVillage
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVillage(int id)
        {
            var village = await _iRepository.GetVillage(id);
            _iRepository.Delete(village);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}