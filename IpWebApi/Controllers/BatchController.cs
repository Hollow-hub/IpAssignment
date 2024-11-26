using IpWebApi.Batching;
using IpWebApi.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace IpWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BatchController(IEnumerable<IHostedService> services, ILogger<BatchController> logger)
    : ControllerBase
{
    private readonly BatchProcessingService batchProcessingService = services.OfType<BatchProcessingService>().First();
    private readonly ILogger<BatchController> logger = logger;

    [HttpPost("batch-update")]
    public Ok<Guid> BatchUpdate([FromBody] List<string> ipAddresses)
    {
        var jobId = Guid.NewGuid();
        var job = new BatchJob
        {
            Id = jobId,
            IpAddresses = ipAddresses,
        };
        
        batchProcessingService.AddJob(job);
        logger.CreatedBatchJob(jobId, ipAddresses.Count);
        
        return TypedResults.Ok(job.Id);
    }
    
    [HttpGet("batch-status/{jobId}")]
    public Results<Ok<BatchProgressResponse> , NotFound<string>> GetBatchProgress(Guid jobId)
    {
        var job = batchProcessingService.GetJob(jobId);
        
        if (job == null)
        {
            logger.JobNotFound(jobId);
            return TypedResults.NotFound("Job does not exist or already completed.");
        }

        var response = job.ToResponse();
        
        return TypedResults.Ok(response);
    }
}