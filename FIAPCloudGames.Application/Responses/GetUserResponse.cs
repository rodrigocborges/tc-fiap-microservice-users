using FIAPCloudGames.Domain.Enumerators;

namespace FIAPCloudGames.Application.Responses
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
