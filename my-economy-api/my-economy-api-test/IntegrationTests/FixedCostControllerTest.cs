using FluentAssertions; // For more readable and expressive assertions in tests, FluentAssertions allows you to write assertions in a way that closely resembles natural language, improving the clarity and maintainability of your test code.
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection; //To use AddScoped in the test setup, also other types of injection if needed
using Microsoft.VisualStudio.TestPlatform.TestHost;
using my_economy_api;
using my_economy_api.Models;
using NSubstitute; // For mocking dependencies in tests, mocking is essential to isolate the unit of work being tested and to control the behavior of dependencies, ensuring that tests are reliable and focused on the specific functionality being evaluated.
using RepositoryPatern.Interfaces;
using System.Net;
using Xunit;

namespace my_economy_api_test.IntegrationTests
{
    public class FixedCostControllerTest : IClassFixture<WebApplicationFactory<Program>> //The IClassFixture<T> interface is used in xUnit to indicate that a test class requires a shared context or fixture. In this case, it indicates that the FixedCostControllertTest class will use a WebApplicationFactory<Program> as its fixture, allowing it to set up and tear down the test environment for each test method in the class.
    {
        private readonly WebApplicationFactory<Program> _factory; //The WebApplicationFactory is a test fixture provided by the Microsoft.AspNetCore.Mvc.Testing package that allows you to create an in-memory test server for your ASP.NET Core application. It is used to set up the testing environment and create HTTP clients for making requests to the API during tests.

        public FixedCostControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetFixedsCost_ReturnsNotFound_WhenNoData()
        {
            var mockDb = Substitute.For<IRepository<FixedCost>>(); //Create a mock instance of the IDbProvider class using NSubstitute
            mockDb.GetAllAsync().Returns(new List<FixedCost>());

            var client = _factory.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => services.AddScoped(_ => mockDb));
            }).CreateClient(); //Create an HTTP client for testing the API, using the WebApplicationFactory to set up the test server and injecting the mocked DbProvider into the service collection.

            var response = await client.GetAsync("/FixedCost");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound); //Assert that the response status code is NotFound (404), which is the expected outcome when there are no fixed costs available in the database.
        }
    }
}