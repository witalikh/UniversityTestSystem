namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<AnswerEntity> _set;

        public AnswerRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<AnswerEntity>();
        }

        public async Task<bool> AddAsync(AnswerEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(AnswerEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<AnswerEntity?> GetAsync(int id)
        {
            return await this._set.FindAsync(id);
        }

        public async Task<List<AnswerEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
            return true;
        }
    }
}
