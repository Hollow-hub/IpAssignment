using System.Net;
using IpWebApi.Entities;

namespace IpWebApi.Abstractions;

public interface IIpRepository
{
    Task<IPDetailsEntity?> GetIPDetailsAsync(IPAddress ip);
    Task AddIPDetailsAsync(IPDetailsEntity ipDetails);
}