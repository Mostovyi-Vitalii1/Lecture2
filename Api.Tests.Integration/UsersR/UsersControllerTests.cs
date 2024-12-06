using System.Net.Http.Json;
using Api.Dtos;
using Domain.Users;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data;
using Xunit;

namespace Api.Tests.Integration.Users;

public class UsersControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly UserR _mainUser;

    public UsersControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _mainUser = UsersData.MainUser;
    }

    [Fact]
    public async Task ShouldCreateUser()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var request = new UserRDto(
            Id: Guid.Empty, 
            FirstName: firstName, 
            LastName: lastName, 
            UpdatedAt: DateTime.MinValue);

        // Act
        var response = await Client.PostAsJsonAsync("users", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var responseUser = await response.ToResponseModel<UserRDto>();
        var userId = new UserRId(responseUser.Id); // Перетворюємо в UserRId

        // Використовуємо Value для порівняння Guid в UserRId
        var dbUser = await Context.UsersR.FirstAsync(x => x.Id == userId);
        dbUser.FirstName.Should().Be(firstName);
        dbUser.LastName.Should().Be(lastName);
        dbUser.FullName.Should().Be($"{firstName} {lastName}");
        // Порівнюємо значення Guid всередині UserRId
        dbUser.Id.Value.Should().Be(responseUser.Id); // Порівняння значення Guid в UserRId
    }

    [Fact]
    public async Task ShouldUpdateUser()
    {
        // Arrange
        var newFirstName = "Jane";
        var newLastName = "Smith";
        var request = new UserRDto(
            Id: _mainUser.Id.Value, 
            FirstName: newFirstName, 
            LastName: newLastName, 
            UpdatedAt: DateTime.MinValue);

        // Act
        var response = await Client.PutAsJsonAsync($"users/{_mainUser.Id.Value}", request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbUser = await Context.UsersR.FirstAsync(x => x.Id == _mainUser.Id);
        dbUser.FirstName.Should().Be(newFirstName);
        dbUser.LastName.Should().Be(newLastName);
        dbUser.FullName.Should().Be($"{newFirstName} {newLastName}");  // Перевірка, що FullName оновлюється правильно

    }



    [Fact]
    public async Task ShouldDeleteUser()
    {
        // Act
        var response = await Client.DeleteAsync($"users/{_mainUser.Id.Value}");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dbUser = await Context.UsersR.FirstOrDefaultAsync(x => x.Id == _mainUser.Id);
        dbUser.Should().BeNull();
    }

    [Fact]
    public async Task ShouldGetAllUsers()
    {
        // Act
        var response = await Client.GetAsync("users");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var users = await response.ToResponseModel<List<UserRDto>>();
        users.Should().ContainSingle();
        users.First().Id.Should().Be(_mainUser.Id.Value);
    }

    public async Task InitializeAsync()
    {
        await Context.UsersR.AddAsync(_mainUser);
        await SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        Context.UsersR.RemoveRange(Context.UsersR);
        await SaveChangesAsync();
    }
}
