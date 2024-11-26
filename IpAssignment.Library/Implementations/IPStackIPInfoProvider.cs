using System.Net;
using System.Net.Sockets;
using IpAssignment.Library.Abstractions;
using IpAssignment.Library.Exceptions;
using IpAssignment.Library.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IpAssignment.Library.Implementations;

public class IPStackIPInfoProvider(ILogger<IPStackIPInfoProvider> logger, IIPStackApi ipStackApi, IOptions<IpStackSettings> settings) : IIPInfoProvider
{
    private readonly IIPStackApi ipStackApi = ipStackApi;
    private readonly IpStackSettings settings = settings.Value;
    private readonly ILogger<IPStackIPInfoProvider> logger = logger;

    public IPDetails GetDetails(string ip)
    {
        var response = GetIPDetailsAsync(ip).Result;
        return response;
    }

    public async Task<IPDetails> GetIPDetailsAsync(string ip)
    {
        if (!IPAddress.TryParse(ip, out var address) || (address.AddressFamily != AddressFamily.InterNetwork && address.AddressFamily != AddressFamily.InterNetworkV6))
        {
            throw new InvalidIPException("Invalid IP address. Only IPv4 and IPv6 addresses are supported.", ip);
        }
        
        if (IsPrivateIPv4(ip) || IsPrivateIPv6(ip))
        {
            throw new InvalidIPException("Private IP addresses are not supported.", ip);
        }
        
        var details = await ipStackApi.GetIPDetailsAsync(ip, settings.ApiKey);
        logger.ReceivedResponse(ip);
        if (details.Error == null)
        {
            return details.Content!;
        }
        
        logger.ApiError(details.Error,ip);
        throw new IPServiceNotAvailableException("Error occurred while fetching IP details", details.Error);
    }
    
    private static bool IsPrivateIPv4(string ip)
    {
        var split = ip.Split('.');
        return split[0] == "10" || (split[0] == "172" && int.Parse(split[1]) >= 16 && int.Parse(split[1]) <= 31) 
                                || (split[0] == "192" && split[1] == "168") 
                                || (split[0] == "169" && split[1] == "254")
                                || split[0] == "127";
    }
    
    private static bool IsPrivateIPv6(string ip)
    {
        var split = ip.Split(':');
        return split[0] == "fc00" || split[0] == "fd00";
    }
}