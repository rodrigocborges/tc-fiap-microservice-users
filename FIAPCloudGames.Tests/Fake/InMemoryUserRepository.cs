using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Tests.Fake
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User> {
            new User("Rodrigo Borges", "rodrigo@email.com", "Senha123@", UserRole.Admin),
            new User("Alberto", "alberto@email.com", "!@!Senha123@", UserRole.Customer),
        };

        public async Task<Guid> Create(User model)
        {
            var userFound = await FindByEmail(email: model.Email.Value);
            if (userFound != null)
                throw new InvalidOperationException("User already exists!");

            _users.Add(model);
            return model.Id;
        }

        public Task Delete(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
                _users.Remove(user);

            return Task.CompletedTask;
        }

        public Task<User?> Find(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<ICollection<User>?> FindAll(int skip = 0, int take = 10)
        {
            return Task.FromResult<ICollection<User>?>(_users);
        }

        public Task<User?> FindByEmail(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Value == email);
            return Task.FromResult(user);
        }

        public Task Update(User model)
        {
            // Simplesmente assume que o objeto na lista já está atualizado.
            return Task.CompletedTask;
        }
    }
}
