using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IranOtaku.Data.Entities;
using IranOtaku.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IranOtaku.Data.Context;

namespace PoshakMonix.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]

    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IranOtakuContext db;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IranOtakuContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await userManager.GetUsersInRoleAsync("User");
            
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userManager.Users
                .Where(u => u.Id != "b74ddd14-6340-4840-95c2-db12554843e5").AsNoTracking().ToListAsync();

            return View(nameof(Index) , users);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Administrators()
        {
            var userIds = await db.UserRoles.Where(u => u.RoleId != "b74d13f0-5201-4840-95c2-a14da6895711"
            && u.RoleId != "fab4fac1-c546-41de-aebc-a14da6895711")
                .Select(u => u.UserId).ToListAsync();

            var users = await userManager.Users.Where(u => userIds.Any(i => i == u.Id))
                .AsNoTracking().ToListAsync();

            return View(nameof(Index) ,users);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if(await userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction(nameof(Index));
                }
                await userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "مشکلی در حذف کاربر به وجود آمد");
            }
            return View();

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(string id)
        {
            var roles = await roleManager.Roles.Where(r => r.NormalizedName != "GOD")
                .AsNoTracking().Select(s => new RoleListViewModel() {
                RoleName = s.Name,
                RoleId = s.Id
            }).ToListAsync();

            var user = await userManager.FindByIdAsync(id);
            if (!await userManager.IsInRoleAsync(user , "Admin") ||
                await userManager.IsInRoleAsync(await userManager.GetUserAsync(User), ""))
            {
                ViewBag.User = user;
                return View(roles);
            }

            return RedirectToAction(nameof(Index), "Users");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserToRole(string userId ,List<string> roles)
        {
            var user = await userManager.FindByIdAsync(userId);
            bool succeeded = true;

            foreach (var item in await roleManager.Roles.ToListAsync())
            {
                var result = await userManager.RemoveFromRoleAsync(user, item.Name);
            };
            
            foreach(var item in roles)
            {
                var result = await userManager.AddToRoleAsync(user, item);
                succeeded = result.Succeeded;
                if (!succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            if (succeeded)
            {
                return RedirectToAction(nameof(Index) , "Users");
            }



            return View();
        }


    }
}
