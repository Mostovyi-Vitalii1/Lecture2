using Domain.Users;

namespace Tests.Data;

public static class UsersData
{
    public static UserR MainUser => UserR.New(UserRId.New(), "Main User First Name", "Main User Last Name");
}