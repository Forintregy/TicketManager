using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TicketsWebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }
    }
}
