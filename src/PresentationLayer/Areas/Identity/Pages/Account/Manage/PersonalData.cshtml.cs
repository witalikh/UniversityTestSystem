// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace PresentationLayer.Areas.Identity.Pages.Account.Manage
{
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<UserEntity> userManager,
            ILogger<PersonalDataModel> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load userEntity with ID '{this._userManager.GetUserId(this.User)}'.");
            }

            return this.Page();
        }
    }
}
