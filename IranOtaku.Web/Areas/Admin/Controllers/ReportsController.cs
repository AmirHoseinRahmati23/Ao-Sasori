using IranOtaku.Core.Utilities;
using IranOtaku.Data.Context;
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
    public class ReportsController : Controller
    {

        private readonly IranOtakuContext db;
        private readonly string name = NameGenerator.FixControllerNameToRedirect(nameof(ReportsController));
        public ReportsController(IranOtakuContext dataBase)
        {
            db = dataBase;
        }
        //public async Task<IActionResult> Index()
        //{
        //    var reports = await db.Reports
        //        .Where(r => r.IsFinally).Include(r => r.Book).ToListAsync();
            
        //    return View(reports);
        //}
    }
}
