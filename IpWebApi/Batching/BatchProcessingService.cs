using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using IpAssignment.Library.Abstractions;
using IpWebApi.Abstractions;
using IpWebApi.Cache;
using IpWebApi.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IpWebApi.Batching;

public class BatchProcessingService(
    ILogger<BatchProcessingService> logger,
    IServiceScopeFactory scopeFactory,
    IMemoryCache memoryCache,
    IOptions<CacheSettings> cacheSettings,
    IHostEnvironment environment,
    IIPInfoProvider ipInfoProvider)
    : BackgroundService
{
    private readonly ILogger<BatchProcessingService> logger = logger;
    private readonly IServiceScopeFactory scopeFactory = scopeFactory;
    private readonly IHostEnvironment environment = environment;
    private readonly IMemoryCache memoryCache = memoryCache;
    private readonly ConcurrentDictionary<Guid, BatchJob> jobs = [];
    private readonly IIPInfoProvider ipInfoProvider = ipInfoProvider;

    private readonly MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.SlidingExpirationMinutes));

    public void AddJob(BatchJob job)
    {
        jobs.AddOrUpdate(job.Id, job, (key, oldValue) => job);
        logger.JobAdded(job.Id, jobs.Count);
    }

    
    public BatchJob? GetJob(Guid jobId)
    {
        var job = jobs.FirstOrDefault(x => x.Key == jobId).Value;
        
        if (job == null)
        {
            logger.JobNotFound(jobId);
        }
        
        return job;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            await Process(stoppingToken);
        }
    }

    private async Task Process(CancellationToken stoppingToken = default)
    {
        using var scope = scopeFactory.CreateScope();
        var ipRepository = scope.ServiceProvider.GetRequiredService<IIpRepository>();
        var toRemove = new List<Guid>();
        foreach (var (id,job) in jobs.Where(j => j.Value.IsCompleted == false).ToList())
        {
            foreach (var ipChunk in job.IpAddresses.Chunk(10))
            {
                if (environment.IsDevelopment())
                {
                    await Task.Delay(2000, stoppingToken);
                }
                
                foreach (var ip in ipChunk)
                {
                    if (!IPAddress.TryParse(ip, out var address)
                        && address!.AddressFamily != AddressFamily.InterNetwork
                        && address.AddressFamily != AddressFamily.InterNetworkV6)
                    {
                        logger.InvalidIp(ip);
                        job.ProcessedCount++;
                    }
                    var details = await ipInfoProvider.GetIPDetailsAsync(ip);
                    var entity = details.ToEntity(address);
                    await ipRepository.AddIPDetailsAsync(entity);
                    
                    job.ProcessedCount++;
                    logger.ProcessedCount(job.ProcessedCount, job.TotalCount, job.Id);
                    memoryCache.Set(address, entity, cacheEntryOptions);
                }
            }
            
            logger.JobCompleted(job.Id);
            toRemove.Add(job.Id);
        }

        foreach (var id in toRemove)
        {
            jobs.Remove(id, out _);
        }
    }
}