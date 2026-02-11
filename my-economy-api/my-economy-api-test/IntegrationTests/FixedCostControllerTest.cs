using FluentAssertions; // For more readable and expressive assertions in tests, FluentAssertions allows you to write assertions in a way that closely resembles natural language, improving the clarity and maintainability of your test code.
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection; //To use AddScoped in the test setup, also other types of injection if needed
using Microsoft.VisualStudio.TestPlatform.TestHost;
using my_economy_api;
using my_economy_api.Models;
using my_economy_api.Services.Interfaces;
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

        private static FixedCost CreateValidFixedCost()
        {
            return new FixedCost
            {
                Id = 0,
                Name = "Gasto Válido",
                Amount = 100.0M,
                Frequency = 30,
                CategoryID = 1,
                Description = "Descripción estándar",
                UserId = "TEST-USER"
            };
        }

        /// <summary>
        /// Creates an authenticated test HTTP client configured with a mock repository for use in integration tests.
        /// </summary>
        /// <remarks>The returned client uses a test authentication scheme to simulate an authenticated user,
        /// allowing access to endpoints that require authentication. The mock repository is registered as a scoped
        /// dependency, enabling controlled test scenarios for FixedCost data access.</remarks>
        /// <param name="mock">The mock implementation of the repository for FixedCost entities. Used to override the repository dependency
        /// in the test client.</param>
        /// <returns>An authenticated HttpClient instance with the mock repository injected. The client can be used to send
        /// requests to the test server with authentication enabled.</returns>
        private HttpClient GetClientWithMock(IRepository<FixedCost> repoMock)
        {
            // Creamos un mock de auth que siempre funcione para no romper los tests viejos
            var defaultAuthMock = Substitute.For<IAuthService>();
            defaultAuthMock.GetUserIdAsync(Arg.Any<string>()).Returns(Task.FromResult<string?>("1"));

            return GetClientWithSpecificAuth(repoMock, defaultAuthMock);
        }

        /// <summary>
        /// Creates an HttpClient instance configured with the specified repository and authentication service mocks for
        /// integration testing.
        /// </summary>
        /// <remarks>This method is intended for use in integration tests where specific service
        /// implementations need to be injected and authentication must be bypassed or controlled. The returned
        /// HttpClient is configured to use a test authentication scheme, allowing requests to endpoints protected by
        /// authorization attributes without requiring real authentication.</remarks>
        /// <param name="repoMock">The repository mock to be injected into the test server's dependency injection container. Used to simulate
        /// data access for FixedCost entities during tests.</param>
        /// <param name="authMock">The authentication service mock to be injected into the test server's dependency injection container. Used
        /// to simulate authentication and authorization behavior during tests.</param>
        /// <returns>An HttpClient instance configured to use the provided mocks and a test authentication scheme. The client
        /// does not automatically follow redirects.</returns>
        private HttpClient GetClientWithSpecificAuth(IRepository<FixedCost> repoMock, IAuthService authMock)
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1. Inyectamos los Mocks específicos que hemos configurado en el test
                    services.AddScoped(_ => repoMock);
                    services.AddScoped(_ => authMock);

                    // 2. Configuramos el sistema de autenticación de prueba
                    // Esto es necesario para que el atributo [Authorize] no bloquee la petición
                    services.AddAuthentication("TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        public static IEnumerable<object[]> GetMandatoryParameterNull()
        {
            var cost1 = CreateValidFixedCost();
            cost1.Name = "";
            yield return new object[] { cost1, "Name is empty" };

            var cost2 = CreateValidFixedCost();
            cost2.Name = null!;
            yield return new object[] { cost2, "Name is null" };

            var cost3 = CreateValidFixedCost();
            cost3.CategoryID = 0;
            yield return new object[] { cost3, "CategoryID is 0" };
        }

        public static IEnumerable<object[]> GetAmountBelowZero()
        {
            var cost1 = CreateValidFixedCost();
            cost1.Amount = -5;
            yield return new object[] { cost1, "Amount is negative" };

            var cost2 = CreateValidFixedCost();
            cost2.Amount = 0;
            yield return new object[] { cost1, "Amount is 0" };
        }

        public static IEnumerable<object[]> GetFrecuencyBelowZero()
        {
            var cost1 = CreateValidFixedCost();
            cost1.Frequency = -5;
            yield return new object[] { cost1, "Frequency is negative" };

            var cost2 = CreateValidFixedCost();
            cost2.Frequency = 0;
            yield return new object[] { cost1, "Frequency is 0" };
        }

        public static IEnumerable<object[]> GetBoundaryInvalidData()
        {
            var cost1 = CreateValidFixedCost();
            cost1.Name = new string('A', 51);
            yield return new object[] { cost1, "Name exceeds 50 chars" };

            var cost2 = CreateValidFixedCost();
            cost2.Frequency = 366;
            yield return new object[] { cost1, "Frequency exceeds 365 days" };
        }

        public static IEnumerable<object[]> GetIdsZero()
        {
            var cost1 = CreateValidFixedCost();
            cost1.Id = 0;
            var id1 = 0;
            yield return new object[] { cost1, id1, "Both ids are 0" };
        }

        public static IEnumerable<object[]> GetDiferentIds()
        {
            var cost1 = CreateValidFixedCost();
            cost1.Id = 12;
            var id1 = 34;
            yield return new object[] { cost1, id1, "Both ids are diferent" };
        }

        public static IEnumerable<object[]> GetZeroOrNegativeIds()
        {
            yield return new object[] { 0, "Id cannot be 0" };
            yield return new object[] { -1, "Id cannot be below 0" };
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

            var client = GetClientWithMock(mockDb);

            var response = await client.GetAsync("/FixedCost");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound); //Assert that the response status code is NotFound (404), which is the expected outcome when there are no fixed costs available in the database.
        }

        [Fact]
        public async Task GetFixedsCost_ReturnsUnauthorized_WhenNoTokenSended()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/FixedCost");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, "because the controller is protected with [Authorize]"); //Assert that the response status code is NotFound (404), which is the expected outcome when there are no fixed costs available in the database.
        }

        /// <summary>
        /// Verifies that the GetFixedsCost API returns a BadRequest response when the provided ID is zero or negative.
        /// </summary>
        /// <remarks>This test ensures that the API does not attempt to access the repository when an
        /// invalid ID is provided, and that it responds with the appropriate HTTP status code.</remarks>
        /// <param name="id">The fixed cost identifier to test. Must be zero or a negative integer to trigger the BadRequest response.</param>
        /// <param name="reason">A description of the test case or the reason for using the specified ID value.</param>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Theory]
        [MemberData(nameof(GetZeroOrNegativeIds))]
        public async Task GetFixedsCost_ReturnsBadRequest_WhenIdIsZeroOrBelow(int id, string reason)
        {
            var mockDb = Substitute.For<IRepository<FixedCost>>(); //Create a mock instance of the IDbProvider class using NSubstitute

            var client = GetClientWithMock(mockDb);
            var response = await client.GetAsync($"/FixedCost/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}"); //Assert that the response status code is NotFound (404), which is the expected outcome when there are no fixed costs available in the database.

            await mockDb.DidNotReceive().GetByIdAsync(Arg.Any<int>()); //Verify that the GetByIdAsync method was not called on the mock repository, as the request should have been rejected before reaching that point.
        }

        /// <summary>
        /// Verifies that the DeleteFixedCost API returns a BadRequest response when called with an ID that is zero or
        /// negative.
        /// </summary>
        /// <remarks>This test ensures that the API correctly rejects invalid IDs by returning a
        /// BadRequest status and does not attempt to delete any records from the repository.</remarks>
        /// <param name="id">The fixed cost identifier to delete. Must be zero or a negative value to trigger the BadRequest response.</param>
        /// <param name="reason">A description of the test case or rationale for using the specified ID value.</param>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Theory]
        [MemberData(nameof(GetZeroOrNegativeIds))]
        public async Task DeleteFixedsCost_ReturnsBadRequest_WhenIdIsZeroOrBelow(int id, string reason)
        {
            var mockDb = Substitute.For<IRepository<FixedCost>>(); //Create a mock instance of the IDbProvider class using NSubstitute

            var client = GetClientWithMock(mockDb);

            var response = await client.DeleteAsync($"/FixedCost/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}"); //Assert that the response status code is NotFound (404), which is the expected outcome when there are no fixed costs available in the database.

            mockDb.DidNotReceive().Delete(Arg.Any<FixedCost>()); //Verify that the GetByIdAsync method was not called on the mock repository, as the request should have been rejected before reaching that point.
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
            var validCost = new FixedCost { Name = "Netflix", Amount = 15.99M, Frequency = 30, CategoryID = 1, UserId = "TEST-123" };
            var mockDb = Substitute.For<IRepository<FixedCost>>();
            // Set up the mock to simulate successful addition of the fixed cost to the database.
            // When the AddAsync method is called with any FixedCost object, it will return a completed task,
            // indicating that the operation was successful without actually interacting with a real database.

            mockDb.AddAsync(Arg.Any<FixedCost>()).Returns(Task.CompletedTask);

            var client = GetClientWithMock(mockDb);

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", validCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        /// Verifies that posting a FixedCost with a non-zero Id returns a BadRequest response.
        /// </summary>
        /// <remarks>This test ensures that the API enforces the requirement that the Id property must be
        /// zero when creating a new FixedCost. If the Id is not zero, the API should respond with a BadRequest status
        /// code.</remarks>
        /// <returns>A task representing the asynchronous test operation.</returns>
         
        [Fact]
        public async Task PostFixedCost_ReturnsBadRequest_WhenIdIsNotZero()
        {
            // Arrange
            var invalidCost = new FixedCost { Id = 1, Name = "Netflix", Amount = 15.99M, Frequency = 30, CategoryID = 1 };
            var mockDb = Substitute.For<IRepository<FixedCost>>();

            var client = GetClientWithMock(mockDb);

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await mockDb.DidNotReceive().AddAsync(Arg.Any<FixedCost>()); //Verify that the AddAsync method was not called on the mock repository, as the request should have been rejected before reaching that point.
        }

        /// <summary>
        /// Verifies that the PutFixedCost endpoint returns a BadRequest response when both the route ID and the
        /// FixedCost object's ID are zero.
        /// </summary>
        /// <remarks>This test ensures that the API correctly rejects update requests with invalid IDs and
        /// does not attempt to update the repository when the IDs are zero.</remarks>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Fact]
        public async Task PutFixedCost_ReturnsBadRequest_WhenIdAreZero()
        {
            // Arrange
            var invalidCost = new FixedCost { Id = 0, Name = "Netflix", Amount = 15.99M, Frequency = 30, CategoryID = 1 };
            var invalidId = 0; // The ID in the query string is zero, which is invalid for an update operation
            var mockDb = Substitute.For<IRepository<FixedCost>>();

            var client = GetClientWithMock(mockDb);

            // Act
            var response = await client.PutAsJsonAsync($"/FixedCost/{invalidId}", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Verify that the Update method was not called on the mock repository, as the request should have been rejected before reaching that point.
            mockDb.DidNotReceive().Update(Arg.Any<FixedCost>());
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

            var client = GetClientWithMock(mockDb);

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}");
             
            await mockDb.DidNotReceive().AddAsync(Arg.Any<FixedCost>());
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

            var client = GetClientWithMock(mockDb);

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}");

            await mockDb.DidNotReceive().AddAsync(Arg.Any<FixedCost>());
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

            var client = GetClientWithMock(mockDb);

            // Act
            var response = await client.PostAsJsonAsync("/FixedCost", invalidCost);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Reason: {reason}");

            await mockDb.DidNotReceive().AddAsync(Arg.Any<FixedCost>());
        }

        /// <summary>
        /// Verifies that the GetFixedCost endpoint returns an Unauthorized response when the authentication service
        /// indicates the provided token is invalid.
        /// </summary>
        /// <remarks>This test simulates an authentication scenario where the token is present but not
        /// recognized as valid by the authentication service. It ensures that the API correctly responds with HTTP 401
        /// Unauthorized in such cases.</remarks>
        /// <returns>A task that represents the asynchronous test operation.</returns>
        [Fact]
        public async Task GetFixedCost_ReturnsUnauthorized_WhenServiceSaysTokenIsInvalid()
        {
            // Arrange
            var repoMock = Substitute.For<IRepository<FixedCost>>();
            var authMock = Substitute.For<IAuthService>();

            // Escenario: El servicio de auth devuelve NULL (token no válido)
            authMock.GetUserIdAsync(Arg.Any<string>()).Returns(Task.FromResult<string?>(null));

            var client = GetClientWithSpecificAuth(repoMock, authMock);

            // IMPORTANTE: Añadir la cabecera para que pase el primer filtro del [Authorize]
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme", "token-invalido");

            // Act
            var response = await client.GetAsync("/FixedCost");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}