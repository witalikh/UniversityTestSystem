namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<UserEntity> _set;

        public UserRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<UserEntity>();
        }

        public async Task<bool> AddAsync(UserEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            return await this._set.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> DeleteAsync(UserEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<UserEntity?> GetAsync(string id)
        {
            return await this._set.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
            return true;
        }
    }
}
