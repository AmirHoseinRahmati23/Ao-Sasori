using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranOtaku.Web.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IranOtakuContext _db;
        public UserPanelController(UserManager<User> userManager, SignInManager<User> signInManager
            , RoleManager<IdentityRole> roleManager, IranOtakuContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }
        public async Task<IActionResult> Index(int id)
        {
            var user = await _userManager.Users
                .Include(u => u.Chapters.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Season)
                .ThenInclude(s => s.Book).FirstOrDefaultAsync( u => u.UserName == User.Identity.Name);


            int chapterCount = user.Chapters.Count;
            int pageCount = (chapterCount % 15 == 0) ? chapterCount / 15 : chapterCount / 15 + 1;

            var model = new UserPanelViewModel()
            {
                User = user,
                Chapters = new PagingGenericViewModel<BookChapter>(id , chapterCount, pageCount)
            };
            model.Chapters.Entities = user.Chapters
                .Skip(model.Chapters.Skip).Take(model.Chapters.EntityCount).ToList();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var model = await _db.BookChapters.Where(c => c.Id == id).AsNoTracking().Select(c => 
            new PayViewModel() 
            {
                Chapter = c,
                BookId = c.Season.BookId,
                BookName = c.Season.Book.Name
            }).SingleOrDefaultAsync();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> FinalPay(int chapterId)
        {
            PayViewModel model;
            var chapter = await _db.BookChapters
                .Include(c => c.Season).ThenInclude(c => c.Book)
                .ThenInclude(b => b.Reports.Where(r => !r.IsFinally)).AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == chapterId);

            var user = await _userManager.Users.Include(u => u.Chapters)
                .SingleAsync(u => u.UserName == User.Identity.Name);


            if((user.Wallet/10) < chapter.Price)
            {
                ViewBag.Error = "موجودی کافی نیست";
                model = new PayViewModel()
                {
                    BookId = chapter.Season.BookId,
                    BookName = chapter.Season.Book.Name,
                    Chapter = chapter
                };
                return View(nameof(Pay), model);
            }

            user.Chapters.Add(chapter);

            user.Wallet -= chapter.Price * 10;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var report = chapter.Season.Book.Reports.FirstOrDefault(r => !r.IsFinally);
                if(report == null)
                {
                    report = new BookReport()
                    {
                        IsFinally = false,
                        BookId = chapter.Season.BookId,
                        MoneyCount = "0",
                        ReportName = "گزارش فعلی " + chapter.Season.Book.Name,
                        UsersCount = 0
                    };

                    await _db.AddAsync(report);
                }
                report.MoneyCount = (int.Parse(report.MoneyCount) + chapter.Price).ToString();
                
                var pay = new ChapterPay()
                {
                    ChapterId = chapter.Id,
                    PayTime = DateTime.Now,
                    Price = chapter.Price,
                    UserId = user.Id
                };
                await _db.AddAsync(pay);
                
                await _db.SaveChangesAsync();

                
                return RedirectToAction(nameof(Index), "UserPanel");
            }

            ViewBag.Error = result.Errors.First().Description;
            
            model = new PayViewModel()
            {
                BookId = chapter.Season.BookId,
                BookName = chapter.Season.Book.Name,
                Chapter = chapter
            };
            return View( nameof(Pay) , model);
        }


        [HttpGet]
        public async Task<IActionResult> PayForSubtitle(int id)
        {
            var model = 
                await _db.Subtitles
                .Include(s => s.Season).ThenInclude(s => s.Anime)
                .AsNoTracking().SingleOrDefaultAsync(s => s.SubtitleId == id);


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinalPayForSubtitle(int id)
        {
            var subtitle = await _db.Subtitles
                .Include(c => c.Season).ThenInclude(c => c.Anime)
                .ThenInclude(a => a.Reports.Where(r => !r.IsFinally)).AsNoTracking()
                .SingleOrDefaultAsync(c => c.SubtitleId == id);

            var user = await _userManager.Users.Include(u => u.Subtitles)
                .SingleAsync(u => u.UserName == User.Identity.Name);



            int finallWallet = user.Wallet - subtitle.Price * 10;
            if (finallWallet < 0)
            {
                ViewBag.Error = "موجودی کافی نیست";
                return View(nameof(PayForSubtitle), subtitle);
            }

            user.Subtitles.Add(subtitle);

            user.Wallet -= subtitle.Price * 10;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var report = subtitle.Season.Anime.Reports.First();
                report.MoneyCount += subtitle.Price;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "UserPanel");
            }

            return RedirectToAction("AnimeDetail" , "Home" , new { id = subtitle.Season.AnimeId });
        }


        [HttpGet]
        public async Task<IActionResult> PayForEpsode(int id)
        {
            var model =
                await _db.AnimeEpsodes
                .Include(s => s.Season).ThenInclude(s => s.Anime)
                .AsNoTracking().SingleOrDefaultAsync(s => s.EpsodeId == id);


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FinalPayForEpsode(int id)
        {
            var epsode = await _db.AnimeEpsodes
                .Include(c => c.Season).ThenInclude(c => c.Anime)
                .ThenInclude(a => a.Reports.Where(r => !r.IsFinally)).AsNoTracking()
                .SingleOrDefaultAsync(c => c.EpsodeId == id);

            var user = await _userManager.Users.Include(u => u.Epsodes)
                .SingleAsync(u => u.UserName == User.Identity.Name);


            int finallWallet = user.Wallet - epsode.Price * 10;
            if (finallWallet < 0)
            {
                ViewBag.Error = "موجودی کافی نیست";
                return View(nameof(PayForSubtitle), epsode);
            }

            user.Epsodes.Add(epsode);

            user.Wallet -= epsode.Price * 10;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var report = epsode.Season.Anime.Reports.First();
                report.MoneyCount += epsode.Price;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "UserPanel");
            }

            return View(nameof(PayForSubtitle), epsode);
        }

    }
}
