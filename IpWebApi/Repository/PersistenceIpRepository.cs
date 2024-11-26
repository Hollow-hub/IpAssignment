using System.Net;
using IpWebApi.Abstractions;
using IpWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace IpWebApi.Repository;

public class PersistenceIpRepository(IpDbContext dbContext) : IIpRepository
{
    private readonly IpDbContext dbContext = dbContext;

    public async Task<IPDetailsEntity?> GetIPDetailsAsync(IPAddress ip)
    {
        return await dbContext.IPDetails.FirstOrDefaultAsync(x => x.Ip == ip);
    }

    public Task AddIPDetailsAsync(IPDetailsEntity ipDetails)
    {
        dbContext.IPDetails.Add(ipDetails);
        return dbContext.SaveChangesAsync();
    }
}