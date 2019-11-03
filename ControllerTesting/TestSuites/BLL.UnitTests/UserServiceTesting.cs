using System;
using System.Threading.Tasks;
using CookBook.BLL.Services;
using CookBook.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Testing.Mocking.BLL;
using Xunit;

namespace Testing.TestSuites.BLL.UnitTests
{
    public class UserServiceTesting : IDisposable
    {
        private readonly UserService _service;
        private readonly UserServiceMocking _mocker;

        public UserServiceTesting()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("testUsers")
                .EnableSensitiveDataLogging()
                .Options;
            _mocker = new UserServiceMocking(options);
            _service = _mocker.Setup();
        }

        [Fact]
        public async Task GetUserOk()
        {
            var result = await _service.GetAsync(1);

            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task GetUserNotFound()
        {
            var result = await _service.GetAsync(-1);

            Assert.Null(result);
        }

        public void Dispose()
        {
            _mocker.ClearContext();
        }
    }
}
