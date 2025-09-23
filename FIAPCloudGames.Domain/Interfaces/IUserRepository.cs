using FIAPCloudGames.Domain.Entities;

namespace FIAPCloudGames.Domain.Interfaces
{
    public interface IUserRepository : ICreate<User>, IUpdate<User>, 
        IFind<User>, IFindAll<User>, IDelete<User>
    {
        Task<User?> FindByEmail(string email);
    }
}
