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
    public class SlidersController : Controller
    {

        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(SlidersController));
        public SlidersController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sliders = await db.Sliders.ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(HomePageSlider slider)
        {

            await db.AddAsync(slider);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);
        }

        [HttpGet]
        public async Task<IActionResult> Items(int id)
        {
            var slider = await db.Sliders
                .Include(s => s.Items).SingleOrDefaultAsync(s => s.Id == id);

            ViewBag.SliderId = id;

            return View(slider);
        }

        [HttpGet]
        public IActionResult AddItem(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int bookId, int sliderId)
        {
            var book = await db.Books.Include(b => b.Sliders)
                .SingleOrDefaultAsync(b => b.Id == bookId && !b.IsDeleted && b.IsConfirmed);

            var slider = await db.Sliders.FirstOrDefaultAsync(s => s.Id == sliderId);
            
            book.Sliders.Add(slider);


            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await db.Sliders.FindAsync(id);

            
            await Task.Run(() =>
            {
                db.Remove(slider);
            });

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int id , int sliderId)
        {
            var book = await db.Books.Include(b => b.Sliders)
                .SingleOrDefaultAsync(b => b.Id == id && !b.IsDeleted && b.IsConfirmed);

            await Task.Run(() =>
           {
               book.Sliders.Remove(book.Sliders.Single(s => s.Id == sliderId));
           });


            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);
        }
    }
}
