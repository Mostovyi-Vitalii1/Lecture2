using Domain.Users;

namespace Application.Users.Exceptions
{
    public class UserAlreadyExistsException : UserRException
    {
        public UserAlreadyExistsException(UserRId userId)
            : base($"User with ID '{userId}' already exists.")
        {
        }
    }

    public class UserUnknownException : UserRException
    {
        public UserUnknownException(UserRId userId, Exception innerException)
            : base($"Unknown error occurred for user with ID '{userId}'.", innerException)
        {
        }
    }

    public abstract class UserRException : Exception
    {
        protected UserRException(string message) : base(message) { }
        protected UserRException(string message, Exception innerException) : base(message, innerException) { }
    }
}