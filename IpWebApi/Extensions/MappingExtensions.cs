using System.Net;
using IpAssignment.Library.Abstractions;
using IpAssignment.Library.Implementations;
using IpWebApi.Batching;
using IpWebApi.Entities;

namespace IpWebApi.Extensions;

public static class MappingExtensions
{
    public static BatchProgressResponse ToResponse(this BatchJob job)
    {
        return new BatchProgressResponse
        {
            Id = job.Id,
            ProcessedCount = job.ProcessedCount,
            TotalCount = job.TotalCount,
            IsCompleted = job.IsCompleted,
            Progress = $"{job.ProcessedCount}/{job.TotalCount}"
        };
    }
    
    public static IPDetailsEntity ToEntity(this IPDetails details, IPAddress ipAddress)
    {
        return new IPDetailsEntity
        {
            City = details.City,
            Continent = details.Continent,
            Country = details.Country,
            Latitude = details.Latitude,
            Longitude = details.Longitude,
            Ip = ipAddress,
        };
    }
    
    public static IPDetailsResponse ToResponse(this IPDetailsEntity details)
    {
        return new IPDetailsResponse
        {
            City = details.City,
            Continent = details.Continent,
            Country = details.Country,
            Latitude = details.Latitude,
            Longitude = details.Longitude,
        };
    }
    
    public static IPDetailsResponse ToModel(this IPDetailsEntity entity)
    {
        return new IPDetailsResponse
        {
            City = entity.City,
            Continent = entity.Continent,
            Country = entity.Country,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude
        };
    }
}