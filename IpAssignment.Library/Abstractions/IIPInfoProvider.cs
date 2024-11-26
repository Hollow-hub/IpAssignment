namespace IpAssignment.Library.Abstractions;

// ReSharper disable once InconsistentNaming - name is provided by the assignment
public interface IIPInfoProvider
{
    IPDetails GetDetails(string ip);
    
    /// <summary>
    /// Asynchronously retrieves the IP details from the IPStack service.
    /// This method is preferred over the synchronous alternative as it allows better scalability and responsiveness.
    /// Maintains compatibility with the original assignment's synchronous target.
    /// </summary>
    /// <param name="ip">The IP address to retrieve details for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the IP details.</returns>
    Task<IPDetails> GetIPDetailsAsync(string ip);
}



