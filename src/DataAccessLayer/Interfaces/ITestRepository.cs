namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Models;

    public interface ITestRepository
    {
        public Task<int> DeleteAllQuestionsAsync(int testPk);

        public Task<List<TestEntity>> GetAllTestByClassroomAsync(int classroomPk);

        public Task<TestEntity?> GetTestAsync(int classroomPk, int testPk);

        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(TestEntity entity);

        public Task<bool> AddAsync(TestEntity entity);

        public Task<List<TestEntity>> FetchAll();

        public Task<TestEntity?> GetAsync(int id);
    }
}
