using Azure.Core;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Implementation;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalFusion.API.Controllers
{
    // https://localhost:7461/api/venue
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueRepository venueRepository;

        public VenueController(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer,Editor,Moderator")]
        public async Task<IActionResult> AddVenue(AddVenueRequestDto request)
        {
            // Map DTO to Domain Model
            var venue = new Venue
            {
                Name = request.Name,
                VenueImageUrl = request.VenueImageUrl,
                Address = request.Address,
                Capacity = request.Capacity
            };

            // Add and save changes
            await venueRepository.CreateAsync(venue);

            // Domain model to DTO
            var response = new VenueDto
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                VenueImageUrl = venue.VenueImageUrl,
                Address = venue.Address,
                Capacity = venue.Capacity
            };

            return Ok(response);
        }

        // GET: /api/venue
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await venueRepository.GetAllAsync();

            //Convert Venue domain model to DTO
            var response = new List<VenueDto>();

            foreach (var venue in venues)
            {
                response.Add(new VenueDto
                {
                    VenueId = venue.VenueId,
                    Name = venue.Name,
                    VenueImageUrl = venue.VenueImageUrl,
                    Address = venue.Address,
                    Capacity = venue.Capacity
                });
            }

            return Ok(response);
        }

        // GET: https://localhost:7461/api/venue/{id}
        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetVenueById([FromRoute] int id)
        {
            var existingVenue = await venueRepository.GetById(id);

            if (existingVenue is null) // would return a 404 error
            {
                return NotFound();
            }

            // Convert to DTO
            var response = new VenueDto
            {
                VenueId = existingVenue.VenueId,
                Name = existingVenue.Name,
                VenueImageUrl = existingVenue.VenueImageUrl,
                Address = existingVenue.Address,
                Capacity = existingVenue.Capacity
            };

            return Ok(response);
        }

        // PUT: https://localhost:7461/api/venue/{id}
        [HttpPut]
        [Authorize(Roles = "Editor,Moderator")]
        [Route("{id:int}")]
        public async Task<IActionResult> EditVenue([FromRoute] int id, UpdateVenueRequestDto request)
        {
            // Convert DTO to Domain Model
            var venue = new Venue
            {
                VenueId = id,
                Name = request.Name,
                VenueImageUrl = request.VenueImageUrl,
                Address = request.Address,
                Capacity = request.Capacity
            };

            venue = await venueRepository.UpdateAsync(venue);

            if (venue == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new VenueDto
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                VenueImageUrl = venue.VenueImageUrl,
                Address = venue.Address,
                Capacity = venue.Capacity
            };

            return Ok(response);
        }

        // DELETE: https://localhost:7033/api/venue/{id}
        [HttpDelete]
        [Authorize(Roles = "Editor")]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveVenue([FromRoute] int id)
        {
            var venue = await venueRepository.DeleteAsync(id);

            if (venue is null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new VenueDto
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                VenueImageUrl = venue.VenueImageUrl,
                Address = venue.Address,
                Capacity = venue.Capacity
            };

            return Ok(response);
        }
    }
}
