namespace IpAssignment.Library.Exceptions;

public class IPServiceNotAvailableException : Exception
{ 
    public IPServiceNotAvailableException(string message) : base(message) { } 
    public IPServiceNotAvailableException(string message, Exception inner) : base(message, inner) { } 
}