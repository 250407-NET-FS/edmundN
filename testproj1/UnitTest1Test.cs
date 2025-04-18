using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using modals;

namespace edmundN.Tests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProgramTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/users");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddUser_WithValidData_ReturnsCreatedResult()
        {
            var client = _factory.CreateClient();
            var user = new AddUserRequest
            {
                Username = "testuser",
                Email = "test@test.com"
            };

            var response = await client.PostAsync("/adduser",
                new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task AddVideo_WithValidData_ReturnsCreatedResult()
        {
            var client = _factory.CreateClient();
            var video = new AddVideoRequest
            {
                Title = "testvideo",
                Url = "testurl"
            };

            var response = await client.PostAsync("/addvideo",
                new StringContent(JsonSerializer.Serialize(video), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetNonExistentUser_ReturnsNotFound()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/getuser/nonexistentuser");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetVideos_ReturnsOkResult()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/videos");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}