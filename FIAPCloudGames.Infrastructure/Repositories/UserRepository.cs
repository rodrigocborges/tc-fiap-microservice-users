using FIAPCloudGames.Domain.Entities;
using FIAPCloudGames.Domain.Interfaces;
using FIAPCloudGames.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(User model)
        {
            await _context.Users.AddAsync(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task Delete(Guid id)
        {
            User? model = await Find(id);
            if (model == null)
                return;
            _context.Users.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> Find(Guid id)
            => await _context.Users.FindAsync(id);

        public async Task<ICollection<User>?> FindAll(int skip = 0, int take = 10)
            => await _context.Users.Skip(skip).Take(take).ToListAsync();

        public async Task<User?> FindByEmail(string email)
            => await _context.Users.FirstOrDefaultAsync(x => x.Email.Value == email);

        public async Task Update(User model)
        {
            _context.Users.Update(model);
            await _context.SaveChangesAsync();
        }
    }
}
