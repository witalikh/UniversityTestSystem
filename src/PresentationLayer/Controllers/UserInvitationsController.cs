using BusinessLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Controllers
{
    using BusinessLayer.Services.Interfaces;
    using DataAccessLayer;
    using DataAccessLayer.Enums;
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class UserInvitationsController : BaseController
    {
        private readonly IInvitationManagerService _invitationManagerService;
        private readonly ILogger<UserInvitationsController> _logger;


        public UserInvitationsController(
            UserManager<UserEntity> manager,
            IInvitationManagerService invitationManagerService,
            ITestManagementService testManagementService,
            ILogger<UserInvitationsController> logger)
            : base(manager)
        {
            this._invitationManagerService = invitationManagerService;
            this._logger = logger;
        }

        [Authorize]
        [Route("me/Invitations", Name = "invitations-by-user-index")]
        public async Task<IActionResult> Index()
        {
            string userId = this.GetUserId() !;

            var invites = await this._invitationManagerService.GetInvitationsByUser(userId);
            return this.View(invites.Select(t => new InvitationViewModel(t)).ToList());
        }

        [Authorize]
        [Route("me/Invitations/{id}", Name = "invitations-by-user-details")]
        public async Task<IActionResult> Details(Guid id)
        {
            string userId = this.GetUserId() !;

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByUserAndId(userId, id);
            if (invitationEntity == null)
            {
                return this.NotFound();
            }

            return this.View(new InvitationViewModel(invitationEntity));
        }

        // POST: Invitations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("me/Invitations/Accept/{id}", Name = "invitations-by-user-accept-post")]
        public async Task<IActionResult> AcceptInvite(Guid id)
        {
            string userId = this.GetUserId() !;

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByUserAndId(userId, id);

            if (invitationEntity != null)
            {
                await this._invitationManagerService.AcceptInvitation(userId, id);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [Authorize]
        [Route("me/Invitations/Delete/{id}", Name = "invitations-by-user-delete-get")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            string userId = this.GetUserId() !;

            if (id == null)
            {
                return this.NotFound();
            }

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByUserAndId(userId, id.Value);
            if (invitationEntity == null)
            {
                return this.NotFound();
            }

            return this.View(new InvitationViewModel(invitationEntity));
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("me/Invitations/Delete/{id}", Name = "invitations-by-user-delete-post")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            string userId = this.GetUserId() !;

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByUserAndId(userId, id);

            if (invitationEntity != null)
            {
                await this._invitationManagerService.DeclineInvitation(userId, id);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
