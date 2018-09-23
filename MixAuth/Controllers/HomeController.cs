using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixAuth.Models;

namespace MixAuth.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginViewModel model, string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                bool succee = (model.UserName == "admin") && (model.Password == "123");

                if (succee)
                {
                    //创建用户身份标识
                    var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaims(new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid, model.UserName),
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim(ClaimTypes.Role, "admin"),
                    });

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Redirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "帐号或者密码错误。");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Home/Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        [HttpGet]
        public string GetToken(string userName, string password)
        {
            bool success = ((userName == "user") && (password == "111"));
            if (!success)
                return "";

            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();

            //创建用户身份标识
            var claims = new Claim[] 
            {
                new Claim(ClaimTypes.Sid, userName),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "user"),
            };

            //创建令牌
            var token = new JwtSecurityToken(
                issuer: jwtTokenOptions.Issuer,
                audience: jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: jwtTokenOptions.Credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

    }
}
