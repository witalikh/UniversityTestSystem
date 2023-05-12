namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class QuestionChosenOptionRepository : IQuestionChosenOptionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<QuestionChoiceOptionEntity> _set;

        public QuestionChosenOptionRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<QuestionChoiceOptionEntity>();
        }

        public async Task<bool> AddAsync(QuestionChoiceOptionEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(QuestionChoiceOptionEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionChoiceOptionEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<QuestionChoiceOptionEntity?> GetAsync(int id)
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
