using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppTemplate.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        //REMEMBER TO NORMALISE EMAILS


        //login and register page
        [HttpGet("")]
        public IActionResult Index(AuthViewModel? vm)
        {
            if (vm == null)
            {
                vm = new AuthViewModel();
            }

            return View(vm);
        }

        [HttpPost("[action]")]
        public IActionResult Login(AuthViewModel vm)
        {
            try
            {
                vm.Email = vm.Email.ToLower();
                //verify user exists
                //validate password
                var success = true;
                if (success)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim("id", "69"));
                    claims.Add(new Claim("role", "admin"));

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = true;
                    authProperties.AllowRefresh = true;
                    authProperties.ExpiresUtc = DateTime.Now.AddMonths(3);

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties).Wait();

                    //sign in or redirect to auth with error
                    return RedirectToAction("Index", "Home");
                }

                vm.Error = "Login failed";

                return RedirectToAction("Index", vm);
            }
            catch (Exception e)
            {
                //create an exception handler to be used throughout entire project
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                //create an exception handler to be used throughout entire project
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost("[action]")]
        public IActionResult Register(AuthViewModel vm)
        {
            try
            {
                vm.Email = vm.Email.ToLower();
                var success = true;

                if (success)
                {
                    //create claim
                    var claims = new List<Claim>();
                    claims.Add(new Claim("id", "69"));
                    claims.Add(new Claim("role", "admin"));

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = true;
                    authProperties.AllowRefresh = true;
                    authProperties.ExpiresUtc = DateTime.Now.AddMonths(3);

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties).Wait();

                    //redirect to home
                    return RedirectToAction("Index", "Home");
                }

                vm.Error = "Registration has failed";

                return RedirectToAction("Index", vm);
            }
            catch (Exception e)
            {
                //create an exception handler to be used throughout entire project
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class AuthViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
    }
}
