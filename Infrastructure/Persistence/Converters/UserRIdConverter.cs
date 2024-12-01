using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Domain.Users;

public class UserRIdConverter : ValueConverter<UserRId, Guid>
{
    public UserRIdConverter() : base(
        id => id.Value, // Convert UserRId to Guid
        value => new UserRId(value)) // Convert Guid back to UserRId
    {
    }
}