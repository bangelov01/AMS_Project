namespace AMS.Tests.Tests.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Xunit;

    using AMS.Data;

    using AMS.Services;
    using AMS.Services.Models;

    using AMS.Web.Areas.Admin.Controllers;

    using AMS.Tests.Mocks;
    using static AMS.Tests.Database.DatabaseInitialize;

    public class UsersTests : IDisposable
    {
        private readonly AMSDbContext data;

        private readonly UsersController usersController;

        private const string testUserId = "TestUser0";

        public UsersTests()
        {
            this.data = Initialize();
            this.usersController = new UsersController(new UserService(data,
                Options.Create(new AppSettingsServiceModel { 
                    Email = "testAdmin",
                    Password = "testAdmin",
                    Username = "testAdmin" }),
                MapperMock.Instance));
        }

        public void Dispose()
        {
            this.data.Dispose();
        }

        [Fact]
        public async Task All_ReturnsViewResultAndUsers_WithUserServiceModel()
        {
            var result = await usersController.All();

            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UsersServiceModel>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task Suspend_ReturnsBadRequest_WithNullOrEmptyUserId_OrNonExistantId(string value)
        {
            var result = await usersController.Suspend(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Suspend_ReturnsRedirectResult_AndSuspendsUser()
        {
            var result = await usersController.Suspend(testUserId);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("All", redirectResult.ActionName);
            Assert.True(data.Users.Find(testUserId).IsSuspended);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task Allow_ReturnsBadRequest_WithNullOrEmptyUserId_OrNonExistantId(string value)
        {
            var result = await usersController.Allow(value);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Allow_ReturnsRedirectResult_AndAllowsUser()
        {
            var result = await usersController.Allow(testUserId);

            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal("All", redirectResult.ActionName);
            Assert.False(data.Users.Find(testUserId).IsSuspended);
        }
    }
}
