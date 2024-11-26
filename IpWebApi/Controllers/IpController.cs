using System.Net;
using System.Net.Sockets;
using IpAssignment.Library.Abstractions;
using IpAssignment.Library.Exceptions;
using IpAssignment.Library.Implementations;
using IpWebApi.Abstractions;
using IpWebApi.Cache;
using IpWebApi.Entities;
using IpWebApi.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IpWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IpController(
    IIPInfoProvider ipInfoProvider,
    IMemoryCache memoryCache,
    ILogger<IpController> logger,
    IIpRepository ipRepository,
    IOptions<CacheSettings> cacheSettings)
    : ControllerBase
{
    private readonly IIPInfoProvider ipInfoProvider = ipInfoProvider;
    private readonly IMemoryCache memoryCache = memoryCache;
    private readonly ILogger<IpController> logger = logger;
    private readonly IIpRepository ipRepository = ipRepository;
    private readonly MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.SlidingExpirationMinutes));

    [HttpGet("{ip}")]
    public async Task<Results<Ok<IPDetailsResponse>, BadRequest<string>>> GetDetails(string ip)
    {
        if (!IPAddress.TryParse(ip, out var address)
            && address!.AddressFamily != AddressFamily.InterNetwork
            && address.AddressFamily != AddressFamily.InterNetworkV6)
        {
            logger.InvalidIp(ip);
            return TypedResults.BadRequest("Invalid IP address. Only IPv4 and IPv6 addresses are supported.");
        }
        
        if (memoryCache.TryGetValue(address, out var cached) && cached is IPDetailsResponse cachedValue)
        {
            logger.CacheHit(address);
            return TypedResults.Ok(cachedValue);
        }
        
        var dbDetails = await ipRepository.GetIPDetailsAsync(address);
        if (dbDetails != null)
        {
            var ipDetails = dbDetails.ToModel();
            logger.DatabaseHit(address);
            memoryCache.Set(address, ipDetails, cacheEntryOptions);
            return TypedResults.Ok(ipDetails);
        }

        memoryCache.Set(address, dbDetails, cacheEntryOptions);

        IPDetails details;
        try
        {
            logger.FetchingIpDetails(ip);
            details = await ipInfoProvider.GetIPDetailsAsync(ip);
        }
        catch (IPServiceNotAvailableException e)
        {
            logger.FailedToFetchIpDetails(e, ip);
            throw;
        }
        
        var entity = new IPDetailsEntity
        {
            Ip = address,
            City = details.City,
            Country = details.Country,
            Continent = details.Continent,
            Latitude = details.Latitude,
            Longitude = details.Longitude,
        };

        await ipRepository.AddIPDetailsAsync(entity);
            
        memoryCache.Set(ip, entity, cacheEntryOptions);
        return TypedResults.Ok(entity.ToResponse());
    }
}