using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementations
{
    public class ClassroomManagementService : IClassroomManagementService
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly ILogger<ClassroomManagementService> _logger;

        public ClassroomManagementService(
            IClassroomRepository classroomRepository,
            ILogger<ClassroomManagementService> logger)
        {
            this._classroomRepository = classroomRepository;
            this._logger = logger;
        }

        public async Task<bool> AddClassroomAsync(ClassroomViewModel classroomData)
        {
            ClassroomEntity classroomEntity = ClassroomViewModel.ToEntity(classroomData);
            return await this._classroomRepository.AddAsync(classroomEntity);
        }

        public async Task<bool> EditClassroomAsync(ClassroomEntity old, ClassroomViewModel data)
        {
            try
            {
                old.Name = data.Name;
                old.Description = data.Description;

                return await this._classroomRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in EditClassroomAsync in ClassroomManagementService.cs");
                return false;
            }
        }

        public async Task<bool> DeleteClassroomAsync(ClassroomEntity classroomEntity)
        {
            try
            {
                return await this._classroomRepository.DeleteAsync(classroomEntity);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in DeleteClassroomAsync in ClassroomManagementService.cs");
                return false;
            }
        }

        public async Task<ClassroomEntity?> GetClassroomByIdAsync(int id)
        {
            try
            {
                return await this._classroomRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetClassroomByIdAsync in ClassroomManagementService.cs");
                return null;
            }
        }

        public async Task<ClassroomEntity?> GetClassroomByIdAndUserAsync(int id, string userPk)
        {
            try
            {
                return await this._classroomRepository.GetClassroomByIdAndUserAsync(id, userPk);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetClassroomByIdAndUserAsync in ClassroomManagementService.cs");
                return null;
            }
        }

        public async Task<ClassroomEntity?> GetOwnedClassroomByIdAndUserAsync(int id, string userPk)
        {
            try
            {
                return await this._classroomRepository.GetClassroomByIdAndUserAndRoleAsync(id, userPk, MembershipRole.Creator);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetClassroomByIdAndUserAsync in ClassroomManagementService.cs");
                return null;
            }
        }

        public async Task<List<ClassroomEntity>> GetClassroomListByUserAsync(string pk)
        {
            try
            {
                return await this._classroomRepository.GetAllClassroomsByUserAsync(pk);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetAllClassroomsByUserAsync in ClassroomManagementService.cs");
                return new List<ClassroomEntity>();
            }
        }

        public async Task<List<ClassroomEntity>> GetClassroomListByUserAndRoleAsync(string pk, MembershipRole role)
        {
            try
            {
                return await this._classroomRepository.GetAllClassroomsByUserAndRoleAsync(pk, role);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Caught exception in GetAllClassroomsByUserAndRoleAsync in ClassroomManagementService.cs");
                return new List<ClassroomEntity>();
            }
        }

        public async Task<MembershipRole?> GetMemberRoleAsync(int id, string userPk)
        {
            var entity = await this._classroomRepository.GetAsync(id);
            if (entity == null)
            {
                return null;
            }

            if (entity.OwnerId == userPk)
            {
                return MembershipRole.Creator;
            }

            if (entity.Members.Any(m => m.UserEntityId == userPk))
            {
                return MembershipRole.Student;
            }

            return null;
        }

        public async Task<bool> IsTestInClassroomAsync(int testPk, int classroomPk)
        {
            return await this._classroomRepository.IsTestInClassroomAsync(testPk, classroomPk);
        }
    }
}
