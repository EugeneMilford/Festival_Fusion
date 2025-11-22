using FestivalFusion.API.Controllers;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FestivalFusion.Tests.Controllers
{
    public class FestivalControllerTests
    {
        private readonly Mock<IFestivalRepository> festivalRepoMock;
        private readonly FestivalController controller;

        public FestivalControllerTests()
        {
            festivalRepoMock = new Mock<IFestivalRepository>();
            controller = new FestivalController(festivalRepoMock.Object);
        }

        [Fact]
        public async Task CreateFestival_ReturnsOkWithCreatedDto()
        {
            var request = new CreateFestivalRequestDto
            {
                FestivalName = "Test Fest",
                FestivalImageUrl = "http://img",
                FestivalDescription = "desc",
                Theme = "theme",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Sponsor = "Sponsor"
            };

            // IFestivalRepository.CreateAsync returns Task<Festival>, so return the same Festival instance
            // and simulate the repository assigning an Id (as EF would).
            festivalRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Festival>()))
                .ReturnsAsync((Festival f) =>
                {
                    f.FestivalId = 1;
                    return f;
                });

            var result = await controller.CreateFestival(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FestivalDto>(ok.Value);
            Assert.Equal(1, dto.FestivalId);
            Assert.Equal(request.FestivalName, dto.FestivalName);
            Assert.Equal(request.Theme, dto.Theme);
        }

        [Fact]
        public async Task GetAllFestivals_ReturnsOkWithList()
        {
            var festivals = new List<Festival>
            {
                new Festival
                {
                    FestivalId = 1,
                    FestivalName = "F1",
                    FestivalImageUrl = "img1",
                    FestivalDescription = "d1",
                    Theme = "t1",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(1),
                    Sponsor = "s1"
                },
                new Festival
                {
                    FestivalId = 2,
                    FestivalName = "F2",
                    FestivalImageUrl = "img2",
                    FestivalDescription = "d2",
                    Theme = "t2",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(2),
                    Sponsor = "s2"
                }
            };

            festivalRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(festivals);

            var result = await controller.GetAllFestivals();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<FestivalDto>>(ok.Value);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task GetFestivalById_Found_ReturnsOk()
        {
            var festival = new Festival
            {
                FestivalId = 10,
                FestivalName = "FoundFest",
                FestivalImageUrl = "img",
                FestivalDescription = "desc",
                Theme = "theme",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Sponsor = "s"
            };

            festivalRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(festival);

            var result = await controller.GetFestivalById(10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FestivalDto>(ok.Value);
            Assert.Equal(festival.FestivalId, dto.FestivalId);
            Assert.Equal(festival.FestivalName, dto.FestivalName);
        }

        [Fact]
        public async Task GetFestivalById_NotFound_ReturnsNotFound()
        {
            festivalRepoMock.Setup(r => r.GetById(999)).ReturnsAsync((Festival?)null);

            var result = await controller.GetFestivalById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditFestival_Success_ReturnsOk()
        {
            var updateRequest = new UpdateFestivalRequestDto
            {
                FestivalName = "Updated",
                FestivalImageUrl = "img-upd",
                FestivalDescription = "desc-upd",
                Theme = "theme-upd",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2),
                Sponsor = "s-upd"
            };

            var updatedFestival = new Festival
            {
                FestivalId = 5,
                FestivalName = updateRequest.FestivalName,
                FestivalImageUrl = updateRequest.FestivalImageUrl,
                FestivalDescription = updateRequest.FestivalDescription,
                Theme = updateRequest.Theme,
                StartDate = updateRequest.StartDate,
                EndDate = updateRequest.EndDate,
                Sponsor = updateRequest.Sponsor
            };

            festivalRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Festival>()))
                .ReturnsAsync(updatedFestival);

            var result = await controller.EditFestival(5, updateRequest);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FestivalDto>(ok.Value);
            Assert.Equal(5, dto.FestivalId);
            Assert.Equal(updateRequest.FestivalName, dto.FestivalName);
        }

        [Fact]
        public async Task EditFestival_NotFound_ReturnsNotFound()
        {
            festivalRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Festival>()))
                .ReturnsAsync((Festival?)null);

            var updateRequest = new UpdateFestivalRequestDto
            {
                FestivalName = "X",
                FestivalImageUrl = "img",
                FestivalDescription = "desc",
                Theme = "t",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Sponsor = "s"
            };

            var result = await controller.EditFestival(1234, updateRequest);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFestival_Success_ReturnsOk()
        {
            var festival = new Festival
            {
                FestivalId = 7,
                FestivalName = "ToDelete",
                FestivalImageUrl = "img",
                FestivalDescription = "desc",
                Theme = "t",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Sponsor = "s"
            };

            festivalRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(festival);

            var result = await controller.DeleteFestival(7);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<FestivalDto>(ok.Value);
            Assert.Equal(7, dto.FestivalId);
        }

        [Fact]
        public async Task DeleteFestival_NotFound_ReturnsNotFound()
        {
            // Some repository implementations return nullable; return null to simulate not found.
            festivalRepoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync((Festival)null);

            var result = await controller.DeleteFestival(42);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
