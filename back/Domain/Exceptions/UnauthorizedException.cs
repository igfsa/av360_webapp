namespace Domain.Exceptions;

public class UnauthorizedException(string message = "Unauthorized") : Exception(message)
{
}