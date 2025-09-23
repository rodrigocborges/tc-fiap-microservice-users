using FIAPCloudGames.Domain.Enumerators;

namespace FIAPCloudGames.Application.Requests
{
    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
