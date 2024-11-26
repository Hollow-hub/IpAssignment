using IpAssignment.Library;
using IpAssignment.Library.Abstractions;
using IpAssignment.Library.Exceptions;
using IpAssignment.Library.Implementations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using Xunit.Abstractions;

namespace IpAssignmentTests;

public class IPInfoProviderTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IPInfoProviderTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetIPDetailsAsync_ReturnsCorrectDetails_ForIPv4()
    {
        // Arrange
        var ipStackApi = RestService.For<IIPStackApi>("https://api.ipstack.com"); 
        var apiKey = Environment.GetEnvironmentVariable("IpStackSettings__ApiKey");
        var settings = new IpStackSettings { ApiKey = apiKey };
        IIPInfoProvider sut = new IPStackIPInfoProvider(NullLogger<IPStackIPInfoProvider>.Instance, ipStackApi, Options.Create(settings));
    
        // Act
        var response = await sut.GetIPDetailsAsync("8.8.8.8");
    
        // Assert
        Assert.Equal("Glenmont", response.City);
        Assert.Equal("North America", response.Continent);
        Assert.Equal("United States", response.Country);
        Assert.Equal(40.536998748779297, response.Latitude);
        Assert.Equal(-82.128593444824219, response.Longitude);
    }
    
    [Fact]
    public async Task GetIPDetailsAsync_ReturnsCorrectDetails_ForIPv6()
    {
        //arrange
        var ipStackApi = RestService.For<IIPStackApi>("https://api.ipstack.com"); 
        var apiKey = Environment.GetEnvironmentVariable("IpStackSettings__ApiKey");
        var settings = new IpStackSettings { ApiKey = apiKey };
        IIPInfoProvider sut = new IPStackIPInfoProvider(NullLogger<IPStackIPInfoProvider>.Instance, ipStackApi, Options.Create(settings));
        //act
        var response = await sut.GetIPDetailsAsync("2606:4700:4700::1111");
        
        //assert
        Assert.Equal("San Francisco", response.City);
        Assert.Equal("North America", response.Continent);
        Assert.Equal("United States", response.Country);
        Assert.Equal(37.775001525878906, response.Latitude);
        Assert.Equal(-122.41832733154297, response.Longitude);
    }
    
    [Fact]
    public async Task GetIPDetailsAsync_ThrowsException_ForInvalidIPv4()
    {
        // Arrange
        var ipStackApi = RestService.For<IIPStackApi>("https://api.ipstack.com"); 
        var apiKey = Environment.GetEnvironmentVariable("IpStackSettings__ApiKey");
        var settings = new IpStackSettings { ApiKey = apiKey };
        IIPInfoProvider sut = new IPStackIPInfoProvider(NullLogger<IPStackIPInfoProvider>.Instance, ipStackApi, Options.Create(settings));
    
        // Act & Assert
        await Assert.ThrowsAsync<InvalidIPException>(() => sut.GetIPDetailsAsync("999.999.999.999"));
    }
    
    [Fact]
    public async Task GetIPDetailsAsync_ThrowsException_ForInvalidIPv6()
    {
        // Arrange
        var ipStackApi = RestService.For<IIPStackApi>("https://api.ipstack.com"); 
        var apiKey = Environment.GetEnvironmentVariable("IpStackSettings__ApiKey");
        var settings = new IpStackSettings { ApiKey = apiKey };
        IIPInfoProvider sut = new IPStackIPInfoProvider(NullLogger<IPStackIPInfoProvider>.Instance, ipStackApi, Options.Create(settings));
    
        // Act & Assert
        await Assert.ThrowsAsync<InvalidIPException>(() => sut.GetIPDetailsAsync("GGGG:GGGG:GGGG:GGGG:GGGG:GGGG:GGGG:GGGG"));
    }

    [Fact]
    public async Task GetIPDetailsAsync_ThrowsException_ForLanPrivateNetworkIP()
    {
        // Arrange
        var ipStackApi = RestService.For<IIPStackApi>("https://api.ipstack.com"); 
        var apiKey = Environment.GetEnvironmentVariable("IpStackSettings__ApiKey");
        var settings = new IpStackSettings { ApiKey = apiKey };
        IIPInfoProvider sut = new IPStackIPInfoProvider(NullLogger<IPStackIPInfoProvider>.Instance, ipStackApi, Options.Create(settings));
    
        // Act & Assert
        await Assert.ThrowsAsync<InvalidIPException>(() => sut.GetIPDetailsAsync("192.168.1.1"));
    }


}