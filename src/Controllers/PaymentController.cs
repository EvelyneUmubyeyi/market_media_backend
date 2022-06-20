using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public PaymentController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }
        #region GetPayment
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment = await _iRepository.GetPayment(id)
;
            var paymap = _mapper.Map<PaymentOutputDto>(payment);
            return Ok(paymap);
        }
        #endregion

        #region GetAllPayments
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payment = await _iRepository.GetAllPayments();
            var paymap = _mapper.Map<List<PaymentOutputDto>>(payment);
            return Ok(paymap);
        }
        #endregion

        #region PostPayment
        [HttpPost]
        public async Task<IActionResult> PostPayment([FromBody] PaymentInputDto inputDto)
        {
            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");
            if ((_dbContext.payments.Where(t => t.Name.Equals(inputDto.Name) && t.Description.Equals(inputDto.Description)).ToList()).Count > 0)
            {
                return BadRequest("payment already exists");
            }

            var pay = _mapper.Map<Payment>(inputDto);
            _iRepository.Add(pay);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdatePayment
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, [FromBody] PaymentInputDto inputDto)
        {
            var payment = await _iRepository.GetPayment(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name)) return BadRequest("Invalid input data.");
                       
            payment.Name = inputDto.Name;
            
            if (inputDto.Description != null)
            {
                payment.Description = inputDto.Description;
            }
            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeletePayment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _iRepository.GetPayment(id)
;            _iRepository.Delete(payment);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}