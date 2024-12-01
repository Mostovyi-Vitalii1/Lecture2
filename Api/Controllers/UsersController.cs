using Api.Extensions;
using Api.Dtos;
using Application.Common.Interfaces.Queries;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;
using Domain.Users;

namespace Api.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IUserRQueries _userQueries;

        public UsersController(ISender sender, IUserRQueries userQueries)
        {
            _sender = sender;
            _userQueries = userQueries;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserRDto>>> GetAll(CancellationToken cancellationToken)
        {
            var entities = await _userQueries.GetAll(cancellationToken);
            return Ok(entities.Select(UserRDto.FromDomainModel).ToList());
        }

        [HttpPost]
        public async Task<ActionResult<UserRDto>> Create(
            [FromBody] UserRDto request,
            CancellationToken cancellationToken)
        {
            var input = new CreateUserRCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var result = await _sender.Send(input, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(UserRDto.FromDomainModel(result.Value));
            }

            return BadRequest(result.Error); // Повертаємо помилку з BadRequest
        }

        // Додавання методу для оновлення користувача
        [HttpPut("{id}")]
        public async Task<ActionResult<UserRDto>> Update(
            Guid id, // Отримуємо ID користувача з URL
            [FromBody] UserRDto request,
            CancellationToken cancellationToken)
        {
            // Перевіряємо, чи є ID
            if (id != request.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var input = new UpdateUserRCommand
            {
                UserId = new UserRId(id), // Перетворення ID в UserRId
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var result = await _sender.Send(input, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(UserRDto.FromDomainModel(result.Value));
            }

            return BadRequest(result.Error); // Повертаємо помилку з BadRequest
        }
        // Додавання методу для видалення користувача
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var input = new DeleteUserRCommand
            {
                UserId = new UserRId(id)
            };

            var result = await _sender.Send(input, cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent(); // Повертаємо статус 204 No Content, якщо видалення успішне
            }

            return BadRequest(result.Error); // Повертаємо помилку з BadRequest
        }
    }
}
