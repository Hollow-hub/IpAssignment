
using IpAssignment.Library;
using IpAssignment.Library.Abstractions;
using IpAssignment.Library.Implementations;
using IpWebApi;
using IpWebApi.Abstractions;
using IpWebApi.Batching;
using IpWebApi.Cache;
using IpWebApi.Repository;
using Microsoft.EntityFrameworkCore;
using Refit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();

builder.Services.AddRefitClient<IIPStackApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.ipstack.com"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

builder.Services.AddDbContext<IpDbContext>(options =>
{ 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IIpRepository, PersistenceIpRepository>();

builder.Services.AddHostedService<BatchProcessingService>();

builder.Services.AddOptions<IpStackSettings>()
    .Bind(builder.Configuration.GetSection("IpStackSettings"))
    .ValidateDataAnnotations();

builder.Services.AddSingleton<IIPInfoProvider, IPStackIPInfoProvider>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program
{
}