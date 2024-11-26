namespace IpWebApi.Batching;

public class BatchJob
{
    public Guid Id { get; init; }
    public List<string> IpAddresses { get; init; }

    public int ProcessedCount { get; set; }

    public int TotalCount => IpAddresses.Count;
    
    public bool IsCompleted => ProcessedCount == TotalCount;
}