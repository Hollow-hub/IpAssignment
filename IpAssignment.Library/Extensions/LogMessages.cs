using Microsoft.Extensions.Logging;

namespace IpAssignment.Library.Extensions;

public static partial class LogMessages
{
    private const string ReceivedResponseMessage = "Received response from IPStack API for IP: {IP}";
    private const string ApiErrorMessage = "API error occurred while fetching IP details for IP: {IP}";
    
    [LoggerMessage(LogLevel.Information, ReceivedResponseMessage)]
    public static partial void ReceivedResponse(this ILogger logger, string ip);
    
    [LoggerMessage(LogLevel.Error, ApiErrorMessage)]
    public static partial void ApiError(this ILogger logger, Exception error, string ip);
}