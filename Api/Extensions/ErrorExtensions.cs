using Application.Users.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions
{
    public static class ErrorExtensions
    {
        public static ObjectResult ToObjectResult(this Exception exception)
        {
            var statusCode = exception switch
            {
                UserAlreadyExistsException => 409, // Конфлікт (користувач уже існує)
                _ => 500 // Загальна серверна помилка
            };

            return new ObjectResult(new { error = exception.Message })
            {
                StatusCode = statusCode
            };
        }
    }
}