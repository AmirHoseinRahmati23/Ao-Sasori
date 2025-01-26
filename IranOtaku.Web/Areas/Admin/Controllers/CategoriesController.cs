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
    public class CategoriesController : Controller
    {
        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(CategoriesController));
        public CategoriesController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Index()
        {
            var categories = await db.Categories.Where(c => !c.IsDeleted).ToListAsync();

            ViewBag.Title = "مدیریت ژانر ها";
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Add(Category model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category = new Category()
                    {
                        Name = model.Name,
                        IsDeleted = false
                    };

                    await db.AddAsync(category);
                    await db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), name);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ViewBag.Error = "مشکلی پیش آمد";
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await db.Categories.FindAsync(id);
            
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Edit(Category model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var category = await db.Categories.FindAsync(model.Id);



                    #region Update Information

                    category.Name = model.Name;

                    #endregion

                    await db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ViewBag.Error = "مشکلی پیش آمد";
            }
            return RedirectToAction(nameof(Index), name);
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await db.Categories.FindAsync(id);
            category.IsDeleted = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }
    }
}
