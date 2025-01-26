using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels;

namespace IranOtaku.Web.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class LoaderController : ControllerBase
    {
        public LoaderController(IranOtakuContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        private readonly IranOtakuContext _db;
        private readonly UserManager<User> _userManager;


        [HttpGet(template: "{id}/{userName}")]
        public async Task<ActionResult<MangaLoaderViewModel>> GetChapters(int id, string userName)
        {
            var user = await _userManager
                .Users.Include(u => u.Chapters)
                .SingleOrDefaultAsync(u => u.UserName == userName);
            var chapter = await _db.BookChapters
                .Include(c => c.Season).ThenInclude(c => c.Book)
                .ThenInclude(s => s.Seasons).ThenInclude(c => c.Chapters).ThenInclude(c => c.Users)
                .Include(c => c.Images)
                .SingleOrDefaultAsync(c => c.Id == id);

            var allChapters = chapter.Season.Book.Seasons.SelectMany(s => s.Chapters).Where(c => !c.IsDeleted).OrderBy(c => c.ChapterNumber).ToList();

            var userChapters =
                (await _userManager.IsInRoleAsync(user, "User")) ?
                allChapters.Where(c => c.Users.Contains(user) || c.Price == 0)
                :
                allChapters;


            var model = new LoaderViewModel()
            {
                UserName = userName,
                DefaultChapterNumber = chapter.ChapterNumber,
                Images = chapter.Images.Select(i => i.ImageName).ToList(),
                AllChapters = allChapters.Select(s => new LoaderViewModel.ChapterViewModel
                {
                    ChapterId = s.Id,
                    ChapterNumber = s.ChapterNumber,
                    Price = s.Price
                }).ToList()
                ,
                UserChapters = userChapters.Select(c => new LoaderViewModel.UserChapterViewModel
                {
                    ChapterId = c.Id,
                    ChapterNumber = c.ChapterNumber
                }).ToList()
            };

            if (chapter.Price != 0)
            {

                if (user.Chapters.Any(c => c.Id == chapter.Id) || !(await _userManager.IsInRoleAsync(user, "User")))
                {
                    return Ok(model);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Ok(model);
            }
        }

        [HttpGet(template: "GetChapter/{id}")]
        public async Task<ActionResult<ApiResponse<ChapterInfo>>> GetChapter(int id)
        {

            var model = new ApiResponse<ChapterInfo>();


            try
            {
                var chapter = await _db.BookChapters.Include(c => c.Season).ThenInclude(s => s.Book)
                .Select(c => new ChapterInfo()
                {
                    ChapterId = c.Id,
                    BookName = c.Season.Book.Name,
                    Price = c.Price,
                    ChapterNumber = c.ChapterNumber,
                    UserName = _userManager.GetUserName(User)
                })
                .FirstOrDefaultAsync(c => c.ChapterId == id);

                if (chapter == null)
                {
                    model.IsSuccessed = false;
                    model.ErrorMessage = "چپتری یافت نشد";
                    return NotFound(model);
                }

                model.Response = chapter;
            }
            catch(Exception e)
            {
                model.IsSuccessed = false;
                model.ErrorMessage = e.Message;
            }


            return Ok(model);

        }


        [HttpGet(template: "Pay/{id}/{userName}")]
        public async Task<ActionResult<ApiResponse<ChapterInfo>>> PayForChapter(int id, string userName)
        {
            var model = new ApiResponse<ChapterInfo>(true, "اروری موجود نیست");

            try
            {
                var user = await _userManager.Users
                    .Include(u => u.Chapters).SingleOrDefaultAsync(u => u.UserName == userName);
                
                var chapter = await _db.BookChapters
                    .Include(c => c.Season).ThenInclude(s => s.Book).ThenInclude(b => b.Reports)
                    .FirstOrDefaultAsync(c => c.Id == id);

                #region Validation
                if (user == null)
                {
                    model.IsSuccessed = false;
                    model.ErrorMessage = "کاربری با این نام کاربری یافت نشد";
                    return NotFound(model);
                }
                if (chapter == null)
                {
                    model.IsSuccessed = false;
                    model.ErrorMessage = "چپتری با این آیدی یافت نشد";
                    return NotFound(model);
                }

                if (user.Chapters.Contains(chapter))
                {
                    model.IsSuccessed = false;
                    model.ErrorMessage = "این چپتر از قبل خریداری شده";
                    return BadRequest(model);
                }
                if ((user.Wallet / 10) < chapter.Price)
                {
                    model.IsSuccessed = false;
                    model.ErrorMessage = "موجودی کافی نیست";
                    return BadRequest(model);
                }

                #endregion

                user.Wallet -= chapter.Price * 10;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var report = chapter.Season.Book.Reports.FirstOrDefault(r => !r.IsFinally);
                    if (report == null)
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

                    model.Response = new ChapterInfo()
                    {
                        ChapterId = chapter.Id,
                        BookName = chapter.Season.Book.Name,
                        ChapterNumber = chapter.ChapterNumber,
                        Price = chapter.Price
                    };
                }
                else
                {
                    model.IsSuccessed = false;
                    model.ErrorMessage = result.Errors.First().Description;
                }


            }
            catch(Exception e)
            {
                model.IsSuccessed = false;
                model.ErrorMessage = e.Message;
            }




            return Ok(model);
        }

    }
}
