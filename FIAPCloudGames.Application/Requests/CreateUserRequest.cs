using FIAPCloudGames.Domain.Enumerators;

namespace FIAPCloudGames.Application.Requests
{
    public class CreateUserRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.Customer;
    }
}
