using IranOtaku.Core.Utilities;
using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
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
    [Authorize(Roles = "Admin,Manager")]
    public class HomeItemsController : Controller
    {
        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(HomeItemsController));
        public HomeItemsController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await db.HomeItems.Include(h => h.Book).ToListAsync();
            return View(items);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(HomeItem item)
        {

            await db.AddAsync(item);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await db.HomeItems.FindAsync(id);

            await Task.Run(() =>
           {
               db.Remove(item);
           });

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);
        }
    }
}
