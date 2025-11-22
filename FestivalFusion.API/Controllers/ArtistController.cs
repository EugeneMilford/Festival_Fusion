using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalFusion.API.Controllers
{
    // https://localhost:7461/api/artists
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistRepository artistRepository;

        public ArtistController(IArtistRepository artistRepository)
        {
            this.artistRepository = artistRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer,Editor,Moderator")]
        public async Task<IActionResult> AddArtist(AddArtistRequestDto request)
        {
            // Map DTO to Domain Model
            var artist = new Artist
            {
                Name = request.Name,
                ArtistImageUrl = request.ArtistImageUrl,
                Genre = request.Genre,
                Country = request.Country,
                Bio = request.Bio
            };

            // Add and save changes
            await artistRepository.CreateAsync(artist);

            // Domain model to DTO
            var response = new ArtistDto
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name,
                ArtistImageUrl = artist.ArtistImageUrl,
                Genre = artist.Genre,
                Country = artist.Country,
                Bio = artist.Bio
            };
            return Ok(response);
        }

        // GET: /api/artists
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await artistRepository.GetAllAsync();

            //Convert Artist domain model to DTO
            var response = new List<ArtistDto>();

            foreach (var artist in artists)
            {
                response.Add(new ArtistDto
                {
                    ArtistId = artist.ArtistId,
                    Name = artist.Name,
                    ArtistImageUrl = artist.ArtistImageUrl,
                    Genre = artist.Genre,
                    Country = artist.Country,
                    Bio = artist.Bio
                });
            }

            return Ok(response);
        }

        // GET: https://localhost:7461/api/artists/{id}
        [HttpGet]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> GetArtistById([FromRoute] int id)
        {
            var existingArtist = await artistRepository.GetById(id);

            if (existingArtist is null) // would return a 404 error
            {
                return NotFound();
            }

            // Convert to DTO
            var response = new ArtistDto
            {
                ArtistId = existingArtist.ArtistId,
                Name = existingArtist.Name,
                ArtistImageUrl = existingArtist.ArtistImageUrl,
                Genre = existingArtist.Genre,
                Country = existingArtist.Country,
                Bio = existingArtist.Bio
            };

            return Ok(response);
        }

        // PUT: /api/artists/{id}
        [HttpPut]
        [Authorize(Roles = "Editor,Moderator")]
        [Route("{id:int}")]
        public async Task<IActionResult> EditArtist([FromRoute] int id, UpdateArtistRequestDto request)
        {
            // Convert DTO to Domain Model
            var artist = new Artist
            {
                ArtistId = id,
                Name = request.Name,
                ArtistImageUrl = request.ArtistImageUrl,
                Genre = request.Genre,
                Country = request.Country,
                Bio = request.Bio
            };

            artist = await artistRepository.UpdateAsync(artist);

            if (artist == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new ArtistDto
            {
                ArtistId = id,
                Name = artist.Name,
                ArtistImageUrl = artist.ArtistImageUrl,
                Genre = artist.Genre,
                Country = artist.Country,
                Bio = artist.Bio
            };

            return Ok(response);
        }

        // DELETE: /api/artists/{id}
        [HttpDelete]
        [Authorize(Roles = "Editor")]
        [Route("{id:int}")]
        public async Task<IActionResult> RemoveArtist([FromRoute] int id)
        {
            var artist = await artistRepository.DeleteAsync(id);

            if (artist is null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new ArtistDto
            {
                ArtistId = artist.ArtistId,
                Name = artist.Name,
                ArtistImageUrl = artist.ArtistImageUrl,
                Genre = artist.Genre,
                Country = artist.Country,
                Bio = artist.Bio
            };

            return Ok(response);
        }
    }
}
