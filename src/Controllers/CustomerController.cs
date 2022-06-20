using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("Customer")]

    public class CustomerController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public CustomerController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }

        #region GetCustomer
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _iRepository.GetCustomer(id);
            var custmap = _mapper.Map<CustomerOutputDto>(customer);
            return Ok(custmap);
        }
        #endregion

        #region GetCustomersByAddress
        [HttpGet("ByAddress{AddressId}")]
        public async Task<IActionResult> GetCustomersByAddress(int AddressId)
        {
            if ((_dbContext.Customers.Where(t => t.AddressId.Equals(AddressId)).ToList().Count == 0))
            {
                return NotFound("No customer found in that address");
            }
            var Customers = await _iRepository.GetCustomersByAddress(AddressId);
            return Ok(Customers);
        }
        #endregion

        #region GetAllCustomers
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var Customer = await _iRepository.GetAllCustomers();
            var custmap = _mapper.Map<List<CustomerOutputDto>>(Customer);
            return Ok(custmap);
        }
        #endregion

        #region PostCustomer
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] CustomerInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.ContactId.ToString())) return BadRequest("Invalid input data.");
            if ((_dbContext.Customers.Where(t => t.Name.Equals(inputDto.Name) && t.AddressId.Equals(inputDto.AddressId) && t.ContactId.Equals(inputDto.ContactId)).ToList()).Count > 0)
            {
               return BadRequest("product already exists");
            }

            var cust = _mapper.Map<Customer>(inputDto);
            _iRepository.Add(cust);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateCustomer
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromBody] CustomerInputDto inputDto)
        {
            var customer = await _iRepository.GetCustomer(id)
;
            if (string.IsNullOrWhiteSpace(inputDto.Name) && string.IsNullOrWhiteSpace(inputDto.ContactId.ToString())) return BadRequest("Invalid input data.");

           
            customer.Name = inputDto.Name;
            customer.ContactId = inputDto.ContactId; 
            customer.AddressId = inputDto.AddressId;
            
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region DeleteCustomer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _iRepository.GetCustomer(id);;
            _iRepository.Delete(customer);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}