using Login.Demo.Domain;
using Login.Demo.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Login.Demo.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _myDbContext;
        public UserController(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            //判断 一下帐号密码的正确性，
            if (await _myDbContext.Users.AnyAsync(a => a.Account == request.Account && a.Password == request.Password))
            {
                //进行登录授权

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,request.Account)
                };
                var claimnsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //它会自动发送token给客户端。并生成cookies
                await HttpContext
                    .SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimnsIdentity),
                    new AuthenticationProperties { 
                        IsPersistent=true
                    });
            }
            else
            {
                return RedirectToAction(nameof(Login));

            }
            //成功的话。跳转到returnurl上
            return Redirect(request.ReturnUrl ?? "/");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }

    public class UserLoginRequest : User
    {
        public string ReturnUrl { get; set; }
    }
}
