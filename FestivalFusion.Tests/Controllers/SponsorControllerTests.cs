using FestivalFusion.API.Controllers;
using FestivalFusion.API.Modals.Domain;
using FestivalFusion.API.Models.Domain;
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
    public class SponsorControllerTests
    {
        private readonly Mock<ISponsorRepository> sponsorRepoMock;
        private readonly SponsorController controller;

        public SponsorControllerTests()
        {
            sponsorRepoMock = new Mock<ISponsorRepository>();
            controller = new SponsorController(sponsorRepoMock.Object);
        }

        [Fact]
        public async Task AddSponsor_ReturnsOkWithCreatedDto()
        {
            var request = new AddSponsorRequestDto
            {
                Name = "Test Sponsor",
                ContactEmail = "sponsor@example.com",
                Phone = "123-456",
                Website = "https://example.com"
            };

            sponsorRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Sponsor>()))
                .ReturnsAsync((Sponsor s) =>
                {
                    s.SponsorId = 1;
                    return s;
                });

            var result = await controller.AddSponsor(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<SponsorDto>(ok.Value);
            Assert.Equal(1, dto.SponsorId);
            Assert.Equal(request.Name, dto.Name);
            Assert.Equal(request.ContactEmail, dto.ContactEmail);
        }

        [Fact]
        public async Task GetAllSponsors_ReturnsOkWithList()
        {
            var sponsors = new List<Sponsor>
            {
                new Sponsor { SponsorId = 1, Name = "S1", ContactEmail = "a@a", Phone = "111", Website = "w1" },
                new Sponsor { SponsorId = 2, Name = "S2", ContactEmail = "b@b", Phone = "222", Website = "w2" }
            };

            sponsorRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(sponsors);

            var result = await controller.GetAllSponsors();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<SponsorDto>>(ok.Value);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task GetSponsorById_Found_ReturnsOk()
        {
            var sponsor = new Sponsor
            {
                SponsorId = 10,
                Name = "FoundSponsor",
                ContactEmail = "found@example.com",
                Phone = "999",
                Website = "w"
            };

            sponsorRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(sponsor);

            var result = await controller.GetSponsorById(10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<SponsorDto>(ok.Value);
            Assert.Equal(sponsor.SponsorId, dto.SponsorId);
            Assert.Equal(sponsor.Name, dto.Name);
        }

        [Fact]
        public async Task GetSponsorById_NotFound_ReturnsNotFound()
        {
            sponsorRepoMock.Setup(r => r.GetById(999)).ReturnsAsync((Sponsor?)null);

            var result = await controller.GetSponsorById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditSponsor_Success_ReturnsOk()
        {
            var updateRequest = new UpdateSponsorRequestDto
            {
                Name = "Updated",
                ContactEmail = "upd@example.com",
                Phone = "000",
                Website = "upd"
            };

            var updatedSponsor = new Sponsor
            {
                SponsorId = 5,
                Name = updateRequest.Name,
                ContactEmail = updateRequest.ContactEmail,
                Phone = updateRequest.Phone,
                Website = updateRequest.Website
            };

            sponsorRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Sponsor>()))
                .ReturnsAsync(updatedSponsor);

            var result = await controller.EditSponsor(5, updateRequest);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<SponsorDto>(ok.Value);
            Assert.Equal(5, dto.SponsorId);
            Assert.Equal(updateRequest.Name, dto.Name);
        }

        [Fact]
        public async Task EditSponsor_NotFound_ReturnsNotFound()
        {
            sponsorRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Sponsor>()))
                .ReturnsAsync((Sponsor?)null);

            var updateRequest = new UpdateSponsorRequestDto
            {
                Name = "X",
                ContactEmail = "x@example.com",
                Phone = "x",
                Website = "x"
            };

            var result = await controller.EditSponsor(1234, updateRequest);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveSponsor_Success_ReturnsOk()
        {
            var sponsor = new Sponsor
            {
                SponsorId = 7,
                Name = "ToDelete",
                ContactEmail = "del@example.com",
                Phone = "777",
                Website = "del"
            };

            sponsorRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(sponsor);

            var result = await controller.RemoveSponsor(7);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<SponsorDto>(ok.Value);
            Assert.Equal(7, dto.SponsorId);
        }

        [Fact]
        public async Task RemoveSponsor_NotFound_ReturnsNotFound()
        {
            sponsorRepoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync((Sponsor?)null);

            var result = await controller.RemoveSponsor(42);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
