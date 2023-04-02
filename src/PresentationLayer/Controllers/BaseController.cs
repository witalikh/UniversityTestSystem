namespace PresentationLayer.Controllers
{
    using DataAccessLayer;
    using DataAccessLayer.Enums;
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected readonly UserManager<UserEntity> _userManager;

        public BaseController(UserManager<UserEntity> manager)
        {
            this._userManager = manager;
        }

        protected string? GetUserId()
        {
            return this._userManager.GetUserId(this.User);
        }

        protected async Task<UserEntity?> GetUser()
        {
            return await this._userManager.GetUserAsync(this.User);
        }

        protected MembershipRole? GetUserRoleBelongingToClassroom(ClassroomEntity classroomEntity, string UserId)
        {
            if (classroomEntity.OwnerId == UserId)
            {
                return MembershipRole.Creator;
            }

            if (classroomEntity.Members.Any(m => m.UserEntityId == UserId))
            {
                return MembershipRole.Student;
            }
            return null;
        }
    }
}
