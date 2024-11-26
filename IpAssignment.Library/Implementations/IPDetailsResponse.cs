using System.Text.Json.Serialization;
using IpAssignment.Library.Abstractions;

namespace IpAssignment.Library.Implementations;

public class IPDetailsResponse : IPDetails
{
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("country_name")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("continent_name")]
    public string Continent { get; set; } = string.Empty;
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
}