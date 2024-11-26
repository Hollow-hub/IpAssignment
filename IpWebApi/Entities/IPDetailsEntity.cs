using System.Net;

namespace IpWebApi.Entities;

public class IPDetailsEntity
{
    public int Id { get; set; }
    public IPAddress Ip { get; set; } = IPAddress.None;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Continent { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
