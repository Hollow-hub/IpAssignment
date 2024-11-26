using System.Net;
using System.Text;
using System.Text.Json;

namespace IpAssignmentTests;

public class BatchControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> factory;

    public BatchControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task BatchUpdate_ReturnsJobId()
    {
        // Arrange
        var ipAddresses = new List<string> { "8.8.8.8", "8.8.4.4" };
        var content = new StringContent(JsonSerializer.Serialize(ipAddresses), Encoding.UTF8, "application/json");
        var client = factory.CreateClient();
        
        // Act
        var response = await client.PostAsync("/api/batch/batch-update", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var jobId = JsonSerializer.Deserialize<Guid>(responseString);
        Assert.NotEqual(Guid.Empty, jobId);
    }
    
    [Fact]
    public async Task GetBatchProgress_ReturnsNotFound_WhenJobDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var jobId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/batch/batch-status/{jobId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}