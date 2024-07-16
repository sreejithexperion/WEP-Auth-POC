using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        ViewData["Title"] = "Login";
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginInputModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginInputModel model, string button)
    {
        if (button != "login")
        {
            return Redirect(model.ReturnUrl);
        }

        if (ModelState.IsValid)
        {
            var user = Config.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            if (user != null)
            {
                AuthenticationProperties props = null;
                if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                    };
                };

                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, user.SubjectId),
                    new Claim(JwtClaimTypes.Name, user.Username)
                };
                claims.AddRange(user.Claims);

                var claimsIdentity = new ClaimsIdentity(claims, "password");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal, props);
                return Redirect(model.ReturnUrl);
            }

            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
        }

        ViewData["Title"] = "Login";
        ViewData["ReturnUrl"] = model.ReturnUrl;
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Logout()
    {
         return SignOut();
    }

    // [HttpPost]
    // [AllowAnonymous]
    // public async Task<IActionResult> Logout()
    // {
    //     return SignOut("Cookies", "oidc");
    // }
}
