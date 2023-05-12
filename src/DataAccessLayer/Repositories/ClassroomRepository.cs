namespace DataAccessLayer.Repositories
{
    using DataAccessLayer.Enums;
    using DataAccessLayer.Interfaces;
    using DataAccessLayer.Models;
    using Microsoft.EntityFrameworkCore;

    public class ClassroomRepository : IClassroomRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<ClassroomEntity> _set;

        public ClassroomRepository(ApplicationDbContext context)
        {
            this._context = context;
            this._set = this._context.Set<ClassroomEntity>();
        }

        public async Task<ClassroomEntity?> GetByIdAsync(int id)
        {
            return await this._set.Include(m => m.Members).ThenInclude(m => m.UserEntity).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ClassroomEntity?> GetClassroomByIdAndUserAsync(int id, string userPk)
        {
            return await this._context.Memberships
                .Where(m => m.UserEntityId == userPk && m.ClassroomEntityId == id)
                .Include(m => m.ClassroomEntity)
                .Select(m => m.ClassroomEntity)
                .Include(m => m.Members)
                .ThenInclude(m => m.UserEntity)
                .FirstOrDefaultAsync();
        }

        public async Task<ClassroomEntity?> GetClassroomByIdAndUserAndRoleAsync(
            int id,
            string userPk,
            MembershipRole role)
        {
            if (role == MembershipRole.Creator)
            {
                return await this._set
                    .Where(m => m.Id == id && m.OwnerId == userPk)
                    .Include(m => m.Members)
                    .ThenInclude(m => m.UserEntity)
                    .FirstOrDefaultAsync();
            }

            return await this._set
                .Where(m => m.Id == id)
                .Where(m => m.Members.Any(m => m.UserEntityId == userPk))
                .Include(m => m.Members)
                .ThenInclude(m => m.UserEntity)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ClassroomEntity>> GetAllClassroomsByUserAsync(string pk)
        {
            return await this._context.Classrooms
                .Where(m => m.OwnerId == pk || m.Members.Any(m => m.UserEntityId == pk))
                .Include(m => m.Members)
                .ThenInclude(m => m.UserEntity)
                .ToListAsync();
        }

        public async Task<List<ClassroomEntity>> GetAllClassroomsByUserAndRoleAsync(string pk, MembershipRole role)
        {
            if (role == MembershipRole.Creator)
            {
                return await this._set
                    .Where(m => m.OwnerId == pk)
                    .Include(m => m.Members)
                    .ThenInclude(m => m.UserEntity)
                    .ToListAsync();
            }

            return await this._set
                .Where(m => m.Members.Any(m => m.UserEntityId == pk))
                .Include(m => m.Members)
                .ThenInclude(m => m.UserEntity)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(ClassroomEntity entity)
        {
            this._set.Add(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(ClassroomEntity entity)
        {
            this._set.Remove(entity);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClassroomEntity>> FetchAll()
        {
            return await this._set.Include(m => m.Members).ThenInclude(m => m.UserEntity).ToListAsync();
        }

        public async Task<ClassroomEntity?> GetAsync(int id)
        {
            return await this._set
                .Include(c => c.Members)
                .ThenInclude(m => m.UserEntity)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsTestInClassroomAsync(int testPk, int classroomPk)
        {
            return await this._context.Tests
                .AnyAsync(t => t.Id == testPk && t.ClassroomId == classroomPk);
        }

        public async Task<bool> AddUserIntoClassroom(int classroomId, string userPk)
        {
            this._context.Memberships.Add(new MembershipEntity()
            {
                ClassroomEntityId = classroomId,
                UserEntityId = userPk,
            });

            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
            return true;
        }
    }
}