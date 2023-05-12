using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class InvitationRepository: IInvitationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<InvitationEntity> _set;

    public InvitationRepository(ApplicationDbContext context)
    {
        this._context = context;
        this._set = this._context.Set<InvitationEntity>();
    }

    public async Task<bool> SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(InvitationEntity entity)
    {
        this._set.Remove(entity);
        await this._context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddAsync(InvitationEntity entity)
    {
        this._set.Add(entity);
        await this._context.SaveChangesAsync();
        return true;
    }

    public async Task<List<InvitationEntity>> FetchAll()
    {
        return await this._set.ToListAsync();
    }

    public async Task<InvitationEntity?> GetAsync(Guid id)
    {
        return await this._set.FindAsync(id);
    }

    public async Task<List<InvitationEntity>> GetAllInvitationsForEmail(string email)
    {
        return await this._set
            .Where(t => t.Email == email)
            .Include(t => t.ClassroomEntity)
            .ToListAsync();
    }

    public async Task<List<InvitationEntity>> GetAllInvitationsForUser(string userPk)
    {
        return await this._set
            .Where(t => t.UserId == userPk)
            .ToListAsync();
    }

    public async Task<List<InvitationEntity>> GetAllInvitationsForClassroom(int classroomPk)
    {
        return await this._set
            .Where(t => t.ClassroomId == classroomPk)
            .ToListAsync();
    }

    public async Task<InvitationEntity?> GetAllInvitationsByIdAndUser(string userPk, Guid id)
    {
        return await this._set
            .Where(t => t.UserId == userPk && t.uuid == id)
            .FirstOrDefaultAsync();
    }

    public async Task<InvitationEntity?> GetAllInvitationsByIdAndClassroom(int classroomPk, Guid id)
    {
        return await this._set
            .Where(t => t.ClassroomId == classroomPk && t.uuid == id)
            .FirstOrDefaultAsync();
    }

    public async Task<InvitationEntity?> GetInvitationByClassroomAndEmail(int classroomPk, string email)
    {
        return await this._set
            .Where(t => t.ClassroomId == classroomPk && t.Email == email)
            .FirstOrDefaultAsync();
    }
}
