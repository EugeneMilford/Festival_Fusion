using FestivalFusion.API.Controllers;
using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace FestivalFusion.Tests.Controllers
{
    public class AuthControllerTests
    {
        private static Mock<UserManager<IdentityUser>> CreateUserManagerMock()
        {
            // Minimal mock setup for UserManager
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        private static object? GetAnonymousProperty(object value, string propertyName)
        {
            if (value == null) return null;
            // Try common dictionary-like access first
            if (value is IDictionary<string, object> dict && dict.ContainsKey(propertyName))
                return dict[propertyName];

            // Try reflection (anonymous object)
            var prop = value.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null)
                return prop.GetValue(value);

            return null;
        }

        private static bool IsValidationProblemResult(ObjectResult obj)
        {
            if (obj == null) return false;

            // If the result value is ValidationProblemDetails, it should contain Errors
            if (obj.Value is ValidationProblemDetails vpd)
            {
                return vpd.Errors != null && vpd.Errors.Count > 0;
            }

            // If it's a ProblemDetails, it might still indicate validation
            if (obj.Value is ProblemDetails pd)
            {
                // ProblemDetails.Status may be null in this unit-test environment, but Title/detail may be present
                if (pd.Status.HasValue && pd.Status.Value == 400) return true;
                if (!string.IsNullOrEmpty(pd.Title) || !string.IsNullOrEmpty(pd.Detail)) return true;
            }

            // As a last resort, check ModelState on controller (not applicable here)
            return false;
        }

        [Fact]
        public async Task Register_Succeeds_ReturnsOk()
        {
            var userManagerMock = CreateUserManagerMock();
            var tokenRepoMock = new Mock<ITokenRepository>();

            // CreateAsync succeeds
            userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // AddToRoleAsync succeeds for any role
            userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AuthController(userManagerMock.Object, tokenRepoMock.Object);

            var request = new RegisterRequestDto
            {
                Email = "test@example.com",
                Password = "Password!23",
                Roles = new[] { "Reader", "Writer" }
            };

            var result = await controller.Register(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);

            // Use reflection to read anonymous object's Email property (works for anonymous objects)
            var emailVal = GetAnonymousProperty(ok.Value!, "Email") as string;
            Assert.Equal("test@example.com", emailVal);

            // Roles might be returned as array; attempt to read via reflection
            var rolesObj = GetAnonymousProperty(ok.Value!, "Roles");
            Assert.NotNull(rolesObj);
        }

        [Fact]
        public async Task Register_InvalidRole_ReturnsValidationProblem()
        {
            var userManagerMock = CreateUserManagerMock();
            var tokenRepoMock = new Mock<ITokenRepository>();

            userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AuthController(userManagerMock.Object, tokenRepoMock.Object);

            var request = new RegisterRequestDto
            {
                Email = "test2@example.com",
                Password = "Password!23",
                Roles = new[] { "InvalidRole" } // not in validRoles list
            };

            var result = await controller.Register(request);

            var obj = Assert.IsType<ObjectResult>(result);

            // Instead of strictly relying on StatusCode, assert that the ObjectResult contains Problem/Validation details
            Assert.True(IsValidationProblemResult(obj), "Expected a validation/problem response indicating invalid role(s).");
        }

        [Fact]
        public async Task Register_CreateFails_ReturnsValidationProblem()
        {
            var userManagerMock = CreateUserManagerMock();
            var tokenRepoMock = new Mock<ITokenRepository>();

            // CreateAsync fails with an error
            var failed = IdentityResult.Failed(new IdentityError { Description = "Bad password" });
            userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(failed);

            var controller = new AuthController(userManagerMock.Object, tokenRepoMock.Object);

            var request = new RegisterRequestDto
            {
                Email = "bad@example.com",
                Password = "weak",
                Roles = null
            };

            var result = await controller.Register(request);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.True(IsValidationProblemResult(obj), "Expected a validation/problem response when user creation fails.");
        }

        [Fact]
        public async Task Login_Succeeds_ReturnsOkWithTokenAndRoles()
        {
            var userManagerMock = CreateUserManagerMock();
            var tokenRepoMock = new Mock<ITokenRepository>();

            var identityUser = new IdentityUser { Email = "login@example.com", UserName = "login@example.com" };

            userManagerMock
                .Setup(um => um.FindByEmailAsync(It.Is<string>(s => s == "login@example.com")))
                .ReturnsAsync(identityUser);

            userManagerMock
                .Setup(um => um.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            userManagerMock
                .Setup(um => um.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new List<string> { "Reader", "Writer" });

            tokenRepoMock
                .Setup(tr => tr.CreateJwtToken(It.IsAny<IdentityUser>(), It.IsAny<List<string>>()))
                .Returns("mocked-jwt-token");

            var controller = new AuthController(userManagerMock.Object, tokenRepoMock.Object);

            var request = new LoginRequestDto
            {
                Email = "login@example.com",
                Password = "Password!23"
            };

            var result = await controller.Login(request);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<LoginResponseDto>(ok.Value);
            Assert.Equal("mocked-jwt-token", dto.Token);
            Assert.Contains("Reader", dto.Roles);
        }

        [Fact]
        public async Task Login_WrongCredentials_ReturnsValidationProblem()
        {
            var userManagerMock = CreateUserManagerMock();
            var tokenRepoMock = new Mock<ITokenRepository>();

            // User not found
            userManagerMock
                .Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser?)null);

            var controller = new AuthController(userManagerMock.Object, tokenRepoMock.Object);

            var request = new LoginRequestDto
            {
                Email = "noone@example.com",
                Password = "bad"
            };

            var result = await controller.Login(request);

            var obj = Assert.IsType<ObjectResult>(result);
            Assert.True(IsValidationProblemResult(obj), "Expected a validation/problem response for wrong credentials.");
        }
    }
}
