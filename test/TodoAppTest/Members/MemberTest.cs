using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

namespace TodoAppTest.Members;

public class MemberTest : BaseIntegrationTest
{
    public MemberTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenUsernameIsMissing()
    {
        var request = new Web.Api.Features.Register.RegisterRequest("", "password", "Jhon Doe");

        var response = await HttpClient.PostAsJsonAsync("/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenRequestIsValid()
    {
        var request = new Web.Api.Features.Register.RegisterRequest("johndoe", "password", "John Doe");

        var response = await HttpClient.PostAsJsonAsync("/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenInvalidCredentials()
    {
        var request = new Web.Api.Features.Login.LoginRequest("johndoe", "wrongpassword");

        var response = await HttpClient.PostAsJsonAsync("/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenValidCredentials()
    {
        var request = new Web.Api.Features.Login.LoginRequest("johndoe", "password");

        var response = await HttpClient.PostAsJsonAsync("/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseBody = await response.Content.ReadFromJsonAsync<Web.Api.Features.Login.LoginResponse>();
        responseBody.Should().NotBeNull();
        responseBody!.Token.Should().NotBeNullOrEmpty();
    }
}
