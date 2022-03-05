#nullable disable

namespace AMS.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Options;

    using AMS.Data.Models;
    using AMS.Services.Models;

    using static AMS.Data.Constants.DataConstants;
    using static AMS.Web.Areas.Admin.Constants.AdminConstants;


    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly AppSettingsServiceModel adminDetails;

        public LoginModel(SignInManager<User> signInManager,
            UserManager<User> userManager,
            IOptions<AppSettingsServiceModel> adminDetails)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.adminDetails = adminDetails.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(UserConstants.UsernameMaxLength,
                MinimumLength = UserConstants.UsernameMinLength,
                ErrorMessage = "{0} must be between {2} and {1} characters long!")]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember Me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Input.UserName);

                if (user != null)
                {
                    if (user.IsSuspended)
                    {
                        ModelState.AddModelError(string.Empty, "Your account has been suspended!");

                        return Page();
                    }
                }

                var result = await this.signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (Input.UserName == adminDetails.Username)
                    {
                        return RedirectToAction("Index", "Home", new { area = AdminAreaName });
                    }

                    return LocalRedirect(returnUrl);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return Page();
                }
            }

            return Page();
        }
    }
}
