using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IranOtaku.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin,Manager,Translator")]
        public IActionResult Index()
        {
            ViewBag.Title = "مدیریت|صفحه ی اصلی";
            return View();
        }
    }
}
