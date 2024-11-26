using IpAssignment.Library.Implementations;
using Refit;

namespace IpAssignment.Library.Abstractions;

public interface IIPStackApi
{
    [Get("/{ip}?access_key={apikey}")]
    Task<ApiResponse<IPDetailsResponse>> GetIPDetailsAsync(string ip, string apikey);
}