using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("Order")]

    public class OrderController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public OrderController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _iRepository.GetOrder(id)
;           var ordmap = _mapper.Map<OrderOutputDto>(order);
            return Ok(ordmap);
        }

        [HttpGet("ByCustomer{CustomerId}")]
        public async Task<IActionResult> GetAllOrdersByCustomer(int CustomerId)
        {
            if ((_dbContext.Orders.Where(t => t.CustomerId.Equals(CustomerId)).ToList().Count == 0))
           {
                return NotFound("No orders by that customer were found ");
            }
            var orders = await _iRepository.GetAllOrdersByCustomer(CustomerId);
            return Ok(orders);
        }

        [HttpGet("ByProduct{CustomerId}")]
        public async Task<IActionResult> GetAllOrdersByProduct(int productId)
        {
            if ((_dbContext.Orders.Where(t => t.ProductId.Equals(productId)).ToList().Count == 0))
            {
                return NotFound("Orders of that product not  found ");
            }
            var orders = await _iRepository.GetAllOrdersByProduct(productId);
            return Ok(orders);
        }

        [HttpGet("ByPayment{PaymentId}")]
        public async Task<IActionResult> GetAllOrdersByPayment(int PaymentId)
        {
            if ((_dbContext.Orders.Where(t => t.PaymentId.Equals(PaymentId)).ToList().Count == 0))
            {
                return NotFound("Orders using that payment mode not  found ");
            }
            var orders = await _iRepository.GetAllOrdersByPayment(PaymentId);
            return Ok(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var order = await _iRepository.GetAllOrders();
            var ordmap = _mapper.Map<List<OrderOutputDto>>(order);
            return Ok(ordmap);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderInputDto inputDto)
        {
            if (string.IsNullOrWhiteSpace(inputDto.CustomerId.ToString()) || string.IsNullOrWhiteSpace(inputDto.ProductId.ToString()) || string.IsNullOrWhiteSpace(inputDto.PaymentId.ToString())) return BadRequest("Invalid input data.");
            
            var order = new Order();

            order.ProductId = inputDto.ProductId;
            order.PaymentId = inputDto.PaymentId;
            order.CustomerId = inputDto.CustomerId;
            order.OrderTime = DateTime.Now; 
            _iRepository.Add(order);
            await _iRepository.Save();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] OrderInputDto inputDto)
        {
            if (string.IsNullOrWhiteSpace(inputDto.ProductId.ToString()) || string.IsNullOrWhiteSpace(inputDto.PaymentId.ToString()) || string.IsNullOrWhiteSpace(inputDto.CustomerId.ToString()))
                return BadRequest("Invalid input data.");
            var order = await _iRepository.GetOrder(id)
;
            order.ProductId = inputDto.ProductId;
            order.PaymentId = inputDto.PaymentId;
            order.CustomerId = inputDto.CustomerId;
            order.OrderTime = inputDto.OrderTime;
            await _iRepository.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _iRepository.GetOrder(id)
;           _iRepository.Delete(order);
            await _iRepository.Save();
            return Ok();
        }

    }
}