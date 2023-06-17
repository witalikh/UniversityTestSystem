namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface IAttemptRepository
    {
        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(AttemptEntity entity);

        public Task<bool> AddAsync(AttemptEntity entity);

        public Task<List<AttemptEntity>> FetchAll();

        public Task<AttemptEntity?> GetAsync(Guid id);

        public Task<int> GetUserAttemptsCount(int testPk, string userPk);

        public Task<List<AttemptEntity>> GetUserTestAttempts(int testPk, string userPk);

        public Task<List<AttemptEntity>> GetAllTestAttempts(int testPk);


    }
}
