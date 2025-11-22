using FestivalFusion.API.Controllers;
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
    public class ContactControllerTests
    {
        private readonly Mock<IContactRepository> contactRepoMock;
        private readonly ContactController controller;

        public ContactControllerTests()
        {
            contactRepoMock = new Mock<IContactRepository>();
            controller = new ContactController(contactRepoMock.Object);
        }

        [Fact]
        public async Task CreateContact_ReturnsCreatedWithDto()
        {
            var request = new AddContactRequestDto
            {
                Name = "Jane",
                Surname = "Doe",
                PhoneNumber = 1234567890,
                Message = "Hello"
            };

            contactRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<Contact>()))
                .ReturnsAsync((Contact c) =>
                {
                    c.ContactId = 1;
                    return c;
                });

            var result = await controller.CreateContact(request);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<ContactDto>(created.Value);
            Assert.Equal(1, dto.ContactId);
            Assert.Equal(request.Name, dto.Name);
            Assert.Equal(request.Surname, dto.Surname);
            Assert.Equal(request.PhoneNumber, dto.PhoneNumber);
            Assert.Equal(request.Message, dto.Message);
        }

        [Fact]
        public async Task CreateContact_NullRequest_ReturnsBadRequest()
        {
            var result = await controller.CreateContact((AddContactRequestDto?)null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetAllContacts_ReturnsOkWithList()
        {
            var contacts = new List<Contact>
            {
                new Contact { ContactId = 1, Name = "A", Surname = "A", PhoneNumber = 1111111111, Message = "m1" },
                new Contact { ContactId = 2, Name = "B", Surname = "B", PhoneNumber = 1111111112, Message = "m2" }
            };

            contactRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(contacts);

            var result = await controller.GetAllContacts();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ContactDto>>(ok.Value);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task RemoveContact_Success_ReturnsOk()
        {
            var contact = new Contact
            {
                ContactId = 7,
                Name = "ToDelete",
                Surname = "User",
                PhoneNumber = 11111113,
                Message = "Please delete"
            };

            contactRepoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(contact);

            var result = await controller.RemoveContact(7);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ContactDto>(ok.Value);
            Assert.Equal(7, dto.ContactId);
            Assert.Equal(contact.Name, dto.Name);
        }

        [Fact]
        public async Task RemoveContact_NotFound_ReturnsNotFound()
        {
            contactRepoMock.Setup(r => r.DeleteAsync(42)).ReturnsAsync((Contact?)null);

            var result = await controller.RemoveContact(42);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}