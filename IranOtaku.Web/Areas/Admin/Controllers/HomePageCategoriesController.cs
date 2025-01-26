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
    public class HomePageCategoriesController : Controller
    {
        private readonly IranOtakuContext db;
        public HomePageCategoriesController(IranOtakuContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await db.HomePageCategories.Select(h => h.Category)
                .AsNoTracking().ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> SelectMembers()
        {
            var categories = await db.Categories.Where(c => !c.IsDeleted)
                .AsNoTracking().ToListAsync();


            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> SelectMembers(List<int> categories)
        {

            var homeCategories = await db.HomePageCategories.Where(b => !b.IsDeleted)
                .AsNoTracking().ToListAsync();

            if (homeCategories.Any())
            {
                foreach (var homeCat in homeCategories)
                {
                    db.HomePageCategories.Remove(homeCat);
                }
            }


            if (categories.Any())
            {
                foreach (int catId in categories)
                {
                    var category = await db.Categories.Where(b => !b.IsDeleted)
                        .AsNoTracking().FirstOrDefaultAsync(b => b.Id == catId);

                    if (category != null)
                    {
                        await db.HomePageCategories.AddAsync(new HomePageCategory
                        {
                            CategoryId = catId
                        });

                    }
                }

                await db.SaveChangesAsync();
            }

            
            return RedirectToAction(nameof(Index) , "HomePageCategories");
        }
    }
}
