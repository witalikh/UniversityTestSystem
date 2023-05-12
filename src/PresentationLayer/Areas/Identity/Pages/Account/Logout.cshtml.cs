// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
namespace PresentationLayer.Areas.Identity.Pages.Account
{
#nullable disable

    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<UserEntity> signInManager, ILogger<LogoutModel> logger)
        {
            this._signInManager = signInManager;
            this._logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await this._signInManager.SignOutAsync();
            this._logger.LogInformation("UserEntity logged out.");
            if (returnUrl != null)
            {
                return this.LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the userEntity gets updated.
                return this.RedirectToPage();
            }
        }
    }
}
