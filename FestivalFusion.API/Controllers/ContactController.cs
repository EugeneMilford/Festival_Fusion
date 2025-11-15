using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Repositories.Interface;

namespace FestivalFusion.API.Controllers
{
    // https://localhost:7461/api/contact
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository contactRepository;

        public ContactController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        // POST: api/contact
        // Creates a new contact message
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] AddContactRequestDto request)
        {
            if (request is null)
                return BadRequest();

            var contact = new Contact
            {
                Name = request.Name,
                Surname = request.Surname,
                PhoneNumber = request.PhoneNumber,
                Message = request.Message
            };

            contact = await contactRepository.CreateAsync(contact);

            var response = new ContactDto
            {
                ContactId = contact.ContactId,
                Name = contact.Name,
                Surname = contact.Surname,
                PhoneNumber = contact.PhoneNumber,
                Message = contact.Message
            };

            // Return 201 with the created resource payload (location could be added when a GetById route exists)
            return CreatedAtAction(nameof(GetAllContacts), null, response);
        }

        // GET: api/contact
        // Returns all contact messages
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await contactRepository.GetAllAsync();

            var response = contacts
                .Select(c => new ContactDto
                {
                    ContactId = c.ContactId,
                    Name = c.Name,
                    Surname = c.Surname,
                    PhoneNumber = c.PhoneNumber,
                    Message = c.Message
                });

            return Ok(response);
        }

        // DELETE: api/contact/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveContact([FromRoute] int id)
        {
            var contact = await contactRepository.DeleteAsync(id);

            if (contact is null)
            {
                return NotFound();
            }

            var response = new ContactDto
            {
                ContactId = contact.ContactId,
                Name = contact.Name,
                Surname = contact.Surname,
                PhoneNumber = contact.PhoneNumber,
                Message = contact.Message
            };

            return Ok(response);
        }
    }
}
