using FestivalFusion.API.Models.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Implementation;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalFusion.API.Controllers
{
    // https://localhost:7461/api/sponsor
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorRepository sponsorRepository;

        public SponsorController(ISponsorRepository sponsorRepository)
        {
            this.sponsorRepository = sponsorRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddSponsor(AddSponsorRequestDto request)
        {
            // Map DTO to Domain Model
            var sponsor = new Sponsor
            {
                Name = request.Name,
                ContactEmail = request.ContactEmail,
                Phone = request.Phone,
                Website = request.Website
            };

            // Add and save changes
            await sponsorRepository.CreateAsync(sponsor);

            // Domain model to DTO
            var response = new SponsorDto
            {
                SponsorId = sponsor.SponsorId,
                Name = sponsor.Name,
                ContactEmail = sponsor.ContactEmail,
                Phone = sponsor.Phone,
                Website = sponsor.Website
            };
            return Ok(response);
        }

        // GET: /api/sponsors
        [HttpGet]
        public async Task<IActionResult> GetAllSponsors()
        {
            var sponsors = await sponsorRepository.GetAllAsync();

            //Convert Festival domain model to DTO
            var response = new List<SponsorDto>();

            foreach (var sponsor in sponsors)
            {
                response.Add(new SponsorDto
                {
                    SponsorId = sponsor.SponsorId,
                    Name = sponsor.Name,
                    ContactEmail = sponsor.ContactEmail,
                    Phone = sponsor.Phone,
                    Website = sponsor.Website
                });
            }

            return Ok(response);
        }

        // GET: https://localhost:7461/api/sponsors/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetSponsorById([FromRoute] int id)
        {
            var existingSponsor = await sponsorRepository.GetById(id);

            if (existingSponsor is null) // would return a 404 error
            {
                return NotFound();
            }

            // Convert to DTO
            var response = new SponsorDto
            {
                SponsorId = existingSponsor.SponsorId,
                Name = existingSponsor.Name,
                ContactEmail = existingSponsor.ContactEmail,
                Phone = existingSponsor.Phone,
                Website = existingSponsor.Website
            };

            return Ok(response);
        }

        // PUT: https://localhost:7461/api/sponsors/{id}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditSponsor([FromRoute] int id, UpdateSponsorRequestDto request)
        {
            // Convert DTO to Domain Model
            var sponsor = new Sponsor
            {
                SponsorId = id,
                Name = request.Name,
                ContactEmail = request.ContactEmail,
                Phone = request.Phone,
                Website = request.Website
            };

            sponsor = await sponsorRepository.UpdateAsync(sponsor);

            if (sponsor == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new SponsorDto
            {
                SponsorId = sponsor.SponsorId,
                Name = sponsor.Name,
                ContactEmail = sponsor.ContactEmail,
                Phone = sponsor.Phone,
                Website = sponsor.Website
            };

            return Ok(response);
        }

        // DELETE: https://localhost:7461/api/sponsors/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveSponsor([FromRoute] int id)
        {
            var sponsor = await sponsorRepository.DeleteAsync(id);

            if (sponsor is null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new SponsorDto
            {
                SponsorId = sponsor.SponsorId,
                Name = sponsor.Name,
                ContactEmail = sponsor.ContactEmail,
                Phone = sponsor.Phone,
                Website = sponsor.Website
            };

            return Ok(response);
        }
    }
}
