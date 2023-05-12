namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface IAnswerChosenOptionRepository
    {
        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(AnswerChosenOptionEntity entity);

        public Task<bool> AddAsync(AnswerChosenOptionEntity entity);

        public Task<List<AnswerChosenOptionEntity>> FetchAll();

        public Task<AnswerChosenOptionEntity?> GetAsync(int id);
    }
}
