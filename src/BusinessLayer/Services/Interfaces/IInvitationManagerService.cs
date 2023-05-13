using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;

namespace BusinessLayer.Services.Interfaces
{
    public interface IInvitationManagerService
    {
        public Task BindInvitationsAfterSignUpAsync(UserEntity user);

        public Task<bool> InviteUserIntoClassroom(int classroomPk, string Email);

        public Task<bool> RevokeUserInvitesFromClassroom(int classroomPk, string Email);

        public Task<List<InvitationEntity>> GetInvitationsByUser(string userPk);

        public Task<List<InvitationEntity>> GetInvitationsByClassroom(int classroomPk);

        public Task<InvitationEntity?> GetInvitationEntityByUserAndId(string userPk, Guid uuid);

        public Task<InvitationEntity?> GetInvitationEntityByClassroomAndId(int classroomPk, Guid uuid);

        public Task<bool> AcceptInvitation(string userPk, Guid id);

        public Task<bool> DeclineInvitation(string userPk, Guid id);
    }
}
