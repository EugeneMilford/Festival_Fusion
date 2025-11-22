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
    public class VenueControllerTests
    {
        private readonly Mock<IVenueRepository> venueRepoMock;
        private readonly VenueController controller;

        public VenueControllerTests()
        {
            venueRepoMock = new Mock<IVenueRepository>();
            controller = new VenueController(venueRepoMock.Object);
        }

        [Fact]
        public async Task AddVenue_ReturnsOkWithCreatedDto()
        {
            var request = new AddVenueRequestDto
            {
                Name = "Test Venue",
                VenueImageUrl = "http://img",
                Address = "123 Test St",
                Capacity = 500
            };

            venueRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Venue>()))
                .ReturnsAsync((Venue v) =>
                {
                    v.VenueId = 1;
                    return v;
                });

            var result = await controller.AddVenue(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<VenueDto>(ok.Value);
            Assert.Equal(1, dto.VenueId);
            Assert.Equal(request.Name, dto.Name);
            Assert.Equal(request.Capacity, dto.Capacity);
        }

        [Fact]
        public async Task GetAllVenues_ReturnsOkWithList()
        {
            var venues = new List<Venue>
            {
                new Venue { VenueId = 1, Name = "V1", VenueImageUrl = "img1", Address = "A1", Capacity = 100 },
                new Venue { VenueId = 2, Name = "V2", VenueImageUrl = "img2", Address = "A2", Capacity = 200 }
            };

            venueRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(venues);

            var result = await controller.GetAllVenues();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<VenueDto>>(ok.Value);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task GetVenueById_Found_ReturnsOk()
        {
            var venue = new Venue
            {
                VenueId = 10,
                Name = "FoundVenue",
                VenueImageUrl = "img",
                Address = "addr",
                Capacity = 300
            };

            venueRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(venue);

            var result = await controller.GetVenueById(10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<VenueDto>(ok.Value);
            Assert.Equal(venue.VenueId, dto.VenueId);
            Assert.Equal(venue.Name, dto.Name);
        }

        [Fact]
        public async Task GetVenueById_NotFound_ReturnsNotFound()
        {
            venueRepoMock.Setup(r => r.GetById(999)).ReturnsAsync((Venue?)null);

            var result = await controller.GetVenueById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditVenue_Success_ReturnsOk()
        {
            var updateRequest = new UpdateVenueRequestDto
            {
                Name = "Updated",
                VenueImageUrl = "img-upd",
                Address = "addr-upd",
                Capacity = 999
            };

            var updatedVenue = new Venue
            {
                VenueId = 5,
                Name = updateRequest.Name,
                VenueImageUrl = updateRequest.VenueImageUrl,
                Address = updateRequest.Address,
                Capacity = updateRequest.Capacity
            };

            venueRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Venue>()))
                .ReturnsAsync(updatedVenue);

            var result = await controller.EditVenue(5, updateRequest);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<VenueDto>(ok.Value);
            Assert.Equal(5, dto.VenueId);
            Assert.Equal(updateRequest.Name, dto.Name);
            Assert.Equal(updateRequest.Capacity, dto.Capacity);
        }

        [Fact]
        public async Task EditVenue_NotFound_ReturnsNotFound()
        {
            venueRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Venue>()))
                .ReturnsAsync((Venue?)null);

            var updateRequest = new UpdateVenueRequestDto
            {
                Name = "X",
                VenueImageUrl = "img",
                Address = "addr",
                Capacity = 1
            };

            var result = await controller.EditVenue(1234, updateRequest);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveVenue_Success_ReturnsOk()
        {
            var venue = new Venue
            {
                VenueId = 7,
                Name = "ToDelete",
                VenueImageUrl = "img",
                Address = "addr",
                Capacity = 150
            };

            venueRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(venue);

            var result = await controller.RemoveVenue(7);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<VenueDto>(ok.Value);
            Assert.Equal(7, dto.VenueId);
        }

        [Fact]
        public async Task RemoveVenue_NotFound_ReturnsNotFound()
        {
            venueRepoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync((Venue?)null);

            var result = await controller.RemoveVenue(42);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}