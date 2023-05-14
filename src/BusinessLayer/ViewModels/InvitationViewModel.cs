using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLayer.ViewModels
{
    public class InvitationViewModel
    {
        public Guid Uuid { get; set; } = Guid.Empty;

        public string Email { get; set; } = string.Empty;

        public int ClassroomId { get; set; } = 0;

        public string? UserId { get; set; } = null;

        public InvitationStatus InvitationStatus { get; set; } = InvitationStatus.Pending;

        public InvitationViewModel() { }

        public InvitationViewModel(InvitationEntity entity)
        {
            this.Uuid = entity.uuid;

            this.Email = entity.Email;

            this.ClassroomId = entity.ClassroomId;

            this.UserId = entity.UserId;

            this.InvitationStatus = entity.InvitationStatus;
        }

        public static InvitationEntity ToEntity(InvitationViewModel viewModel)
        {
            return new InvitationEntity
            {
                uuid = viewModel.Uuid,
                Email = viewModel.Email,
                ClassroomId = viewModel.ClassroomId,
                UserId = viewModel.UserId,
                InvitationStatus = viewModel.InvitationStatus,
            };
        }
    }

}
