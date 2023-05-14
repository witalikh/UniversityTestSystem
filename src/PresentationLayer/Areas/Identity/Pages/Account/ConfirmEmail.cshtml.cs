// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BusinessLayer.Services.Interfaces;

namespace PresentationLayer.Areas.Identity.Pages.Account
{
#nullable disable

    using System.Text;
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;

    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IInvitationManagerService _invitationManagerService;

        public ConfirmEmailModel(
            UserManager<UserEntity> userManager,
            IInvitationManagerService invitationManagerService
            )
        {
            this._userManager = userManager;
            this._invitationManagerService = invitationManagerService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound($"Unable to load userEntity with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await this._userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await this._invitationManagerService.BindInvitationsAfterSignUpAsync(user);
            }
            this.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return this.Page();
        }
    }
}
