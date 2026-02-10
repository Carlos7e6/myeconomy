using FluentAssertions; // For more readable and expressive assertions in tests, FluentAssertions allows you to write assertions in a way that closely resembles natural language, improving the clarity and maintainability of your test code.
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection; //To use AddScoped in the test setup, also other types of injection if needed
using Microsoft.VisualStudio.TestPlatform.TestHost;
using my_economy_api;
using my_economy_api.Models;
using NSubstitute; // For mocking dependencies in tests, mocking is essential to isolate the unit of work being tested and to control the behavior of dependencies, ensuring that tests are reliable and focused on the specific functionality being evaluated.
using RepositoryPatern.Interfaces;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace my_economy_api_test.IntegrationTests
{
    public class FixedCostControllerTest : IClassFixture<WebApplicationFactory<Program>> //The IClassFixture<T> interface is used in xUnit to indicate that a test class requires a shared context or fixture. In this case, it indicates that the FixedCostControllertTest class will use a WebApplicationFactory<Program> as its fixture, allowing it to set up and tear down the test environment for each test method in the class.
    {
        private readonly WebApplicationFactory<Program> _factory; //The WebApplicationFactory is a test fixture provided by the Microsoft.AspNetCore.Mvc.Testing package that allows you to create an in-memory test server for your ASP.NET Core application. It is used to set up the testing environment and create HTTP clients for making requests to the API during tests.

        public static IEnumerable<object[]> GetMandatoryParameterNull()
        {
            yield return new object[] { new FixedCost { Name = "", Amount = 50, Description = "Sin nombre" }, "Name is empty" };
            yield return new object[] { new FixedCost { Name = null!, Amount = 10, Description = "Nulo" }, "Name is null" };
            yield return new object[] { new FixedCost { Name = "Switch2", Amount = 10, Description = "Nulo", CategoryID = 0 }, "CategoryID is 0" };
        }

        public static IEnumerable<object[]> GetAmountBelowZero()
        {
            yield return new object[] { new FixedCost { Name = "", Amount = -5, Description = "Amount negative" }, "Amount is negative" };
            yield return new object[] { new FixedCost { Name = null!, Amount = 0, Description = "Amount 0" }, "Amount is 0" };
        }

        public static IEnumerable<object[]> GetFrecuencyBelowZero()
        {
            yield return new object[] { new FixedCost { Name = "", Frequency = -5, Description = "Frequency negative" }, "Frequency is negative" };
            yield return new object[] { new FixedCost { Name = null!, Frequency = 0, Description = "Frequency 0" }, "Frequency is 0" };
        }

        public static IEnumerable<object[]> GetBoundaryInvalidData()
        {
            yield return new object[] { new FixedCost { Name = new string('A', 51), Amount = 10 }, "Name exceeds 50 chars" };
            yield return new object[] { new FixedCost { Name = "Test", Amount = 10, Frequency = 366 }, "Frequency exceeds 365 days" };
        }

        public FixedCostControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Verifies that the GetFixedsCost API endpoint returns a NotFound (404) response when no fixed cost data is
        /// available.
        /// </summary>
        /// <remarks>This test uses a mocked repository to simulate an empty data set and asserts that the
        /// API responds appropriately. It ensures that the endpoint does not return data when none exists, supporting
        /// correct error handling in client applications.</remarks>
        /// <returns>A task representing the asynchronous test operation.</returns>

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

        /// <summary>
        /// Verifies that posting a valid fixed cost returns a Created (201) response.
        /// </summary>
        /// <remarks>This test ensures that when a valid FixedCost object is submitted to the API, the
        /// response status code is Created. The test uses a mocked repository to simulate successful
        /// persistence.</remarks>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Fact]
        public async Task PostFixedCost_ReturnsCreated_WhenDataIsValid()
        {
            // Arrange
            var validCost = new FixedCost { Name = "Netflix", Amount = 15.99f, Frequency = 30, CategoryID = 1 };
            var mockDb = Substitute.For<IRepository<FixedCost>>();
            // Set up the mock to simulate successful addition of the fixed cost to the database.
            // When the AddAsync method is called with any FixedCost object, it will return a completed task,
            // indicating that the operation was successful without actually interacting with a real database.

            mockDb.AddAsync(Arg.Any<FixedCost>()).Returns(Task.CompletedTask);

            var client = _factory.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => services.AddScoped(_ => mockDb));
            }).CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", validCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        /// Verifies that posting a fixed cost with missing mandatory parameters returns a BadRequest response.
        /// </summary>
        /// <remarks>This test uses parameterized input to ensure the API consistently rejects requests
        /// with missing required fields. The test expects the HTTP response status code to be BadRequest for each
        /// invalid input scenario.</remarks>
        /// <param name="invalidCost">A FixedCost object with one or more mandatory parameters set to null or missing. Used to simulate invalid
        /// input for the API.</param>
        /// <param name="reason">A description of which mandatory parameter is missing and why the test case is invalid.</param>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Theory]
        [MemberData(nameof(GetMandatoryParameterNull))]
        public async Task PostFixedCost_ReturnsBadRequest_WhenMandatoryParameterIsMissing(FixedCost invalidCost, string reason)
        {
            var mockDb = Substitute.For<IRepository<FixedCost>>(); //Create a mock instance of the IDbProvider class using NSubstitute
            
            var client = _factory.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => services.AddScoped(_ => mockDb));
            }).CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}");
        }

      /// <summary>
      /// Verifies that posting a fixed cost with an amount less than zero returns a BadRequest response.
      /// </summary>
      /// <remarks>This test ensures that the API correctly rejects fixed cost submissions with negative
      /// amount values by returning a BadRequest status. It uses parameterized test data to cover multiple invalid
      /// scenarios.</remarks>
      /// <param name="invalidCost">The fixed cost object containing an invalid amount value below zero to be submitted to the API.</param>
      /// <param name="reason">A description of why the provided fixed cost is considered invalid in this test case.</param>
      /// <returns>A task representing the asynchronous test operation.</returns>
        [Theory]
        [MemberData(nameof(GetAmountBelowZero))]

        public async Task PostFixedCost_ReturnsBadRequest_WhenAmountValueUnderZero(FixedCost invalidCost, string reason)
        {
            var mockDb = Substitute.For<IRepository<FixedCost>>(); //Create a mock instance of the IDbProvider class using NSubstitute

            var client = _factory.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => services.AddScoped(_ => mockDb));
            }).CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}");
        }

        /// <summary>
        /// Verifies that posting a fixed cost with boundary invalid data returns a BadRequest response.
        /// </summary>
        /// <remarks>This test uses boundary invalid data to ensure the API correctly rejects requests and
        /// returns a BadRequest status. The test is parameterized to cover multiple invalid scenarios.</remarks>
        /// <param name="invalidCost">A FixedCost object containing boundary invalid values to be submitted to the API.</param>
        /// <param name="reason">A description of why the provided data is considered invalid.</param>
        /// <returns>A task representing the asynchronous test operation.</returns>
        [Theory]
        [MemberData(nameof(GetBoundaryInvalidData))]

        public async Task PostFixedCost_ReturnsBadRequest_WhenBoundaryInvalidData(FixedCost invalidCost, string reason)
        {
            var mockDb = Substitute.For<IRepository<FixedCost>>(); //Create a mock instance of the IDbProvider class using NSubstitute

            var client = _factory.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => services.AddScoped(_ => mockDb));
            }).CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}");
        }
    }
}