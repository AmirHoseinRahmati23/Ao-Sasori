using IranOtaku.Core.UnitOfWork.Repositories;
using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Context;
using IranOtaku.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = IranOtaku.Data.Entities.Type;

namespace IranOtaku.Core.UnitOfWork.Services
{
    public class BookManagement : IBookManagement
    {
        private readonly IranOtakuContext db;
        public BookManagement(IranOtakuContext dataBase)
        {
            db = dataBase;
        }
        public async Task<List<BooksGroupViewModel>> GetNewBooksAsync()
        {
            var types = await db.Types.Where(t => !t.IsDeleted).Take(2)
                .Include(t => t.Books)
                .ThenInclude(b => b.Categories)
                .Select(t => new BooksGroupViewModel()
                {
                    Books = t.Books
                    .Where(b => !b.IsDeleted && b.IsConfirmed)
                    .OrderByDescending(b => b.UpdateDate).Take(10).ToList(),
                    
                    GroupName = t.TypeName
                })
                .AsNoTracking().ToListAsync();

            return types;
        }

        public async Task<List<Book>> GetHomePageBooksAsync()
        {
            var categories = await db.Categories.Include(b => b.Books).ToListAsync();

            return null;
        }
    }
}
