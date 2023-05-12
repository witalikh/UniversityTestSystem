namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class TestRepository : ITestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TestEntity> _set;

        public TestRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<TestEntity>();
        }

        public async Task<int> DeleteAllQuestionsAsync(int testPk)
        {
            var questions = await this._set
                .Where(t => t.Id == testPk)
                .Include(t => t.Questions)
                .SelectMany(t => t.Questions)
                .ToListAsync();

            foreach (var _question in questions)
            {
                await this._context.QuestionChoiceOptions
                    .Where(m => m.QuestionId == _question.Id)
                    .ExecuteDeleteAsync();
            }

            return await this._context.Questions
                .Where(t => t.TestId == testPk)
                .ExecuteDeleteAsync();
        }

        public async Task<List<TestEntity>> GetAllTestByClassroomAsync(int classroomPk)
        {
            return await this._set
                .Where(m => m.ClassroomId == classroomPk)
                .Include(m => m.ClassroomEntity)
                .Include(m => m.Questions)
                .ThenInclude(t => t.ChoiceOptions)
                .ToListAsync();
        }

        public async Task<TestEntity?> GetTestAsync(int classroomPk, int testPk)
        {
            return await this._set
                .Where(m => m.ClassroomId == classroomPk && m.Id == testPk)
                .Include(m => m.ClassroomEntity)
                .Include(m => m.Questions)
                .ThenInclude(t => t.ChoiceOptions)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddAsync(TestEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(TestEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TestEntity>> FetchAll()
        {
            return await this._set.ToListAsync();
        }

        public async Task<TestEntity?> GetAsync(int id)
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
