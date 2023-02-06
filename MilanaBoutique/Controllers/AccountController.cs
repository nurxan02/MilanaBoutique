using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MilanaBoutique.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(register.Username);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = register.Username,
                    Firstname = register.Firstname,
                    Surname = register.Surname,
                    Email = register.Email,
                    Phone = register.Phone,
                    Country = register.Country,
                    City = register.City,
                    Zip = register.Zip,
                    Address = register.Address


                };
                if (register.Username == null)
                {
                    ModelState.AddModelError("Username", "Please fill this field");
                    return View();

                }
                if (!register.TermsAndConditions)
                {
                    ModelState.AddModelError("TermsAndConditions", "Please fill this box!");
                    return View();
                }
                IdentityResult result = await _userManager.CreateAsync(user, register.Password);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }

            }
            else
            {
                ModelState.AddModelError("", "This username already taken");
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("finalprojectcodeacademy@gmail.com", "Milana Boutique");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Email Verification";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/VerifyEmail.html"))
            {
                body = reader.ReadToEnd();
            }

            mail.Body = body.Replace("{{link}}", link);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("finalprojectcodeacademy@gmail.com", "shldhrtkghufzhmg");
            smtp.Send(mail);
            TempData["Verify"] = true;
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            if (_context.BlacklistTokens.Any(c=>c.Token.ToLower().Trim() == token.ToLower().Trim()))
            {
                TempData["BlackToken"] = true;
                return RedirectToAction("Index", "Home");

            }
            BlacklistTokens blackToken = new BlacklistTokens
            {
                Token = token
            };
            _context.BlacklistTokens.Add(blackToken);
            _context.SaveChanges();

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            TempData["Verified"] = true;

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "username or password is incorrect");
                return View();
            }
            if (user.IsAdmin == true)
            {
                ModelState.AddModelError("", "username or password is incorrect");
                return View();
            }
            if (user.IsBlocked == true)
            {
                ModelState.AddModelError("", "You have blocked, contact with admin!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, login.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "you are blocked for 5 minutes");
                    return View();
                }
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(AccountVM account)
        {
            AppUser user = await _userManager.FindByEmailAsync(account.AppUser.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "No account exists with this email!");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
          

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("finalprojectcodeacademy@gmail.com", "Milana Boutique Reset Password!");
            mail.To.Add(new MailAddress(user.Email));

            mail.Subject = "Reset Password";
            mail.Body = $"<a href='{link}'>Reset password</a>";
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("finalprojectcodeacademy@gmail.com", "shldhrtkghufzhmg");
            smtp.Send(mail);
            TempData["Verify"] = true;
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {


            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            AccountVM model = new AccountVM
            {
                AppUser = user,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AccountVM account)
        {

            AppUser user = await _userManager.FindByEmailAsync(account.AppUser.Email);
            AccountVM model = new AccountVM
            {
                AppUser = user,
                Token = account.Token
            };
            if (!ModelState.IsValid) return View(model);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, account.Token, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            TempData["Reseted"] = true;

            return RedirectToAction("Index", "Home");
        }



        [Authorize]
        public async Task<IActionResult> Edit()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            UserEditVM editedUser = new UserEditVM
            {
                Username = user.UserName,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                Phone = user.Phone,
                Zip = user.Zip,
                Email = user.Email,
                Firstname = user.Firstname,
                Surname = user.Surname,
                Order = _context.Orders.OrderByDescending(o=>o.Id).Include(o=>o.OrderItems).ThenInclude(o=>o.ProductSizeColor).ThenInclude(p=>p.Size).Include(p=>p.OrderItems).ThenInclude(p=>p.ProductSizeColor).ThenInclude(pc=>pc.Color).Include(p=>p.OrderItems).ThenInclude(p=>p.ProductSizeColor).ThenInclude(p=>p.Product).ThenInclude(p=>p.Gender).Include(o=>o.Status).Where(o=>o.AppUserId == user.Id).ToList()
            };

            return View(editedUser);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM editedUser)
        {
            if (!ModelState.IsValid) return View(editedUser);

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEditVM eUser = new UserEditVM
            {
                Username = user.UserName,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                Phone = user.Phone,
                Zip = user.Zip,
                Email = user.Email,
                Firstname = user.Firstname,
                Surname = user.Surname,
                Order = _context.Orders.OrderByDescending(o => o.Id).Include(o => o.OrderItems).ThenInclude(o => o.ProductSizeColor).ThenInclude(p => p.Size).Include(p => p.OrderItems).ThenInclude(p => p.ProductSizeColor).ThenInclude(pc => pc.Color).Include(p => p.OrderItems).ThenInclude(p => p.ProductSizeColor).ThenInclude(p => p.Product).ThenInclude(p => p.Gender).Include(o => o.Status).Where(o => o.AppUserId == user.Id).ToList()

            };

            if (user.UserName != editedUser.Username && await _userManager.FindByNameAsync(editedUser.Username) != null)
            {
                ModelState.AddModelError("", $"{editedUser.Username} has already taken");
                return View(eUser);
            }
            if (user.Email != editedUser.Email && await _userManager.FindByEmailAsync(editedUser.Email) != null)
            {
                ModelState.AddModelError("", $"{editedUser.Email} has already taken");
                return View(eUser);
            }

            if (string.IsNullOrWhiteSpace(editedUser.CurrentPassword))
            {
                user.UserName = editedUser.Username;
                user.Email = editedUser.Email;
                user.Firstname = editedUser.Firstname;
                user.Surname = editedUser.Surname;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                user.UserName = editedUser.Username;
                user.Email = editedUser.Email;
                user.Firstname = editedUser.Firstname;
                user.Surname = editedUser.Surname;

                IdentityResult result = await _userManager.ChangePasswordAsync(user, editedUser.CurrentPassword, editedUser.Password);

                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return View(eUser);
                }
            }
            await _signInManager.SignInAsync(user,true);

            return RedirectToAction("index", "home");

        }




    }
}
