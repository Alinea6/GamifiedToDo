using System.Net;
using System.Text;
using FluentAssertions;
using GamifiedToDo.API.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GamifiedToDo.Tests.Integration.Host.Controllers;

public partial class UserControllerTest
{
    [TestCase(null)]
    [TestCase("")]
    public async Task Register_should_throw_exception_when_login_is_empty(string login)
    {
        var request = new RegisterRequest
        {
            Login = login,
            Email = "test-email@example.com",
            Password = "test-password"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Register_should_throw_exception_when_login_is_longer_than_50_characters()
    {
        var request = new RegisterRequest
        {
            Login = "0123456789012345678901234567890123456789012345678901234567890123456789",
            Email = "test-email@example.com",
            Password = "test-password"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [TestCase(null)]
    [TestCase("")]
    public async Task Register_should_throw_exception_when_password_is_empty(string password)
    {
        var request = new RegisterRequest
        {
            Login = "test-login",
            Email = "test-email@example.com",
            Password = password
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task Register_should_throw_exception_when_password_is_longer_than_50_characters()
    {
        var request = new RegisterRequest
        {
            Login = "test-login",
            Email = "test-email@example.com",
            Password = "0123456789012345678901234567890123456789012345678901234567890123456789"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task Register_should_throw_exception_when_email_is_longer_than_50_characters()
    {
        var request = new RegisterRequest
        {
            Login = "test-login",
            Email = "0123456789012345678901234567890123456789012345678901234567890123456789@example.com",
            Password = "test-password"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [TestCase(null)]
    [TestCase("")]
    public async Task Register_should_throw_exception_when_email_is_empty(string email)
    {
        var request = new RegisterRequest
        {
            Login = "test-login",
            Email = email,
            Password = "test-password"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public async Task Register_should_throw_exception_when_email_input_is_not_an_email_address()
    {
        var request = new RegisterRequest
        {
            Login = "test-login",
            Email = "test",
            Password = "test-password"
        };

        var json = JsonConvert.SerializeObject(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/user/register", data);


        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}