namespace Domain.Exceptions;

public class BusinessException(string message) : Exception(message)
{
}
