namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface IUserRepository
    {

        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(UserEntity entity);

        public Task<bool> AddAsync(UserEntity entity);

        public Task<UserEntity?> GetUserByEmail(string email);

        public Task<List<UserEntity>> FetchAll();

        public Task<UserEntity?> GetAsync(string id);
    }
}
