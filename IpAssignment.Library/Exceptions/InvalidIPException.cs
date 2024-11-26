namespace IpAssignment.Library.Exceptions;

public class InvalidIPException : Exception
{
    public InvalidIPException(string message, string ip) : base(message)
    {
    }
}