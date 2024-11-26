using System.Net;
using IpWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace IpWebApi;

public class IpDbContext(DbContextOptions<IpDbContext> options) : DbContext(options)
{
    public DbSet<IPDetailsEntity> IPDetails { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IPDetailsEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ip).HasConversion(
                v => v.ToString(),
                v => IPAddress.Parse(v));
        });
    }
}