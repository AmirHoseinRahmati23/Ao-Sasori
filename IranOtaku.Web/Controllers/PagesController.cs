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
    public class PagesController : ControllerBase
    {
        public PagesController(IranOtakuContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        private readonly IranOtakuContext _db;
        private readonly UserManager<User> _userManager;


        [HttpGet(template: "{id}/{userName}")]
        public async Task<ActionResult<MangaLoaderViewModel>> GetImages(int id , string userName)
        {
            var chapter = await _db.BookChapters.FindAsync(id);

            var images = new MangaLoaderViewModel()
            {
                Images = await _db.ChapterImages.Where(i => i.ChapterId == id)
                    .OrderBy(i => i.ImageNumber).Select(i => i.ImageName).ToListAsync()
            };
            
            if (chapter.Price != 0 )
            {
                var user = await _userManager
                    .Users.Include(u => u.Chapters)
                    .SingleOrDefaultAsync(u => u.UserName == userName);

                if (user.Chapters.Any(c => c.Id == chapter.Id) || !(await _userManager.IsInRoleAsync(user, "User")))
                {
                    return Ok(images);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Ok(images);
            }
        }
    }

   

}
