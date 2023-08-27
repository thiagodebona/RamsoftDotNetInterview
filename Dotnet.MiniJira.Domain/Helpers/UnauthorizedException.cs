namespace Dotnet.MiniJira.Domain.Helpers;

using System.Globalization;

// custom exception class for throwing application specific exceptions (e.g. for validation) 
// that can be caught and handled within the application
public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base() {}

    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException(string message, params object[] args) 
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}