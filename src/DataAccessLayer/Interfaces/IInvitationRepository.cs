using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces;

public interface IInvitationRepository
{
    public Task<bool> SaveChangesAsync();

    public Task<bool> DeleteAsync(InvitationEntity entity);

    public Task<bool> AddAsync(InvitationEntity entity);

    public Task<List<InvitationEntity>> FetchAll();

    public Task<InvitationEntity?> GetAsync(Guid id);

    public Task<List<InvitationEntity>> GetAllInvitationsForEmail(string email);

    public Task<List<InvitationEntity>> GetAllInvitationsForUser(string userPk);

    public Task<List<InvitationEntity>> GetAllInvitationsForClassroom(int classroomPk);

    public Task<InvitationEntity?> GetAllInvitationsByIdAndUser(string userPk, Guid id);

    public Task<InvitationEntity?> GetAllInvitationsByIdAndClassroom(int classroomPk, Guid id);

    public Task<InvitationEntity?> GetInvitationByClassroomAndEmail(int classroomPk, string email);
}