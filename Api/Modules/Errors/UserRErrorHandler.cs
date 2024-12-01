
using System;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors
{
    public abstract class UserRException : Exception
    {
        protected UserRException(string message) : base(message) { }

        protected UserRException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class UserRNotFoundException : UserRException
    {
        public UserRNotFoundException(Guid userId)
            : base($"UserR with ID '{userId}' was not found.")
        {
        }
    }

    public class UserRAlreadyExistsException : UserRException
    {
        public UserRAlreadyExistsException(string fullName)
            : base($"UserR with the full name '{fullName}' already exists.")
        {
        }
    }

    public class UserRUnknownException : UserRException
    {
        public UserRUnknownException(Guid userId, Exception innerException)
            : base($"An unknown error occurred with the UserR ID '{userId}'.", innerException)
        {
        }
    }

    public static class UserRErrorHandler
    {
        public static ObjectResult ToObjectResult(this UserRException exception)
        {
            return new ObjectResult(exception.Message)
            {
                StatusCode = exception switch
                {
                    UserRNotFoundException => StatusCodes.Status404NotFound,
                    UserRAlreadyExistsException => StatusCodes.Status409Conflict,
                    UserRUnknownException => StatusCodes.Status500InternalServerError,
                    _ => throw new NotImplementedException("UserR error handler is not implemented")
                }
            };
        }
    }
}
