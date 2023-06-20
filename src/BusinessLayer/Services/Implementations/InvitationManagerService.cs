using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.Implementations;

public class InvitationManagerService: IInvitationManagerService
{
    private IInvitationRepository _invitationRepository;
    private IClassroomRepository _classroomRepository;
    private IUserRepository _userRepository;
    private ILogger<InvitationManagerService> _logger;

    public InvitationManagerService(
        IInvitationRepository invitationRepository,
        IClassroomRepository classroomRepository,
        IUserRepository userRepository,
        ILogger<InvitationManagerService> logger)
    {
        this._invitationRepository = invitationRepository;
        this._classroomRepository = classroomRepository;
        this._userRepository = userRepository;
        this._logger = logger;
    }

    public async Task BindInvitationsAfterSignUpAsync(UserEntity user)
    {
        string email = user.Email!;
        var pendingInvites = await this._invitationRepository.GetAllInvitationsForEmail(email);
        foreach (var invitationEntity in pendingInvites)
        {
            invitationEntity.User = user;
        }

        await this._invitationRepository.SaveChangesAsync();
        return;
    }

    public async Task<bool> InviteUserIntoClassroom(int classroomPk, InvitationEntity invite)
    {
        var user = await this._userRepository.GetUserByEmail(invite.Email);
        var newInvite = new InvitationEntity()
        {
            ClassroomId = classroomPk,
            Email = invite.Email,
            ExpirationDate = invite.ExpirationDate,
            User = user,
            InvitationStatus = InvitationStatus.Pending,
        };

        return await this._invitationRepository.AddAsync(newInvite);
    }

    public async Task<bool> RevokeUserInvitesFromClassroom(int classroomPk, string Email)
    {
        var invite = await this._invitationRepository.GetInvitationByClassroomAndEmail(classroomPk, Email);
        if (invite == null)
        {
            return false;
        }

        return await this._invitationRepository.DeleteAsync(invite);
    }

    public async Task<List<InvitationEntity>> GetInvitationsByUser(string userPk)
    {
        return await this._invitationRepository.GetAllInvitationsForUser(userPk);
    }

    public async Task<List<InvitationEntity>> GetInvitationsByClassroom(int classroomPk)
    {
        return await this._invitationRepository.GetAllInvitationsForClassroom(classroomPk);
    }

    public async Task<InvitationEntity?> GetInvitationEntityByUserAndId(string userPk, Guid uuid)
    {
        return await this._invitationRepository.GetAllInvitationsByIdAndUser(userPk, uuid);
    }

    public async Task<InvitationEntity?> GetInvitationEntityByClassroomAndId(int classroomPk, Guid uuid)
    {
        return await this._invitationRepository.GetAllInvitationsByIdAndClassroom(classroomPk, uuid);
    }

    public async Task<bool> AcceptInvitation(string userPk, Guid id)
    {
        var invite = await this._invitationRepository.GetAsync(id);
        if (invite == null || invite.UserId != userPk || invite.InvitationStatus != InvitationStatus.Pending)
        {
            return false;
        }

        if (invite.ExpirationDate > DateTime.UtcNow)
        {
            return false;
        }

        var classroom = await this._classroomRepository.GetAsync(invite.ClassroomId);
        if (classroom == null)
        {
            await this._invitationRepository.DeleteAsync(invite);
            return false;
        }

        invite.InvitationStatus = InvitationStatus.Accepted;
        await this._invitationRepository.SaveChangesAsync();

        return await this._classroomRepository.AddUserIntoClassroom(invite.ClassroomId, userPk);
    }

    public async Task<bool> DeclineInvitation(string userPk, Guid id)
    {
        var invite = await this._invitationRepository.GetAsync(id);
        if (invite == null || invite.UserId != userPk || invite.InvitationStatus != InvitationStatus.Pending)
        {
            return false;
        }

        var classroom = await this._classroomRepository.GetAsync(invite.ClassroomId);
        if (classroom == null)
        {
            await this._invitationRepository.DeleteAsync(invite);
            return false;
        }

        invite.InvitationStatus = InvitationStatus.Declined;
        await this._invitationRepository.SaveChangesAsync();

        return true;
    }
}