using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;

namespace FIAPCloudGames.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> Create(User model)
        {
            var userFound = await FindByEmail(email: model.Email.Value);
            if (userFound != null)
                throw new InvalidOperationException("User is already exists!");

            return await _repository.Create(model);
        }

        public async Task Delete(Guid id)
            => await _repository.Delete(id);

        public async Task<User?> Find(Guid id)
            => await _repository.Find(id);

        public async Task<ICollection<User>?> FindAll(int skip = 0, int take = 10)
            => await _repository.FindAll(skip, take);

        public async Task<User?> FindByEmail(string email)
            => await _repository.FindByEmail(email);

        public async Task<User?> Login(string email, string password)
        {
            User? userFound = await FindByEmail(email: email);
            if (userFound == null)
                return null;

            if(!userFound.Password.IsEqual(password))
                return null;

            return userFound;
        }

        public async Task Update(User model)
            => await _repository.Update(model);
    }
}
