namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class AttemptRepository : IAttemptRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<AttemptEntity> _set;

        public AttemptRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<AttemptEntity>();
        }

        public async Task<bool> AddAsync(AttemptEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(AttemptEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AttemptEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<AttemptEntity?> GetAsync(Guid id)
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
