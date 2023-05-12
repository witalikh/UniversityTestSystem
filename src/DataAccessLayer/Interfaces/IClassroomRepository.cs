namespace DataAccessLayer.Interfaces
{
    using DataAccessLayer.Enums;
    using DataAccessLayer.Models;

    public interface IClassroomRepository
    {

        public Task<ClassroomEntity?> GetByIdAsync(int id);

        public Task<ClassroomEntity?> GetClassroomByIdAndUserAsync(int id, string userPk);

        public Task<ClassroomEntity?> GetClassroomByIdAndUserAndRoleAsync(int id, string userPk, MembershipRole role);

        public Task<List<ClassroomEntity>> GetAllClassroomsByUserAsync(string pk);

        public Task<List<ClassroomEntity>> GetAllClassroomsByUserAndRoleAsync(string pk, MembershipRole role);

        public Task<bool> SaveChangesAsync();

        public Task<bool> DeleteAsync(ClassroomEntity entity);

        public Task<bool> AddAsync(ClassroomEntity entity);

        public Task<List<ClassroomEntity>> FetchAll();

        public Task<ClassroomEntity?> GetAsync(int id);

        public Task<bool> IsTestInClassroomAsync(int testPk, int classroomPk);

        public Task<bool> AddUserIntoClassroom(int classroomId, string userPk);
    }
}
