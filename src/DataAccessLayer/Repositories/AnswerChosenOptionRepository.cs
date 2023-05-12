namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class AnswerChosenOptionRepository : IAnswerChosenOptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<AnswerChosenOptionEntity> _set;

        public AnswerChosenOptionRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<AnswerChosenOptionEntity>();
        }

        public async Task<bool> AddAsync(AnswerChosenOptionEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(AnswerChosenOptionEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AnswerChosenOptionEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<AnswerChosenOptionEntity?> GetAsync(int id)
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
