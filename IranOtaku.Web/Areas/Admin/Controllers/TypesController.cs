using IranOtaku.Core.ViewModels;
using IranOtaku.Core.Utilities;
using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Type = IranOtaku.Data.Entities.Type;
using Microsoft.AspNetCore.Authorization;

namespace IranOtaku.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Admin")]
    public class TypesController : Controller
    {
        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(TypesController));
        public TypesController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var types = await db.Types.ToListAsync();
            ViewBag.Title = "مدیریت انواع";
            return View(types);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(string typeName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var type = new Type()
                    {
                        TypeName = typeName.ToUpper(),
                    };

                    await db.AddAsync(type);
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await db.Types.FindAsync(id);
            type.IsDeleted = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }

    }
}
