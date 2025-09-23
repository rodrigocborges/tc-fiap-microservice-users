using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Services;
using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Tests.Fake;

namespace FIAPCloudGames.Tests;

public class UserServiceTest
{
    [Fact]
    public async Task CreateUser_WithValidData_ShouldAddUserAndReturnId()
    {
        // Arrange
        var userRepository = new InMemoryUserRepository();
        var service = new UserService(userRepository);

        var request = new CreateUserRequest
        {
            Name = "Rodrigo Borges",
            Email = "rodrigo2@email.com",
            Password = "Senha123@",
            Role = UserRole.Admin
        };

        // Act
        var result = await service.Create(new User(request.Name, request.Email, request.Password, request.Role));

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        var createdUser = await userRepository.Find(result);
        Assert.NotNull(createdUser);
        Assert.Equal(request.Name, createdUser!.Name);
        Assert.Equal(request.Email, createdUser.Email.Value);
        Assert.Equal(request.Role, createdUser.Role);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        var userRepository = new InMemoryUserRepository();
        var service = new UserService(userRepository);

        var request = new CreateUserRequest
        {
            Name = "Rodrigo2",
            Email = "rodrigo@email.com",
            Password = "Password123!",
            Role = UserRole.Customer
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await service.Create(new User(request.Name, request.Email, request.Password, request.Role));
        });
    }
}
