using IranOtaku.Core.Utilities;
using IranOtaku.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranOtaku.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class CommentsController : Controller
    {
        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(CommentsController));
        public CommentsController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Index()
        {
            var categories = await db.BookComments.Where(c => !c.IsDeleted).ToListAsync();

            ViewBag.Title = "مدیریت کامنت ها";
            return View(categories);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await db.BookComments.FindAsync(id);
            comment.IsDeleted = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Confirm(int id)
        {
            var comment = await db.BookComments.FindAsync(id);
            comment.Confirmed = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }
    }
}
