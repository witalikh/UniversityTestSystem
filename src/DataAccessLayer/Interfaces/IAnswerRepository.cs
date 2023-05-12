namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface IAnswerRepository
    {
        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(AnswerEntity entity);

        public Task<bool> AddAsync(AnswerEntity entity);

        public Task<List<AnswerEntity>> FetchAll();

        public Task<AnswerEntity?> GetAsync(int id);
    }
}
