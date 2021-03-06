﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using palikohrnne_web_app.Api;
using palikohrnne_web_app.Extensions;
using palikohrnne_web_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Controllers
{
    public class AccountController : Controller
    {

        private readonly CubesService _cubesService;


        public class LoginSate
        {
            public bool IsConnected { get; set; }
            public string Rang { get; set; }
            public string FullName { get; set; }
            public string EMail { get; set; }
            public int CitoyenID { get; set; }
            public string Pseudo { get; set; }
        }

        private readonly ILogger<AccountController> _logger;
        public AccountController(CubesService cubesService, ILogger<AccountController> logger)
        {
            _cubesService = cubesService;
            _logger = logger;
        }
        
        public async Task<IActionResult> IndexAsync()
        {
            
            var citoyens = await _cubesService.GetAllCitoyens();
            return View(citoyens);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(Citoyen obj)
        {
            await _cubesService.CreateCitoyen(obj);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel login, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                // Use Input.Email and Input.Password to authenticate the user
                // with your custom authentication logic.
                //
                // For demonstration purposes, the sample validates the user
                // on the email address maria.rodriguez@contoso.com with 
                // any password that passes model validation.

                var user = await AuthentificationAsync(login);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                LoginResponseModel loginResponse = await _cubesService.SeConnecter(new Citoyen
                {
                    Mail = login.UserEmail,
                    MotDePasse = login.UserPwd
                });

                #region snippet1
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.FullName),
                    new Claim(ClaimTypes.Name, user.EMail),
                    new Claim("FullName", user.FullName),
                    new Claim("Pseudo", user.Pseudo),
                    new Claim("ID", user.CitoyenID.ToString()),
                    new Claim("Token", loginResponse.token),
                    new Claim(ClaimTypes.Role, user.Rang),
                };


                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = true,
                    IssuedUtc = DateTime.UtcNow,
                    RedirectUri = "https://localhost:5001"
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                #endregion
                _logger.LogInformation("User {Email} logged in at {Time}.",
                    user.EMail, DateTime.UtcNow);

                if (!String.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                
                return RedirectToAction("Index","Home");
            }
            

            return View();
        }

        private async Task<LoginSate> AuthentificationAsync(LoginModel login)
        {
            var citoyens = await _cubesService.GetAllCitoyens();


            foreach (var citoyen in citoyens)
            {
                if (citoyen.Mail == login.UserEmail && citoyen.MotDePasse == login.UserPwd)
                {
                    var rang = await _cubesService.GetRangById(citoyen.RangID);
                    var rangName = rang.Nom;
                    return new LoginSate()
                    {
                        IsConnected = true,
                        Rang = rangName,
                        FullName = citoyen.Nom + " " + citoyen.Prenom,
                        EMail = citoyen.Mail,
                        CitoyenID = citoyen.ID,
                        Pseudo = citoyen.Pseudo
                    };
                }
            }

            return null;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
