using FestivalFusion.API.Data;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalFusion.API.Controllers
{
    // https://localhost:7164/api/festivals
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        private readonly IFestivalRepository festivalRepository;

        public FestivalController(IFestivalRepository festivalRepository)
        {
            this.festivalRepository = festivalRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFestival(CreateFestivalRequestDto request)
        {
            // Map DTO to Domain Model
            var festival = new Festival
            {
                FestivalName = request.FestivalName,
                FestivalImageUrl = request.FestivalImageUrl,
                FestivalDescription = request.FestivalDescription,
                Theme = request.Theme,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Sponsor = request.Sponsor
            };

            // Add and save changes
            await festivalRepository.CreateAsync(festival);

            // Domain model to DTO
            var response = new FestivalDto
            {
                FestivalId = festival.FestivalId,
                FestivalName = festival.FestivalName,
                FestivalImageUrl = festival.FestivalImageUrl,
                Theme = festival.Theme,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                Sponsor = festival.Sponsor
            };

            return Ok(response);
        }

        // GET: /api/festivals
        [HttpGet]
        public async Task<IActionResult> GetAllFestivals()
        {
            var festivals = await festivalRepository.GetAllAsync();

            //Convert Festival domain model to DTO
            var response = new List<FestivalDto>();
            foreach (var festival in festivals)
            {
                response.Add(new FestivalDto
                {
                    FestivalId = festival.FestivalId,
                    FestivalName = festival.FestivalName,
                    FestivalImageUrl = festival.FestivalImageUrl,
                    FestivalDescription = festival.FestivalDescription,
                    Theme = festival.Theme,
                    StartDate = festival.StartDate,
                    EndDate = festival.EndDate,
                    Sponsor = festival.Sponsor
                });
            }

            return Ok(response);
        }

        // GET: https://localhost:7164/api/festivals/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetFestivalById([FromRoute] int id)
        {
            var existingFestival = await festivalRepository.GetById(id);

            if (existingFestival is null) // would return a 404 error
            {
                return NotFound();
            }

            // Convert to DTO
            var response = new FestivalDto
            {
                FestivalId = existingFestival.FestivalId,
                FestivalName = existingFestival.FestivalName,
                FestivalImageUrl = existingFestival.FestivalImageUrl,
                FestivalDescription = existingFestival.FestivalDescription,
                Theme = existingFestival.Theme,
                StartDate = existingFestival.StartDate,
                EndDate = existingFestival.EndDate,
                Sponsor = existingFestival.Sponsor
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditFestival([FromRoute] int id, UpdateFestivalRequestDto request)
        {
            // Convert DTO to Domain Model
            var festival = new Festival
            {
                FestivalId = id,
                FestivalName = request.FestivalName,
                FestivalImageUrl = request.FestivalImageUrl,
                FestivalDescription = request.FestivalDescription,
                Theme = request.Theme,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Sponsor = request.Sponsor
            };

            festival = await festivalRepository.UpdateAsync(festival);

            if (festival == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new FestivalDto
            {
                FestivalId = id,
                FestivalName = festival.FestivalName,
                FestivalImageUrl = festival.FestivalImageUrl,
                FestivalDescription = festival.FestivalDescription,
                Theme = festival.Theme,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                Sponsor = festival.Sponsor
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteFestival([FromRoute] int id)
        {
            var festival = await festivalRepository.DeleteAsync(id);

            if (festival is null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new FestivalDto
            {
                FestivalId = festival.FestivalId,
                FestivalName = festival.FestivalName,
                FestivalImageUrl = festival.FestivalImageUrl,
                FestivalDescription = festival.FestivalDescription,
                Theme = festival.Theme,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                Sponsor = festival.Sponsor
            };

            return Ok(response);
        }
    }
}
