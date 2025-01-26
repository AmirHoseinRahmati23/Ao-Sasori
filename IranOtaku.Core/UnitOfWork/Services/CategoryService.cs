using IranOtaku.Core.UnitOfWork.Repositories;
using IranOtaku.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.UnitOfWork.Services
{
    public class CategoryService : ICategoryRepository
    {
        private readonly IranOtakuContext db;
        public CategoryService(IranOtakuContext dataBase)
        {
            db = dataBase;
        }
        public async Task<bool> IsBookInCategoryAsync(int bookId , int categoryId)
        {
            var book = await db.Books.Where(b => !b.IsDeleted && b.Id == bookId).FirstAsync();

            bool result = book.Categories.Any(c => c.Id == categoryId && !c.IsDeleted);

            return result;
        }

        public async Task<bool> IsCategoryInHomePageAsync(int categoryId)
        {
            bool result = await db.HomePageCategories.Include(h => h.Category).AnyAsync(h => h.Category.Id == categoryId);

            return result;
        }
    }
}
