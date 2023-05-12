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
    }
}
