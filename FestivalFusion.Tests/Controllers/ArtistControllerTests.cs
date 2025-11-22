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
    public class ArtistControllerTests
    {
        private readonly Mock<IArtistRepository> artistRepoMock;
        private readonly ArtistController controller;

        public ArtistControllerTests()
        {
            artistRepoMock = new Mock<IArtistRepository>();
            controller = new ArtistController(artistRepoMock.Object);
        }

        [Fact]
        public async Task AddArtist_ReturnsOkWithCreatedDto()
        {
            var request = new AddArtistRequestDto
            {
                Name = "Test Artist",
                ArtistImageUrl = "http://img",
                Genre = "Rock",
                Country = "USA",
                Bio = "Test bio"
            };

            artistRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Artist>()))
                .ReturnsAsync((Artist a) =>
                {
                    a.ArtistId = 1;
                    return a;
                });

            var result = await controller.AddArtist(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ArtistDto>(ok.Value);
            Assert.Equal(1, dto.ArtistId);
            Assert.Equal(request.Name, dto.Name);
            Assert.Equal(request.Genre, dto.Genre);
        }

        [Fact]
        public async Task GetAllArtists_ReturnsOkWithList()
        {
            var artists = new List<Artist>
            {
                new Artist
                {
                    ArtistId = 1,
                    Name = "A1",
                    ArtistImageUrl = "img1",
                    Genre = "g1",
                    Country = "c1",
                    Bio = "b1"
                },
                new Artist
                {
                    ArtistId = 2,
                    Name = "A2",
                    ArtistImageUrl = "img2",
                    Genre = "g2",
                    Country = "c2",
                    Bio = "b2"
                }
            };

            artistRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(artists);

            var result = await controller.GetAllArtists();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ArtistDto>>(ok.Value);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task GetArtistById_Found_ReturnsOk()
        {
            var artist = new Artist
            {
                ArtistId = 10,
                Name = "Found",
                ArtistImageUrl = "img",
                Genre = "genre",
                Country = "country",
                Bio = "bio"
            };

            artistRepoMock.Setup(r => r.GetById(10)).ReturnsAsync(artist);

            var result = await controller.GetArtistById(10);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ArtistDto>(ok.Value);
            Assert.Equal(artist.ArtistId, dto.ArtistId);
            Assert.Equal(artist.Name, dto.Name);
        }

        [Fact]
        public async Task GetArtistById_NotFound_ReturnsNotFound()
        {
            artistRepoMock.Setup(r => r.GetById(999)).ReturnsAsync((Artist?)null);

            var result = await controller.GetArtistById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditArtist_Success_ReturnsOk()
        {
            var updateRequest = new UpdateArtistRequestDto
            {
                Name = "Updated",
                ArtistImageUrl = "img-upd",
                Genre = "genre-upd",
                Country = "country-upd",
                Bio = "bio-upd"
            };

            var updatedArtist = new Artist
            {
                ArtistId = 5,
                Name = updateRequest.Name,
                ArtistImageUrl = updateRequest.ArtistImageUrl,
                Genre = updateRequest.Genre,
                Country = updateRequest.Country,
                Bio = updateRequest.Bio
            };

            artistRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Artist>()))
                .ReturnsAsync(updatedArtist);

            var result = await controller.EditArtist(5, updateRequest);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ArtistDto>(ok.Value);
            Assert.Equal(5, dto.ArtistId);
            Assert.Equal(updateRequest.Name, dto.Name);
        }

        [Fact]
        public async Task EditArtist_NotFound_ReturnsNotFound()
        {
            artistRepoMock
                .Setup(r => r.UpdateAsync(It.IsAny<Artist>()))
                .ReturnsAsync((Artist?)null);

            var updateRequest = new UpdateArtistRequestDto
            {
                Name = "X",
                ArtistImageUrl = "img",
                Genre = "g",
                Country = "c",
                Bio = "b"
            };

            var result = await controller.EditArtist(1234, updateRequest);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task RemoveArtist_Success_ReturnsOk()
        {
            var artist = new Artist
            {
                ArtistId = 7,
                Name = "ToDelete",
                ArtistImageUrl = "img",
                Genre = "g",
                Country = "c",
                Bio = "b"
            };

            artistRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(artist);

            var result = await controller.RemoveArtist(7);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ArtistDto>(ok.Value);
            Assert.Equal(7, dto.ArtistId);
        }

        [Fact]
        public async Task RemoveArtist_NotFound_ReturnsNotFound()
        {
            artistRepoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync((Artist?)null);

            var result = await controller.RemoveArtist(42);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}