namespace Domain.Exceptions;

public class ForbiddenException(string message = "Forbidden") : Exception(message)
{
}