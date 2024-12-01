using Api.Dtos;
using Domain.Users;

public record UserRDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    DateTime UpdatedAt)
{
    public static UserRDto FromDomainModel(UserR userR)
        => new(
            Id: userR.Id.Value,
            FirstName: userR.FirstName,
            LastName: userR.LastName,
            FullName: $"{userR.FirstName} {userR.LastName}",
            UpdatedAt: userR.UpdatedAt);
}
