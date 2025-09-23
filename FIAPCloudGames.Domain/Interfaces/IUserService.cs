using FIAPCloudGames.Domain.Entities;

namespace FIAPCloudGames.Domain.Interfaces
{
    public interface IUserService : ICreate<User>, IUpdate<User>,
        IFind<User>, IFindAll<User>, IDelete<User>
    {
        Task<User?> Login(string email, string password);
        Task<User?> FindByEmail(string email);
    }
}
