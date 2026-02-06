using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Viamus.Sonarqube.Mcp.Server.Configuration;
using Viamus.Sonarqube.Mcp.Server.Middleware;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Configuration;

public class ApiKeyMiddlewareTests
{
    private readonly ILogger<ApiKeyMiddleware> _logger = Substitute.For<ILogger<ApiKeyMiddleware>>();

    private static IOptionsMonitor<ServerSecuritySettings> CreateOptions(bool requireApiKey, string apiKey = "")
    {
        var settings = new ServerSecuritySettings { RequireApiKey = requireApiKey, ApiKey = apiKey };
        var monitor = Substitute.For<IOptionsMonitor<ServerSecuritySettings>>();
        monitor.CurrentValue.Returns(settings);
        return monitor;
    }

    [Fact]
    public async Task InvokeAsync_WhenApiKeyNotRequired_ShouldPassThrough()
    {
        var nextCalled = false;
        var middleware = new ApiKeyMiddleware(_ => { nextCalled = true; return Task.CompletedTask; }, _logger);
        var context = new DefaultHttpContext();
        var options = CreateOptions(requireApiKey: false);

        await middleware.InvokeAsync(context, options);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_WhenApiKeyRequired_AndMissing_ShouldReturn401()
    {
        var nextCalled = false;
        var middleware = new ApiKeyMiddleware(_ => { nextCalled = true; return Task.CompletedTask; }, _logger);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var options = CreateOptions(requireApiKey: true, apiKey: "valid-key");

        await middleware.InvokeAsync(context, options);

        nextCalled.Should().BeFalse();
        context.Response.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task InvokeAsync_WhenApiKeyRequired_AndValid_ShouldPassThrough()
    {
        var nextCalled = false;
        var middleware = new ApiKeyMiddleware(_ => { nextCalled = true; return Task.CompletedTask; }, _logger);
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Api-Key"] = "valid-key";
        var options = CreateOptions(requireApiKey: true, apiKey: "valid-key");

        await middleware.InvokeAsync(context, options);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_WhenApiKeyRequired_AndInvalid_ShouldReturn401()
    {
        var nextCalled = false;
        var middleware = new ApiKeyMiddleware(_ => { nextCalled = true; return Task.CompletedTask; }, _logger);
        var context = new DefaultHttpContext();
        context.Request.Headers["X-Api-Key"] = "wrong-key";
        context.Response.Body = new MemoryStream();
        var options = CreateOptions(requireApiKey: true, apiKey: "valid-key");

        await middleware.InvokeAsync(context, options);

        nextCalled.Should().BeFalse();
        context.Response.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task InvokeAsync_WhenHealthEndpoint_ShouldBypassAuthentication()
    {
        var nextCalled = false;
        var middleware = new ApiKeyMiddleware(_ => { nextCalled = true; return Task.CompletedTask; }, _logger);
        var context = new DefaultHttpContext();
        context.Request.Path = "/health";
        var options = CreateOptions(requireApiKey: true, apiKey: "valid-key");

        await middleware.InvokeAsync(context, options);

        nextCalled.Should().BeTrue();
    }
}
