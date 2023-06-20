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

    public class ClassroomInvitationsController : BaseController
    {
        private readonly IClassroomManagementService _classroomManagementService;
        private readonly IInvitationManagerService _invitationManagerService;
        private readonly ILogger<ClassroomInvitationsController> _logger;

        private async Task<bool> IsClassroomOwner(string userPk, int classroomPk)
        {
            return await this._classroomManagementService.GetMemberRoleAsync(classroomPk, userPk) ==
                   MembershipRole.Creator;
        }

        public ClassroomInvitationsController(
            UserManager<UserEntity> manager,
            IClassroomManagementService classroomManagementService,
            IInvitationManagerService invitationManagerService,
            ITestManagementService testManagementService,
            ILogger<ClassroomInvitationsController> logger)
            : base(manager)
        {
            this._classroomManagementService = classroomManagementService;
            this._invitationManagerService = invitationManagerService;
            this._logger = logger;
        }

        // GET: Invitations
        [Authorize]
        [Route("{classroomPk}/Invitations", Name = "invitations-by-classroom-index")]
        public async Task<IActionResult> Index(int classroomPk)
        {
            string userId = this.GetUserId() !;

            if (!await this.IsClassroomOwner(userId, classroomPk))
            {
                return this.Forbid();
            }

            this.ViewBag.ClassroomId = classroomPk;
            var invites = await this._invitationManagerService.GetInvitationsByClassroom(classroomPk);
            return this.View(invites.Select(t => new InvitationViewModel(t)).ToList());
        }

        // GET: Invitations/Details/5
        [Authorize]
        [Route("{classroomPk}/Invitations/{id}", Name = "invitations-by-classroom-details")]
        public async Task<IActionResult> Details(int classroomPk, Guid id)
        {
            string userId = this.GetUserId() !;
            this.ViewBag.ClassroomId = classroomPk;

            if (!await this.IsClassroomOwner(userId, classroomPk))
            {
                return this.Forbid();
            }

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByClassroomAndId(classroomPk, id);
            if (invitationEntity == null)
            {
                return this.NotFound();
            }

            return this.View(new InvitationViewModel(invitationEntity));
        }

        // GET: Invitations/Create
        [Authorize]
        [Route("{classroomPk}/Invitations/Create", Name = "invitations-by-classroom-create-get")]
        public async Task<IActionResult> Create(int classroomPk)
        {
            string userId = this.GetUserId() !;
            this.ViewBag.ClassroomId = classroomPk;

            if (!await this.IsClassroomOwner(userId, classroomPk))
            {
                return this.Forbid();
            }

            return this.View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("{classroomPk}/Invitations/Create", Name = "invitations-by-classroom-create-post")]
        public async Task<IActionResult> Create(
            int classroomPk,
            [Bind("uuid,Email,ClassroomId,UserId,InvitationStatus,ExpirationDate")] InvitationEntity invitationEntity)
        {
            string userId = this.GetUserId() !;
            this.ViewBag.ClassroomId = classroomPk;

            if (!await this.IsClassroomOwner(userId, classroomPk))
            {
                return this.Forbid();
            }

            this.ModelState.Remove("ClassroomEntity");
            this.ModelState.Remove("UserId");
            if (this.ModelState.IsValid)
            {
                invitationEntity.uuid = Guid.NewGuid();
                invitationEntity.ClassroomId = classroomPk;
                bool created = await this._invitationManagerService.InviteUserIntoClassroom(classroomPk, invitationEntity);
                if (created)
                {
                    return this.RedirectToAction(nameof(this.Index), new { classroomPk });
                }
            }

            return this.View(new InvitationViewModel(invitationEntity));
        }

        // GET: Invitations/Delete/5
        [Authorize]
        [Route("{classroomPk}/Invitations/Delete/{id}", Name = "invitations-by-classroom-delete-get")]
        public async Task<IActionResult> Delete(int classroomPk, Guid? id)
        {
            string userId = this.GetUserId() !;
            this.ViewBag.ClassroomId = classroomPk;

            if (!await this.IsClassroomOwner(userId, classroomPk))
            {
                return this.Forbid();
            }

            if (id == null)
            {
                return this.NotFound();
            }

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByClassroomAndId(classroomPk, id.Value);
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
        [Route("{classroomPk}/Invitations/Delete/{id}", Name = "invitations-by-classroom-delete-post")]
        public async Task<IActionResult> DeleteConfirmed(int classroomPk, Guid id)
        {
            string userId = this.GetUserId() !;
            this.ViewBag.ClassroomId = classroomPk;

            if (!await this.IsClassroomOwner(userId, classroomPk))
            {
                return this.Forbid();
            }

            var invitationEntity =
                await this._invitationManagerService.GetInvitationEntityByClassroomAndId(classroomPk, id);

            if (invitationEntity != null)
            {
                await this._invitationManagerService.RevokeUserInvitesFromClassroom(classroomPk, invitationEntity.Email);
            }
            return this.RedirectToAction(nameof(this.Index), new { classroomPk });
        }
    }
}
