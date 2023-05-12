namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface IQuestionRepository
    {
        public Task<bool> SaveChangesAsync();

        public Task<List<QuestionEntity>> GetAllQuestionsByTestAsync(int testPk);

        public Task<QuestionEntity?> GetQuestionAsync(int testPk, int questionPk);

        public Task<bool> DeleteAsync(QuestionEntity entity);

        public Task<bool> AddAsync(QuestionEntity entity);

        public Task<int> DeleteAllQuestionOptionsAsync(int questionPk);

        public Task<List<QuestionEntity>> FetchAll();

        public Task<QuestionEntity?> GetAsync(int id);
    }
}
