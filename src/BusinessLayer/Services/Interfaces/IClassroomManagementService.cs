using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLayer.Services.Interfaces
{
    public interface IClassroomManagementService
    {
        public Task<bool> AddClassroomAsync(ClassroomViewModel classroomData);

        public Task<bool> EditClassroomAsync(ClassroomEntity toEdit, ClassroomViewModel classroomData);

        public Task<bool> DeleteClassroomAsync(ClassroomEntity classroomEntity);

        public Task<ClassroomEntity?> GetClassroomByIdAsync(int id);

        public Task<ClassroomEntity?> GetClassroomByIdAndUserAsync(int id, string userPk);

        public Task<ClassroomEntity?> GetOwnedClassroomByIdAndUserAsync(int id, string userPk);

        public Task<List<ClassroomEntity>> GetClassroomListByUserAsync(string pk);

        public Task<List<ClassroomEntity>> GetClassroomListByUserAndRoleAsync(string pk, MembershipRole role);

        public Task<MembershipRole?> GetMemberRoleAsync(int id, string userPk);

        public Task<bool> IsTestInClassroomAsync(int testPk, int classroomPk);
    }
}
