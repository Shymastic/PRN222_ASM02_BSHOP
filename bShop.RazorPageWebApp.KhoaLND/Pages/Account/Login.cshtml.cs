using bShop.Entities.KhoaLND.Models;
using bShop.Services.KhoaLND;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace bShop.RazorPageWebApp.KhoaLND.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IUserAccountService _userAccountService;
        public LoginModel(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var userAccount = _userAccountService.LoginAsync(Email, Password).Result;
                if (userAccount != null)
                {
                    var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, userAccount.UserName),
                                    new Claim(ClaimTypes.Role, userAccount.RoleId.ToString())
                                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    Response.Cookies.Append("UserName", userAccount.FullName);
                    Response.Cookies.Append("Role", userAccount.RoleId.ToString());

                    return Redirect("../Reviews");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Login failed: " + ex.Message);
            }
            return Page();


        }
    }
}
