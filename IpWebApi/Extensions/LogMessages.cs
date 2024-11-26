using System.Net;

namespace IpWebApi.Extensions;

public static partial class LogMessages
{
    private const string InvalidIpAddress = "Invalid IP address: {IP}";
    private const string JobProcessingMessage = "Processing job {JobId} with {TotalCount} items.";
    private const string ProcessedCountMessage = "Processed {ProcessedCount}/{TotalCount} items for job {JobId}.";
    private const string JobCompletedMessage = "Job {JobId} completed.";
    private const string JobAddedMessage = "Added job {JobId} to the job list. Jobs count: {Count}.";
    private const string BatchJobNotFoundMessage = "Job {JobId} not found in the job list.";
    private const string CreatedBatchJobMessage = "Created batch job {JobId} with {Count} items.";
    private const string JobStatusMessage = "Job {JobId} status: processed {ProcessedCount}/{TotalCount} items. Is completed: {IsCompleted}.";
    private const string JobNotFoundMessage = "Job {JobId} not found.";
    private const string CacheHitMessage = "Cache hit for {Address}";
    private const string DatabaseHitMessage = "Database hit for {Address}";
    private const string FetchingIpDetailsMessage = "Fetching IP details for {IP}.";
    private const string FailedToFetchIpDetailsMessage = "Failed to fetch IP details for {IP}.";

    
    [LoggerMessage(LogLevel.Error, InvalidIpAddress)]
    public static partial void InvalidIp(this ILogger logger, string ip);

    [LoggerMessage(LogLevel.Information, JobProcessingMessage)]
    public static partial void JobProcessing(this ILogger logger, Guid jobId, int totalCount);

    [LoggerMessage(LogLevel.Information, ProcessedCountMessage)]
    public static partial void ProcessedCount(this ILogger logger, int processedCount, int totalCount, Guid jobId);

    [LoggerMessage(LogLevel.Information, JobCompletedMessage)]
    public static partial void JobCompleted(this ILogger logger, Guid jobId);

    [LoggerMessage(LogLevel.Information, JobAddedMessage)]
    public static partial void JobAdded(this ILogger logger, Guid jobId, int count);

    [LoggerMessage(LogLevel.Warning, BatchJobNotFoundMessage)]
    public static partial void BatchJobNotFound(this ILogger logger, Guid jobId);
    
    [LoggerMessage(LogLevel.Information, CreatedBatchJobMessage)]
    public static partial void CreatedBatchJob(this ILogger logger, Guid jobId, int count);
    
    [LoggerMessage(LogLevel.Warning, JobNotFoundMessage)]
    public static partial void JobNotFound(this ILogger logger, Guid jobId);
    
    [LoggerMessage(LogLevel.Information, CacheHitMessage)]
    public static partial void CacheHit(this ILogger logger, IPAddress address);
    
    [LoggerMessage(LogLevel.Information, DatabaseHitMessage)]
    public static partial void DatabaseHit(this ILogger logger, IPAddress address);
    
    [LoggerMessage(LogLevel.Information, JobStatusMessage)]
    public static partial void JobStatus(this ILogger logger, Guid jobId, int processedCount, int totalCount, bool isCompleted);
    
    [LoggerMessage(LogLevel.Information, FetchingIpDetailsMessage)]
    public static partial void FetchingIpDetails(this ILogger logger, string ip);
    
    [LoggerMessage(LogLevel.Error, FailedToFetchIpDetailsMessage)]
    public static partial void FailedToFetchIpDetails(this ILogger logger, Exception exception, string ip);
}