using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = IranOtaku.Data.Entities.Type;

namespace IranOtaku.Core.UnitOfWork.Repositories
{
    public interface IBookManagement
    {
        public Task<List<BooksGroupViewModel>> GetNewBooksAsync();
        public Task<List<Book>> GetHomePageBooksAsync();
    }
}
