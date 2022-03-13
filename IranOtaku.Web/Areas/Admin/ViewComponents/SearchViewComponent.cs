using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Type = IranOtaku.Data.Entities.Type;

namespace IranOtaku.Web.Areas.Admin.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly IranOtakuContext db;

        public SearchViewComponent(IranOtakuContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string controllerName , string actionName , string areaName)
        {
            var model = new SearchViewModel()
            {
                AreaName = areaName,
                ActionName = actionName,
                ControllerName = controllerName,
                Text = "",
                PageId = 1,
                TypeId = 1
            };
            if(controllerName == "Books")
            {
                model.Types = await GetTypesAsync();
            }

            return View(model);
        }
        private async Task<List<Type>> GetTypesAsync()
        {
            var types = await db.Types.ToListAsync();
            return types;
        }
    }
}
