using Microsoft.AspNetCore.Mvc;
using MarketMedia.src.EF;
using MarketMedia.src.Entities;
using MarketMedia.src.Models;
using MarketMedia.src.Services;

namespace MarketMedia.src.Controllers
{
    [Route("Contact")]
    public class ContactController : Controller
    {
        private readonly IRepository _iRepository;
        private MMDbContext _dbContext;

        public ContactController(IRepository Repository, MMDbContext mmDbContext)
        {
            _iRepository = Repository;
            _dbContext = mmDbContext;
        }

        #region GetContact(id)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            if ((_dbContext.Contacts.Where(t => t.Id.Equals(id)).ToList().Count == 0))
            {
                return NotFound("Contact not found");
            }
            var contact = await _iRepository.GetContact(id);
            return Ok(contact);
        }
        #endregion

        #region GetAllContacts
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _iRepository.GetAllContacts();
            return Ok(contacts);
        }
        #endregion

        #region PostContact
        [HttpPost]
        public async Task<IActionResult> PostContact([FromBody] ContactInputDto inputDto)
        {

            if (string.IsNullOrWhiteSpace(inputDto.Email)) return BadRequest("Invalid input data.");
            if ((_dbContext.Contacts.Where(t => t.Email.Contains(inputDto.Email)
                                        && t.phone.Contains(inputDto.phone))
                                        .ToList()).Count > 0)
            {
                return BadRequest("Contact is already recorded");
            }

            var contact = new Contact();
            if (inputDto.Email != null)
            {
                if(contact.Email != null)
                {
                    contact.Email = contact.Email + "," + inputDto.Email;
                }
                else if (contact.Email == null)
                {
                    contact.Email = inputDto.Email;
                }
            }

            if (inputDto.phone != null)
            {
                if (contact.phone != null)
                {
                    contact.phone = contact.phone + "," + inputDto.phone;
                }
                else if (contact.phone == null)
                {
                    contact.phone = inputDto.phone;
                }
            }

            _iRepository.Add(contact);
            await _iRepository.Save();
            return Ok();
        }
        #endregion

        #region UpdateContact
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, [FromBody] ContactInputDto inputDto)
        {
            if (string.IsNullOrWhiteSpace(inputDto.Email) && string.IsNullOrWhiteSpace(inputDto.phone))
                return BadRequest("Invalid input data.");

            var contact = await _iRepository.GetContact(id);
            if (inputDto.Email != null)
            {
                    contact.Email = contact.Email + "," + inputDto.Email;
            }

            if (inputDto.phone != null)
            {                
                    contact.phone = contact.phone + "," + inputDto.phone;
            }

            await _iRepository.Save();

            return Ok();
        }
        #endregion

        #region DeleteContact
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _iRepository.GetContact(id);
            _iRepository.Delete(contact);
            await _iRepository.Save();
            return Ok();
        }
        #endregion
    }
}

