using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
using IranOtaku.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IranOtaku.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IranOtakuContext _db;

        public HomeController(ILogger<HomeController> logger, IranOtakuContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BooksGroupViewModel> categories = await _db.HomePageCategories.Where(c => !c.IsDeleted)
                .Include(c => c.Category)
                .ThenInclude(c => c.Books)
                .ThenInclude(b => b.Categories.Where(c => !c.IsDeleted))
                .Include(c => c.Category)
                .ThenInclude(c => c.Books.Where(b => !b.IsDeleted && b.IsConfirmed))
                .ThenInclude(b => b.Seasons.Where(s => !s.IsDeleted))
                .Select(h => new BooksGroupViewModel() 
                {
                    Books = h.Category.Books
                    .Where(b => !b.IsDeleted && b.IsConfirmed)
                    .OrderByDescending(b => b.UpdateDate).Take(10).ToList(),

                    GroupName = h.Category.Name
                }).ToListAsync();



            var homeItems = await _db.HomeItems.Select(h => h.Book)
                .Where(b => b.IsConfirmed && !b.IsDeleted).ToListAsync();

            var sliders = await _db.Sliders
                .Include(s => s.Items.Where(b => !b.IsDeleted && b.IsConfirmed))
                .ThenInclude(i => i.Categories.Where(c => !c.IsDeleted))
                .ToListAsync();

            var model = new HomePageViewModel
            {
                Categories = categories,
                Items = homeItems,
                Sliders = sliders
            };

            return View(model);
        }

        [HttpGet]
        [Route("{controller}/{action}/{id}/{pageId}")]
        public async Task<IActionResult> Category(int id , int pageId = 1)
        {
            var category = await _db.Categories
                .Include(c => c.Books.Where(b => b.IsConfirmed && !b.IsDeleted))
                .ThenInclude(b => b.Categories)
                .SingleOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            int bookCount = category.Books.Count;
            int pageCount = (bookCount % 9 == 0)? bookCount / 9 : bookCount / 9 + 1;
            var model = new PagingGenericViewModel<Book>(pageId, 9, pageCount);

            model.Name = category.Name;
            model.Entities = category.Books.OrderBy(b => b.UpdateDate)
                .Skip(model.Skip).Take(model.EntityCount).ToList();

            ViewBag.CategoryId = category.Id;


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(int id , string text)
        {
            int bookCount = await _db.Books
                .Where
                (b => !b.IsDeleted && b.IsConfirmed && (b.Name.Contains(text) || b.OtherNames.Contains(text)))
                .CountAsync();

            int pageCount = (bookCount % 9 == 0) ? bookCount / 9 : bookCount / 9 + 1;

            var model = new PagingGenericViewModel<Book>(id, 9, pageCount);
            model.Entities = await _db.Books
                .Where
                (b => !b.IsDeleted && b.IsConfirmed && (b.Name.Contains(text) || b.OtherNames.Contains(text)))
                .OrderBy(b => b.UpdateDate)
                .Skip(model.Skip).Take(model.EntityCount).Include(b => b.Categories)
                .ToListAsync();
            model.Name = text;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var book = await _db.Books
                .Include(b => b.Categories.Where(c => !c.IsDeleted))
                .Include(b => b.Comments.Where(c => !c.IsDeleted))
                .Include(b => b.Type)
                .Include(b => b.Seasons.Where(s => !s.IsDeleted).OrderBy(s => s.SeasonNumber))
                .ThenInclude(s => s.Chapters.Where(c => !c.IsDeleted).OrderBy(c => c.ChapterNumber))
                .SingleOrDefaultAsync(b => !b.IsDeleted && b.IsConfirmed && b.Id == id);

            book.Views++;

            await _db.SaveChangesAsync();

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> AnimeDetail(int id)
        {
            var anime = await _db.Animes

                .Include(b => b.Categories.Where(c => !c.IsDeleted))
                .Include(b => b.Comments.Where(c => !c.IsDeleted))

                .Include(b => b.Seasons.Where(s => !s.IsDeleted).OrderBy(s => s.SeasonNumber))
                .ThenInclude(s => s.Subtitles)
                .Include(b => b.Seasons.Where(s => !s.IsDeleted).OrderBy(s => s.SeasonNumber))
                .ThenInclude(s => s.Epsodes)

                .SingleOrDefaultAsync(b => !b.IsDeleted && b.IsConfirmed && b.Id == id);

            anime.Views++;

            await _db.SaveChangesAsync();

            return View(anime);
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int id , string commentText , string name , string email)
        {
            var comment = new BookComment()
            {
                Confirmed = false,
                BookId = id,
                Name = name,
                Email = email,
                IsDeleted = false,
                SendDate = DateTime.Now,
                Text = commentText
            };

            await _db.AddAsync(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Detail), "Home", new { id = id });
        }


        [HttpGet]
        public IActionResult Read(int id)
        {
            ViewBag.Title = "مانگا لودر";
            return View(id);
        }
    }
}
