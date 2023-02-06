using MilanaBoutique.DAL;
using MilanaBoutique.Models;
using MilanaBoutique.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilanaBoutique.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInResult;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInResult, AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInResult = signInResult;
        }


        [Authorize(Roles = "SuperAdmin")]
        public IActionResult UserList(int page=1)
        {
            ViewBag.Currentpage = page;
            ViewBag.Totalpage = Math.Ceiling((decimal)_context.Users.Count() / 10);

            List<AppUser> users = _userManager.Users.Skip((page-1)*10).Take(10).ToList();
            return View(users);
        }
        [Authorize(Roles = "SuperAdmin")]



        public async Task<IActionResult> AdminStatus(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            AppUser logged = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!ModelState.IsValid) return RedirectToAction("UserList", "Account");
            if (user == null)
            {
                return NotFound();
            }
            if (await _userManager.IsInRoleAsync(user, "SuperAdmin") == true)
            {
                return Json(new {status=500 });
            }
            if (user.Id == logged.Id)
            {
                return Json(new { status = 60 });
            }
            
            else
            {

                if (!user.IsAdmin == true)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _userManager.RemoveFromRoleAsync(user, "Member");
                    user.IsAdmin = true;
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, "Admin");
                    user.IsAdmin = false;
                    await _userManager.AddToRoleAsync(user, "Member");
                }
                await _context.SaveChangesAsync();
                return Json(new { status = 200 });
            }




        }


        public async Task<IActionResult> Logout()
        {
            await _signInResult.SignOutAsync();
            return RedirectToAction(nameof(Login));
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
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            if (!user.IsAdmin)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInResult.PasswordSignInAsync(user, login.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            return RedirectToAction("Index", "Dashboard");

        }


        public IActionResult Testuser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = 400 });
            }

            return Json(new { status = 200 });
        }

        public async Task<IActionResult> Edit()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            AdminEditVM adminEdit = new AdminEditVM
            {
                Username = user.UserName,
               
                Email = user.Email,
                Firstname = user.Firstname,
                Surname = user.Surname,
            };
            return View(adminEdit);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminEditVM editedUser)
        {
            if (!ModelState.IsValid) return View(editedUser);

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            AdminEditVM eUser = new AdminEditVM
            {
                Username = user.UserName,

                Email = user.Email,
                Firstname = user.Firstname,
                Surname = user.Surname,

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
            await _signInResult.SignInAsync(user, true);

            return RedirectToAction("Index", "Dashboard");

        }

        public async Task<IActionResult> BlockStatus(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(user, "SuperAdmin") || await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return Json(new { status = 500 });
            }

            if (await _userManager.IsInRoleAsync(user,"Blocked"))
            {
                await _userManager.RemoveFromRoleAsync(user,"Blocked");
                await _userManager.AddToRoleAsync(user, "Member");
                user.IsBlocked = false;

            }
            else
            {

                await _userManager.RemoveFromRoleAsync(user, "Member");

                await _userManager.AddToRoleAsync(user, "Blocked");
                user.IsBlocked = true;
              
            }

            await _context.SaveChangesAsync();
            return Json(new {status=200 });

        }
        #region Create role
        //public async Task CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Blocked"));
        //    //await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    //await _roleManager.CreateAsync(new IdentityRole("Member"));
        //}

        //public async Task CreateAdmin()
        //{
        //    AppUser user = new AppUser
        //    {
        //        UserName = "Superadmin",
        //        Email = "random@gmail.com",
        //        Firstname = "Nurxan",
        //        Surname = "Masimzade",
        //        EmailConfirmed = true
        //    };

        //    await _userManager.CreateAsync(user, "nurxan123");
        //    await _userManager.AddToRoleAsync(user, "SuperAdmin");

        //}
        #endregion
    }
}
