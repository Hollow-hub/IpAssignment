namespace IpWebApi.Batching;

public class BatchProgressResponse
{
    public Guid Id { get; set; }
    
    public int ProcessedCount { get; set; }
    
    public int TotalCount { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public string Progress { get; set; } = string.Empty;
}