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
                Theme = festival.Theme,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                Sponsor = festival.Sponsor
            };

            return Ok(response);
        }
    }
}
