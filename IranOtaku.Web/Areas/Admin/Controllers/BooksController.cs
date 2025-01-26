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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FluentFTP;
using System.Net;
using System.Threading;
using FluentFTP.Helpers;
using System.IO.Compression;

namespace IranOtaku.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class BooksController : Controller
    {
        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(BooksController));
        public BooksController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }

        [Authorize(Roles = "Admin,Manager,Translator")]
        [HttpGet]
        public async Task<IActionResult> Index(SearchViewModel model)
        {
            List<Book> books;

            model.PageId = (model.PageId <= 0) ? 1 : model.PageId;
            model.TypeId = (model.TypeId <= 0) ? 1 : model.TypeId;

            int take = 9,
                skip = (model.PageId - 1) * 9;

            if (string.IsNullOrEmpty(model.Text))
            {
                books = await db.Books.Where(b => !b.IsDeleted && b.IsConfirmed)
                    .OrderByDescending(b => b.UpdateDate).Skip(skip).Take(take).ToListAsync();
            }
            else
            {
                books = await db.Types.Where(t => t.Id == model.TypeId && !t.IsDeleted)
                    .SelectMany(t => t.Books)
                    .Where(b => !b.IsDeleted && b.IsConfirmed
                    && (b.Name.Contains(model.Text) || b.OtherNames.Contains(model.Text)))
                    .OrderByDescending(b => b.UpdateDate)
                    .Skip(skip).Take(take).ToListAsync();
            }

            ViewBag.Title = "مدیریت کتاب ها";
            return View(books);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> NotConfirmed(SearchViewModel model)
        {
            List<Book> books;

            model.PageId = (model.PageId <= 0) ? 1 : model.PageId;
            model.TypeId = (model.TypeId <= 0) ? 1 : model.TypeId;

            int take = 9,
                skip = (model.PageId - 1) * 9;

            if (string.IsNullOrEmpty(model.Text))
            {
                books = await db.Books.Where(b => !b.IsDeleted && !b.IsConfirmed)
                    .OrderByDescending(b => b.UpdateDate).Skip(skip).Take(take).ToListAsync();
            }
            else
            {
                books = await db.Types.Where(t => t.Id == model.TypeId && !t.IsDeleted)
                    .SelectMany(t => t.Books)
                    .Where(b => !b.IsDeleted && !b.IsConfirmed
                    && (b.Name.Contains(model.Text) || b.OtherNames.Contains(model.Text)))
                    .OrderByDescending(b => b.UpdateDate)
                    .Skip(skip).Take(take).ToListAsync();
            }

            ViewBag.Title = "مدیریت کتاب ها";
            return View(books);
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Add()
        {
            var model = new AddViewModel()
            {
                AvaibleTypes = await db.Types.ToListAsync()
            };

            return View(model);
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
                    var book = new Book()
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
                        TypeId = model.TypeId,
                        Image = fileName,
                        Country = model.Country,
                        Status = model.Status,
                        Price = model.Price,
                        IsDeleted = false,
                        IsConfirmed = false
                    };

                    await db.AddAsync(book);
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
                model.AvaibleTypes = await db.Types.ToListAsync();
                ViewBag.Error = "مشکلی پیش آمد";
            }

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await db.Books.FindAsync(id);
            var model = new EditViewModel()
            {
                Name = book.Name,
                TypeId = book.TypeId,
                Age = book.Age,
                AvaibleTypes = await db.Types.ToListAsync(),
                ChapterSize = book.ChapterSize,
                Image = book.Image,
                RealeseDate = book.RealeseDate,
                Translator = book.Translator,
                TranslatorTeam = book.TranslatorTeam,
                Id = book.Id,
                ShortStory = book.ShortStory,
                OtherNames = book.OtherNames,
                Country = book.Country,
                Price = book.Price,
                Status = book.Status
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

                    var book = await db.Books.FindAsync(model.Id);

                    #region Delete And Add Image File


                    if (!string.IsNullOrEmpty(book.Image) && model.ImageFile != null)
                    {
                        var token = new CancellationToken();
                        using (var client = new FtpClient("", "", ""))
                        {

                            await client.ConnectAsync(token);

                            await client.DeleteFileAsync("/public_html/BookImages/" + book.Image);
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
                        book.Image = fileName;
                    }

                    #endregion

                    #region Update Information

                    book.Name = model.Name;
                    book.RealeseDate = model.RealeseDate;
                    book.ShortStory = model.ShortStory;
                    book.Age = model.Age;
                    book.ChapterSize = model.ChapterSize;
                    book.Translator = model.Translator;
                    book.TranslatorTeam = model.TranslatorTeam;
                    book.UpdateDate = DateTime.Now;
                    book.OtherNames = model.OtherNames;
                    book.TypeId = model.TypeId;
                    book.Country = model.Country;
                    book.Status = model.Status;
                    book.Price = model.Price;

                    #endregion

                    await Task.Run(() =>
                   {
                       db.Update(book);
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
                model.AvaibleTypes = await db.Types.ToListAsync();
                ViewBag.Error = "مشکلی پیش آمد";
            }
            return RedirectToAction(nameof(Index), name);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Confirm(int id)
        {
            var book = await db.Books.FindAsync(id);
            book.IsConfirmed = true;
            book.UpdateDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await db.Books.FindAsync(id);
            book.IsDeleted = true;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), name);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteChapter(int id)
        {
            var chapter = await db.BookChapters
                .Include(c => c.Season).Include(c => c.Images)
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            chapter.IsDeleted = true;
            chapter.ChapterNumber = 0;
            await db.SaveChangesAsync();
            

            foreach (var image in chapter.Images)
            {
                using (var client = new FtpClient("", "", ""))
                {
                    var token = new CancellationToken();
                    await client.ConnectAsync(token);

                    await client.DeleteFileAsync("/public_html/ChapterParts/" + image.ImageName);
                }

                await Task.Run(() =>
               {
                   db.Remove(image);
               });
            }
            await db.SaveChangesAsync();


            return RedirectToAction(nameof(Chapters), name, new { id = chapter.Season.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FreeChapter(int id)
        {
            var chapter = await db.BookChapters
                .Include(c => c.Season).Include(c => c.Images)
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            chapter.Price = 0;
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Chapters), name, new { id = chapter.Season.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            var season = await db.BookSeasons
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

            season.IsDeleted = true;
            season.SeasonNumber = 0;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), name, new { id = season.BookId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await db.ChapterImages.FindAsync(id);

            using (var client = new FtpClient("", "", ""))
            {
                var token = new CancellationToken();
                await client.ConnectAsync(token);

                await client.DeleteFileAsync("/public_html/ChapterParts/" + image.ImageName);
            }

            await Task.Run(() =>
            {
                db.Remove(image);
            });

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ChapterImages), name, new { id = image.ChapterId });

        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddToCategory(int id)
        {
            var book = await db.Books.Include(b => b.Categories).SingleOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            ViewBag.Book = book;

            var categories = await db.Categories.Where(c => !c.IsDeleted)
                .Include(c => c.Books).AsNoTracking().ToListAsync();

            return View(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddToCategory(List<int> categoryId, int id)
        {
            var book = await db.Books.Include(b => b.Categories).SingleOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            foreach (var category in book.Categories)
            {
                if (!categoryId.Any(c => c == category.Id))
                {
                    book.Categories.Remove(category);
                }
            }

            var categories = await db.Categories.Where(c => !c.IsDeleted).AsNoTracking().ToListAsync();
            foreach (var category in categories)
            {
                if (categoryId.Any(c => c == category.Id) && !book.Categories.Any(c => c.Id == category.Id))
                {
                    book.Categories.Add(category);
                }
            }

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), name);

        }


        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Detail(int id)
        {
            var book = await db.Books
                .Include(b => b.Seasons.Where(s => !s.IsDeleted).OrderBy(s => s.SeasonNumber))
                .SingleOrDefaultAsync(b => !b.IsDeleted && b.Id == id);

            return View(book);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddSeason(int id)
        {
            ViewBag.BookId = id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddSeason(BookSeason season)
        {
            if (!ModelState.IsValid)
            {
                return View(season);
            }
            season.Id = default;

            await db.BookSeasons.AddAsync(season);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Detail), name, new { id = season.BookId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddChapter(int id)
        {
            ViewBag.SeasonId = id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddChapter(BookChapter chapter)
        {
            if (!ModelState.IsValid)
            {
                return View(chapter);
            }
            chapter.Id = default;


            await db.AddAsync(chapter);
            await db.SaveChangesAsync();

            int chapterId = chapter.Id;
            chapter = await db.BookChapters
                .Include(c => c.Season).ThenInclude(c => c.Book).AsNoTracking()
                .SingleAsync(c => c.Id == chapterId);

            var book = await db.Books.FindAsync(chapter.Season.BookId);

            book.IsDeleted = false;
            book.UpdateDate = DateTime.Now;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Chapters), name, new { id = chapter.SeasonId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddImage(int id)
        {
            ViewBag.ChapterId = id;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddImage(ChapterImage image, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(image);
            }
            image.ImageId = default;


            #region Add Image File

            var fileName = NameGenerator.CreateName() + Path.GetExtension(imageFile.FileName);
            if (imageFile != null)
            {
                var path = Directory.GetCurrentDirectory() + "/wwwroot" +
                "/ChapterParts" + "/" + fileName;
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var result = await UploadFile("", "", "", path, fileName);
                if(result == FtpStatus.Failed)
                {
                    ModelState.AddModelError("", "عملیات با شکست مواجه شد");
                    return View(image);
                }
                

            }

            #endregion


            image.ImageName = fileName;
            await db.AddAsync(image);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(ChapterImages), name, new { id = image.ChapterId });
        }



        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult AddImagesByZip(int id)
        {
            ViewBag.ChapterId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> AddImagesByZip(IFormFile imageFile, int chapterId)
        {

            #region Add Image Files
            // allocating file name
            var fileName = NameGenerator.CreateName() + Path.GetExtension(imageFile.FileName);
            if (imageFile != null)
            {

                // uploading zip file to server
                var directory = Directory.GetCurrentDirectory() + "/wwwroot" +
                "/ChapterParts" + "/";
                var path = directory + fileName;
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // extracting zip file
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(path, directory);
                });

                // deleting extracted zip file(becouse it's content is already extracted)
                System.IO.File.Delete(path);



                // definind variables, one for folders inside the zip file, one temp variable for folders, and one for total images
                List<string> folders = new List<string>(),
                    tempFolders = new List<string>();
                var images = new List<string>();

                // adding the main directory that zip file is extracted in to folders list
                folders.Add(directory);

                // adding all images, inside all folders to the image list
                while (folders.Any())
                {
                    foreach (var folder in folders)
                    {
                        images.AddRange(Directory.GetFiles(folder));
                        tempFolders.AddRange(Directory.GetDirectories(folder));
                    }
                    folders.Clear();
                    folders.AddRange(tempFolders);
                    tempFolders.Clear();
                }
                
                
                var result = await UploadImages(images, chapterId);
                
                if (result == null)
                {
                    return RedirectToAction(nameof(ChapterImages), name, new { id = chapterId });
                }
                else if (result.Contains(FtpStatus.Failed))
                {
                    int failureRate = result.Where(r => r == FtpStatus.Failed).Count();
                    ModelState.AddModelError("", $"failed to upload a total of {failureRate} items.");
                    return View(model :imageFile);
                }




                await db.SaveChangesAsync();
            }



            return RedirectToAction(nameof(ChapterImages), name, new { id = chapterId });
            #endregion



            #region Upload Images

            async Task<List<FtpStatus>> UploadImages(IEnumerable<string> images, int chapterId)
            {
                var results = new List<FtpStatus>();
                if (images.Any())
                {
                    foreach (var imagePath in images)
                    {
                        string imageName = NameGenerator.CreateName() + Path.GetExtension(imagePath);
                        var image = new ChapterImage()
                        {
                            ChapterId = chapterId,
                            ImageName = imageName,
                        };

                        if (int.TryParse(Path.GetFileNameWithoutExtension(imagePath), out int imageNumber))
                            image.ImageNumber = imageNumber;
                        else
                            image.ImageNumber = 1;

                        var result = await UploadFile("", "", "", imagePath, imageName);
                        results.Add(result);


                        await db.AddAsync(image);
                    }

                    return results;
                }
                else
                {
                    return null;
                }


            }
            #endregion

        }


        private async Task<FtpStatus> UploadFile(string host, string user, string pass, string filePath, string fileName)
        {
            using (var client = new FtpClient(host, user, pass))
            {
                var token = new CancellationToken();
                await client.ConnectAsync(token);

                var result = await client.UploadFileAsync(filePath, "/public_html/ChapterParts/" + fileName
                    , FtpRemoteExists.NoCheck, true);
                System.IO.File.Delete(filePath);
                return result;
            }
        }

        [Route("{controller}/{action}/{id}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Chapters(int id)
        {
            var season = await db.BookSeasons
                .Include(s => s.Chapters.Where(c => !c.IsDeleted).OrderBy(c => c.ChapterNumber))
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            return View(season);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> ChapterImages(int id)
        {
            var chapter = await db.BookChapters
                .Include(s => s.Images).FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            return View(chapter);
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Translator")]
        public async Task<IActionResult> Report(int id)
        {
            var model = await db.BookReports
                .SingleOrDefaultAsync(r => r.BookId == id && r.IsFinally == false);

            if (model == null)
            {
                var report = new BookReport()
                {
                    BookId = id,
                    IsFinally = false,
                    ReportName = "گزارش فعلی کتاب ",
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
        public async Task<IActionResult> SaveReport(BookReport oldReport)
        {
            if (string.IsNullOrEmpty(oldReport.ReportName))
            {
                return View(oldReport);
            }

            var report = await db.BookReports.FindAsync(oldReport.Id);

            report.IsFinally = true;
            report.ReportName = oldReport.ReportName;

            await db.SaveChangesAsync();


            return RedirectToAction(nameof(Index), name, new { bookId = report.BookId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OldReports(int bookId)
        {
            var reports = await db.BookReports.Where(r => r.IsFinally && r.BookId == bookId).ToListAsync();

            return View(reports);
        }
    }
}
