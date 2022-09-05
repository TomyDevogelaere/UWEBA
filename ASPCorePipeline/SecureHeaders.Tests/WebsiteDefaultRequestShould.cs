using Microsoft.AspNetCore.Mvc.Testing;
using System.Globalization;

namespace SecureHeaders.Tests;

public class WebsiteDefaultRequestShould
{
    private readonly WebApplicationFactory<global::Program> server;
    private readonly HttpClient client;
    public WebsiteDefaultRequestShould()
    {
        this.server = new WebApplicationFactory<global::Program>()
            .WithWebHostBuilder(builder =>
            { 
                
            });
        this.client = this.server.CreateClient();
    }
    [Fact]
    public async Task addHeadersToResponse()
    {
        //Arrange
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        //Act
        var headers = response.Headers;
        string? xss = headers.GetValues("X-Xss-Protection").FirstOrDefault();
        string? frameOptions = headers.GetValues("X-Frame-Options").FirstOrDefault();
        string? contentType = headers.GetValues("X-Content-Type-Options").FirstOrDefault();

        //Assert
        Assert.Equal("1", xss);
        Assert.Equal("SAMEORIGIN", frameOptions);
        Assert.Equal("nosniff", contentType);
    }
}