namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<QuestionEntity> _set;

        public QuestionRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<QuestionEntity>();
        }

        public async Task<bool> AddAsync(QuestionEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(QuestionEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<int> DeleteAllQuestionOptionsAsync(int questionPk)
        {
            return await this._set
                .Where(t => t.Id == questionPk)
                .Include(t => t.ChoiceOptions)
                .SelectMany(t => t.ChoiceOptions)
                .ExecuteDeleteAsync();
        }

        public async Task<QuestionEntity?> GetAsync(int id)
        {
            return await this._set.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionEntity>> GetAllQuestionsByTestAsync(int testPk)
        {
            return await this._set
                .Where(m => m.TestId == testPk)
                .Include(m => m.ChoiceOptions)
                .ToListAsync();
        }

        public async Task<QuestionEntity?> GetQuestionAsync(int testPk, int questionPk)
        {
            return await this._set
                .Where(m => m.TestId == testPk && m.Id == questionPk)
                .Include(m => m.ChoiceOptions)
                .FirstOrDefaultAsync();
        }
    }
}
