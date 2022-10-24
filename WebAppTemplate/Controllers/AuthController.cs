using DAL.AppContext;
using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace WebAppTemplate.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        public AppDbContext dbContext { get; set; }

        public AuthController(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

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
        public async Task<IActionResult> Login(AuthViewModel vm)
        {
            try
            {
                vm.Email = vm.Email.ToLower();
                var error = "";

                //get user
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == vm.Email);

                //validate user exists
                if (user != null)
                {
                    if (user.Password != vm.Password)
                    {
                        error = "Incorrect password";
                        return RedirectToAction("Index", vm);
                    }

                    if (string.IsNullOrWhiteSpace(error))
                    {
                        var claims = new List<Claim>();
                        var role = user.Id == 1 ? "admin" : "user";
                        claims.Add(new Claim("id", user.Id.ToString()));
                        claims.Add(new Claim("role", role));

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
                }
                else
                {
                    error = $"User with email {vm.Email} not found";
                }

                vm.Error = error;

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
        public async Task<IActionResult> Register(AuthViewModel vm)
        {
            try
            {
                vm.Email = vm.Email.ToLower();
                var user = new User()
                {
                    Email = vm.Email.ToLower(),
                    Password = vm.Password,
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(user.Id.ToString()))
                {
                    //create claim
                    var claims = new List<Claim>();
                    var role = user.Id == 1 ? "admin" : "user";
                    claims.Add(new Claim("id", user.Id.ToString()));
                    claims.Add(new Claim("role", role));

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
