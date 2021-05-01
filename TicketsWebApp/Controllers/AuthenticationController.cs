using Microsoft.AspNetCore.Mvc;

namespace TicketsWebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Logout()
        {
            return new SignOutResult(new[] { "oidc", "Cookies"});
        }
    }
}
