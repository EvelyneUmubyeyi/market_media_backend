using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;
namespace MarketMedia.src.Controllers
{
    [Route("Seller")]
    public class SellerController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;

        public SellerController(IRepository Repository, MMDbContext mmDbContext)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
        }

        #region GetSeller(id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeller(int id)
        {
            var seller = await _iRepository.GetSeller(id);
            return Ok(seller);
        }
        #endregion

        #region GetAllSellers
        [HttpGet]
        public async Task<IActionResult> GetAllSellers()
        {
            var seller = await _iRepository.GetAllSellers();
            return Ok(seller);
        }
        #endregion

        #region PostSeller
        [HttpPost]
        public async Task<IActionResult> PostSeller([FromBody] SellerInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");
            if (_dbContext.Sellers.Where(t => t.Name.Equals(inputDto.Name)&& t.TIN_number.Equals(inputDto.TIN_number) && t.mobilePaymentCode.Equals(inputDto.mobilePaymentCode) && t.about.Equals(inputDto.about) && t.website.Equals(inputDto.website) && t.socialmedia.Equals(inputDto.socialmedia)).ToList().Count > 0)
            {
                return BadRequest("Seller already exists");
            }

            var seller = new Seller();
            seller.Name = inputDto.Name;
            seller.TIN_number = inputDto.TIN_number;
            if (inputDto.mobilePaymentCode != null)
            {
                seller.mobilePaymentCode = inputDto.mobilePaymentCode;
            }
            if (inputDto.about != null)
            {
                seller.mobilePaymentCode = inputDto.mobilePaymentCode;
            }
            if (inputDto.about != null)
            {
                seller.about = inputDto.about;
            }
            if (inputDto.website != null)
            {
                seller.website = inputDto.website;
            }
            if (inputDto.socialmedia != null)
            {
                seller.socialmedia = inputDto.socialmedia;
            }

            _iRepository.Add(seller);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateSeller
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeller(int id, [FromBody] SellerInputDto inputDto)
        {
            var seller = await _iRepository.GetSeller(id);
            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");

            seller.Name = inputDto.Name;
            seller.TIN_number = inputDto.TIN_number;
            if (inputDto.mobilePaymentCode != null)
            {
                seller.mobilePaymentCode = inputDto.mobilePaymentCode;
            }
            if (inputDto.about != null)
            {
                seller.mobilePaymentCode = inputDto.mobilePaymentCode;
            }
            if (inputDto.about != null)
            {
                seller.about = inputDto.about;
            }
            if (inputDto.website != null)
            {
                seller.website = inputDto.website;
            }
            if (inputDto.socialmedia != null)
            {
                seller.socialmedia = inputDto.socialmedia;
            }
            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteSeller
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var seller = await _iRepository.GetSeller(id);
            _iRepository.Delete(seller);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}