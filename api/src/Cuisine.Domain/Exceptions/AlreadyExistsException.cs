namespace Cuisine.Domain.Exceptions;

public class AlreadyExistsException(string message) : Exception(message)
{
}