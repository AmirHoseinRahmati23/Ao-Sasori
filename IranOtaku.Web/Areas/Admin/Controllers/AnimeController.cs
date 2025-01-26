using FluentFTP;
using IranOtaku.Core.Utilities;
using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IranOtaku.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AnimeController : Controller
    {
        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(AnimeController));
        public AnimeController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddToCategory(int id)
        {
            var anime = await db.Animes.Include(b => b.Categories).SingleOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            ViewBag.Anime = anime;

            var categories = await db.Categories.Where(c => !c.IsDeleted)
                .Include(c => c.Animes).AsNoTracking().ToListAsync();

            return View(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddToCategory(List<int> categoryId, int id)
        {
            var anime = await db.Animes.Include(b => b.Categories).SingleOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            foreach (var category in anime.Categories)
            {
                if (!categoryId.Any(c => c == category.Id))
                {
                    anime.Categories.Remove(category);
                }
            }

            var categories = await db.Categories.Where(c => !c.IsDeleted).AsNoTracking().ToListAsync();
            foreach (var category in categories)
            {
                if (categoryId.Any(c => c == category.Id) && !anime.Categories.Any(c => c.Id == category.Id))
                {
                    anime.Categories.Add(category);
                }
            }

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);

        }

        [Authorize(Roles = "Admin,Manager,Translator")]
        [HttpGet]
        public async Task<IActionResult> Index(SearchViewModel model)
        {
            List<Anime> animes;

            model.PageId = (model.PageId <= 0) ? 1 : model.PageId;
            model.TypeId = (model.TypeId <= 0) ? 1 : model.TypeId;

            int take = 9,
                skip = (model.PageId - 1) * 9;

            if (string.IsNullOrEmpty(model.Text))
            {
                animes = await db.Animes.Where(b => !b.IsDeleted && b.IsConfirmed)
                    .OrderByDescending(b => b.UpdateDate).Skip(skip).Take(take).ToListAsync();
            }
            else
            {
                animes = await db.
                    Animes.Where(b => !b.IsDeleted && b.IsConfirmed
                    && (b.Name.Contains(model.Text) || b.OtherNames.Contains(model.Text)))
                    .OrderByDescending(b => b.UpdateDate)
                    .Skip(skip).Take(take).ToListAsync();
            }

            ViewBag.Title = "مدیریت انیمه ها";
            return View(animes);
        }






        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> NotConfirmed(SearchViewModel model)
        {
            List<Anime> animes;

            model.PageId = (model.PageId <= 0) ? 1 : model.PageId;
            model.TypeId = (model.TypeId <= 0) ? 1 : model.TypeId;

            int take = 9,
                skip = (model.PageId - 1) * 9;

            if (string.IsNullOrEmpty(model.Text))
            {
                animes = await db.Animes.Where(b => !b.IsDeleted && !b.IsConfirmed)
                    .OrderByDescending(b => b.UpdateDate).Skip(skip).Take(take).ToListAsync();
            }
            else
            {
                animes = await db.Animes
                    .Where(b => !b.IsDeleted && !b.IsConfirmed
                    && (b.Name.Contains(model.Text) || b.OtherNames.Contains(model.Text)))
                    .OrderByDescending(b => b.UpdateDate)
                    .Skip(skip).Take(take).ToListAsync();
            }

            ViewBag.Title = "مدیریت انیمه ها";
            return View(animes);
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Add(AddViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    #region Add Image File

                    var fileName = NameGenerator.CreateName() + Path.GetExtension(model.ImageFile.FileName);
                    if (model.ImageFile == null)
                    {
                        fileName = "Default.png";
                        model.ImageIsUploaded = false;
                    }
                    else
                    {

                        var path = Directory.GetCurrentDirectory() + "/wwwroot" +
                        "/BookImages" + "/" + fileName;
                        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        using (var client = new FtpClient("", "", ""))
                        {
                            var token = new CancellationToken();
                            await client.ConnectAsync(token);

                            var result = await client
                                .UploadFileAsync(path, "/public_html/BookImages/" + fileName, FtpRemoteExists.NoCheck, true);

                            if (result == FtpStatus.Success)
                            {
                                model.ImageIsUploaded = true;
                            }
                            else
                            {
                                model.ImageIsUploaded = false;
                            }
                        }

                        System.IO.File.Delete(path);
                    }

                    #endregion
                    var anime = new Anime()
                    {
                        Name = model.Name,
                        ShortStory = model.ShortStory,
                        Translator = model.Translator,
                        TranslatorTeam = model.TranslatorTeam,
                        OtherNames = model.OtherNames,
                        ChapterSize = model.ChapterSize,
                        ImageIsUploaded = model.ImageIsUploaded,
                        RealeseDate = model.RealeseDate,
                        UpdateDate = DateTime.Now,
                        Age = model.Age,
                        Views = 0,
                        Image = fileName,
                        Country = model.Country,
                        Status = model.Status,
                        Price = model.Price,
                        IsDeleted = false,
                        IsConfirmed = false
                    };

                    await db.AddAsync(anime);
                    await db.SaveChangesAsync();


                    return RedirectToAction(nameof(NotConfirmed), name);
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

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Edit(int id)
        {
            var anime = await db.Animes.FindAsync(id);
            var model = new EditViewModel()
            {
                Name = anime.Name,
                Age = anime.Age,
                AvaibleTypes = await db.Types.ToListAsync(),
                ChapterSize = anime.ChapterSize,
                Image = anime.Image,
                RealeseDate = anime.RealeseDate,
                Translator = anime.Translator,
                TranslatorTeam = anime.TranslatorTeam,
                Id = anime.Id,
                ShortStory = anime.ShortStory,
                OtherNames = anime.OtherNames,
                Country = anime.Country,
                Price = anime.Price,
                Status = anime.Status
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var anime = await db.Animes.FindAsync(model.Id);

                    #region Delete And Add Image File


                    if (!string.IsNullOrEmpty(anime.Image) && model.ImageFile != null)
                    {
                        var token = new CancellationToken();
                        using (var client = new FtpClient("", "", ""))
                        {

                            await client.ConnectAsync(token);

                            await client.DeleteFileAsync("/public_html/BookImages/" + anime.Image);
                        }
                    }

                    if (model.ImageFile != null)
                    {
                        var fileName = NameGenerator.CreateName() + Path.GetExtension(model.ImageFile.FileName);
                        var path = Directory.GetCurrentDirectory() + "/wwwroot" +
                        "/BookImages" + "/" + fileName;
                        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        var token = new CancellationToken();
                        using (var client = new FtpClient("", "", ""))
                        {

                            await client.ConnectAsync(token);

                            var result = await client
                                .UploadFileAsync(path, "/public_html/BookImages/" + fileName, FtpRemoteExists.NoCheck, true);

                            if (result == FtpStatus.Success)
                            {
                                model.ImageIsUploaded = true;
                            }
                            else
                            {
                                model.ImageIsUploaded = false;
                            }
                            System.IO.File.Delete(path);
                        }
                        anime.Image = fileName;
                    }

                    #endregion

                    #region Update Information

                    anime.Name = model.Name;
                    anime.RealeseDate = model.RealeseDate;
                    anime.ShortStory = model.ShortStory;
                    anime.Age = model.Age;
                    anime.ChapterSize = model.ChapterSize;
                    anime.Translator = model.Translator;
                    anime.TranslatorTeam = model.TranslatorTeam;
                    anime.UpdateDate = DateTime.Now;
                    anime.OtherNames = model.OtherNames;
                    anime.Country = model.Country;
                    anime.Status = model.Status;
                    anime.Price = model.Price;

                    #endregion

                    await Task.Run(() =>
                    {
                        db.Update(anime);
                    });
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
        public async Task<IActionResult> Confirm(int id)
        {
            var anime = await db.Animes.FindAsync(id);
            anime.IsConfirmed = true;
            anime.UpdateDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var anime = await db.Animes.FindAsync(id);
            anime.IsDeleted = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Detail(int id)
        {
            var anime = await db.Animes
                .Include(b => b.Seasons.Where(s => !s.IsDeleted).OrderBy(s => s.SeasonNumber))
                .SingleOrDefaultAsync(b => !b.IsDeleted && b.Id == id);

            return View(anime);
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddSeason(int id)
        {
            ViewBag.AnimeId = id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddSeason(AnimeSeason season)
        {
            if (!ModelState.IsValid)
            {
                return View(season);
            }
            season.Id = default;

            await db.AddAsync(season);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), name, new { id = season.AnimeId });
        }

        [Route("{area}/{controller}/{action}/{id}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Subtitles(int id)
        {
            var season = await db.AnimeSeasons
                .Include(s => s.Subtitles.OrderBy(s => s.SubtitleNumber))
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            return View(season);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddSubtitle(int id)
        {
            ViewBag.SeasonId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddSubtitleWithFile(AddEpsodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            #region Add Image File

            var fileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "_" + NameGenerator.CreateName().Substring(0,6) + Path.GetExtension(model.File.FileName);
            if (model.File != null)
            {
                var path = Directory.GetCurrentDirectory() + "/wwwroot" +
                "/ChapterParts" + "/" + fileName;
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await model.File.CopyToAsync(stream);
                }

                using (var client = new FtpClient("", "", ""))
                {
                    var token = new CancellationToken();
                    await client.ConnectAsync(token);

                    var result = await client.UploadFileAsync(path, "/public_html/ChapterParts/" + fileName
                        , FtpRemoteExists.NoCheck, true);
                    System.IO.File.Delete(path);
                    if (result == FtpStatus.Failed)
                    {
                        return RedirectToAction(nameof(Subtitles), name, new { id = model.SeasonId });
                    }
                }

            }

            #endregion
            var subtitle = new Subtitle()
            {
                IsLink = false,
                Price = model.Price,
                SeasonId = model.SeasonId,
                SubtitleLinkOrName = fileName,
                SubtitleNumber = model.Number
            };

            await db.AddAsync(subtitle);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Subtitles), name, new { id = model.SeasonId });
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddSubtitleWithLink(AddEpsodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var subtitle = new Subtitle()
            {
                IsLink = true,
                Price = model.Price,
                SeasonId = model.SeasonId,
                SubtitleLinkOrName = model.Link,
                SubtitleNumber = model.Number
            };

            await db.AddAsync(subtitle);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Subtitles), name, new { id = model.SeasonId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteSubtitle(int id)
        {
            var subtitle = await db.Subtitles.FindAsync(id);

            if (!subtitle.IsLink)
            {
                using (var client = new FtpClient("171.22.24.61", "pz13195", "x7JdBaZH"))
                {
                    var token = new CancellationToken();
                    await client.ConnectAsync(token);

                    await client.DeleteFileAsync("/public_html/ChapterParts/" + subtitle.SubtitleLinkOrName);
                }
            }


            await Task.Run(() =>
            {
                db.Remove(subtitle);
            });

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Subtitles), name, new { id = subtitle.SeasonId });

        }


        [Route("{area}/{controller}/{action}/{id}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Hardsubs(int id)
        {
            var season = await db.AnimeSeasons
                .Include(s => s.Epsodes.OrderBy(c => c.EpsodeNumber))
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            return View(season);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddHardsub(int id)
        {
            ViewBag.SeasonId = id;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddHardsub(AddEpsodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var epsode = new AnimeEpsode()
            {
                IsLink = true,
                Price = model.Price,
                SeasonId = model.SeasonId,
                EpsodeLinkOrName = model.Link,
                EpsodeNumber = model.Number,
                Quality = model.Quality
            };

            await db.AddAsync(epsode);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Hardsubs), name, new { id = model.SeasonId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteHardsub(int id)
        {
            var epsode = await db.AnimeEpsodes.FindAsync(id);



            await Task.Run(() =>
            {
                db.Remove(epsode);
            });

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Hardsubs), name, new { id = epsode.Season });

        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            var season = await db.AnimeSeasons
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            season.IsDeleted = true;
            season.SeasonNumber = 0;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), name, new { id = season.AnimeId });
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Report(int id)
        {
            var model = await db.AnimeReports
                .SingleOrDefaultAsync(r => r.AnimeId == id && r.IsFinally == false);

            if (model == null)
            {
                var report = new AnimeReport()
                {
                    AnimeId = id,
                    IsFinally = false,
                    ReportName = "گزارش فعلی انیمه ",
                    MoneyCount = "0",
                    UsersCount = 0
                };

                await db.AddAsync(report);
                await db.SaveChangesAsync();

                model = report;
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SaveReport(int id)
        {
            ViewBag.ReportId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveReport(AnimeReport oldReport)
        {
            if (string.IsNullOrEmpty(oldReport.ReportName))
            {
                return View(oldReport);
            }

            var report = await db.AnimeReports.FindAsync(oldReport.Id);

            report.IsFinally = true;
            report.ReportName = oldReport.ReportName;

            await db.SaveChangesAsync();


            return RedirectToAction(nameof(OldReports), name , new { animeId = report.AnimeId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OldReports(int animeId)
        {
            var reports = await db.AnimeReports.Where(r => r.IsFinally && r.AnimeId == animeId).ToListAsync();

            return View(reports);
        }

    }
}
