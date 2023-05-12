namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface IQuestionChosenOptionRepository
    {
        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(QuestionChoiceOptionEntity entity);

        public Task<bool> AddAsync(QuestionChoiceOptionEntity entity);

        public Task<List<QuestionChoiceOptionEntity>> FetchAll();

        public Task<QuestionChoiceOptionEntity?> GetAsync(int id);
    }
}
