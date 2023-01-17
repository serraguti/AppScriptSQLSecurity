using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppScriptSQLSecurity.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult>
            Login(string username, string password)
        {
            if (username.ToLower() == "admin" &&
                password.ToLower() == "admin")
            {
                //DEBEMOS CREAR UNA IDENTIDAD (name y role)
                //Y UN PRINCIPAL
                //DICHA IDENTIDAD DEBEMOS COMBINARLA CON LA COOKIE DE 
                //AUTENTIFICACION
                ClaimsIdentity identity =
                    new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);
                //TODO USUARIO PUEDE CONTENER UNA SERIE DE CARACTERISTICAS
                //LLAMADA CLAIMS.  DICHAS CARACTERISTICAS PODEMOS ALMACENARLAS
                //DENTRO DE USER PARA UTILIZARLAS A LO LARGO DE LA APP
                Claim claimUserName =
                    new Claim(ClaimTypes.Name, username);
                Claim claimRole =
                    new Claim(ClaimTypes.Role, "USUARIO");
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
                    , userPrincipal
                    , new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(15)
                    });
                return RedirectToAction("Index", "Timers");
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
