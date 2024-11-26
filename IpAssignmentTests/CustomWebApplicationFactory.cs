using IpWebApi;
using Microsoft.AspNetCore.Hosting;

namespace IpAssignmentTests;

using Microsoft.AspNetCore.Mvc.Testing;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(services =>
        {
           
        });
    }
}

