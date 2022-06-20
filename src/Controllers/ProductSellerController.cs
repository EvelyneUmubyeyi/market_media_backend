using AutoMapper;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketMed.src.Controllers
{
    [Route("ProductSeller")]
    public class ProductSellerController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;
        private IMapper _mapper;
        public ProductSellerController(IRepository Repository, MMDbContext mmDbContext, IMapper mapper)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
            _mapper = mapper;
        }
        #region GetProductSeller
        [HttpGet("{productId}/{sellerId}")]
        public async Task<IActionResult> GetProductSeller(int productId, int sellerId)
        {
            var productSeller = await _iRepository.GetProductSeller(productId, sellerId)
;
            var psmap = _mapper.Map<ProductSellerOutputDto>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetAllProductSellers
        [HttpGet]
        public async Task<IActionResult> GetAllProductSellers()
        {
            var productSeller = await _iRepository.GetAllProductSellers();
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetAllProductsOnDiscount
        [HttpGet("ProductsOnDiscount")]
        public async Task<IActionResult> GetAllProductsOnDiscount()
        {
            var productSeller = await _iRepository.GetAllProductsOnDiscount();
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetAllProductsOnPromotion
        [HttpGet("ProductsOnPromotion")]
        public async Task<IActionResult> GetAllProductsOnPromotion()
        {
            var productSeller = await _iRepository.GetAllProductsOnPromotion();
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetProductOnDiscount
        [HttpGet("ProductOnDiscount/{productId}")]
        public async Task<IActionResult> GetProductOnDiscount(int productId)
        {
            var productSeller = await _iRepository.GetProductOnDiscount(productId);
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetProductOnPromotion
        [HttpGet("ProductOnPromotion/{productId}")]
        public async Task<IActionResult> GetProductOnPromotion(int productId)
        {
            var productSeller = await _iRepository.GetProductOnPromotion(productId);
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetProductByPrice
        [HttpGet("GetProductByPrice")]
        public async Task<IActionResult> GetProductByPrice(int productId, decimal lowBound, decimal highBound)
        {
            var productSeller = await _iRepository.GetAllProductsByPrice(productId, lowBound, highBound);
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetAllProductsBySeller
        [HttpGet("BySeller/{sellerId}")]
        public async Task<IActionResult> GetAllProductsBySellers(int sellerId)
        {
            var productSeller = await _iRepository.GetAllProductsBySeller(sellerId);
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region GetAllSellersByProduct
        [HttpGet("ByProduct/{productId}")]
        public async Task<IActionResult> GetAllSellersByProduct(int productId)
        {
            var productSeller = await _iRepository.GetAllsellersByProduct(productId);
            var psmap = _mapper.Map<List<ProductSellerOutputDto>>(productSeller);
            return Ok(psmap);
        }
        #endregion

        #region PostProductSeller
        [HttpPost]
        public async Task<IActionResult> PostProductSeller([FromBody] ProductSellerInputDto inputDto)
        {
            if (string.IsNullOrWhiteSpace(inputDto.ProductId.ToString()) || string.IsNullOrWhiteSpace(inputDto.ProductId.ToString())) return BadRequest("Invalid input data.");
            if ((_dbContext.ProductSellers.Where(t => t.ProductId.Equals(inputDto.ProductId) && t.SellerId.Equals(inputDto.SellerId)).ToList()).Count > 0)
            {
                return BadRequest("That product is already registered under that seller");
            }
            if(inputDto.discount<0 || inputDto.discount.ToString() == "" || string.IsNullOrWhiteSpace(inputDto.ProductId.ToString()))
            {
                inputDto.discount = 0;
            }
            var ps = _mapper.Map<ProductSeller>(inputDto);
            ps.total_price = inputDto.price-(inputDto.price * inputDto.discount / 100);
            _iRepository.Add(ps);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateProductSeller
        [HttpPut("{productId}/{sellerId}")]
        public async Task<IActionResult> PutProductSeller(int sellerId, int productId, [FromBody] ProductSellerInputDto inputDto)
        {
            var productSeller = await _iRepository.GetProductSeller(productId, sellerId)
;
            if (string.IsNullOrWhiteSpace(inputDto.ProductId.ToString()) || string.IsNullOrWhiteSpace(inputDto.SellerId.ToString())) return BadRequest("Invalid input data.");

            productSeller.ProductId = inputDto.ProductId;
            productSeller.SellerId = inputDto.SellerId;


            productSeller.price = inputDto.price;
            productSeller.discount = inputDto.discount;
            
            if (inputDto.quantity_measurement != null)
            {
                productSeller.quantity_measurement = inputDto.quantity_measurement;
            }

            productSeller.quantity = inputDto.quantity;

            if (inputDto.promotion != null)
            {
                productSeller.promotion = inputDto.promotion;
            }
            productSeller.total_price = inputDto.price - (inputDto.price* inputDto.discount/100);

            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteProductSeller
        [HttpDelete("{productId}/{sellerId}")]
        public async Task<IActionResult> DeleteProductSeller(int sellerId, int productId)
        {
            var ps = await _iRepository.GetProductSeller(productId,sellerId);
            _iRepository.Delete(ps);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}