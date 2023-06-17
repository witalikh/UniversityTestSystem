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
            return await this._set
                .Include(a => a.Member)
                .Include(a => a.Answers)
                .ThenInclude(a => a.ChosenOptions)
                .Include(a => a.Answers)
                .ThenInclude(a => a.Question)
                .ThenInclude(q => q.ChoiceOptions)
                .Where(a => a.uuid == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetUserAttemptsCount(int testPk, string userPk)
        {
            return await this._set
                .Include(a => a.Member)
                .Where(a => a.TestId == testPk && a.Member.UserEntityId == userPk)
                .CountAsync();
        }

        public async Task<List<AttemptEntity>> GetUserTestAttempts(int testPk, string userPk)
        {
            return await this._set
                .Include(a => a.Member)
                .Include(a => a.Answers)
                .ThenInclude(a => a.ChosenOptions)
                .Include(a => a.Answers)
                .ThenInclude(a => a.Question)
                .ThenInclude(q => q.ChoiceOptions)
                .Where(a => a.TestId == testPk && a.Member.UserEntityId == userPk)
                .ToListAsync();
        }

        public async Task<List<AttemptEntity>> GetAllTestAttempts(int testPk)
        {
            return await this._set
                .Include(a => a.Member)
                .Include(a => a.Answers)
                .ThenInclude(a => a.ChosenOptions)
                .Include(a => a.Answers)
                .ThenInclude(a => a.Question)
                .ThenInclude(q => q.ChoiceOptions)
                .Where(a => a.TestId == testPk)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
            return true;
        }
    }
}
