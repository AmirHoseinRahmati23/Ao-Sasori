using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.UnitOfWork.Repositories
{
    public interface ICategoryRepository
    {
        public Task<bool> IsBookInCategoryAsync(int bookId , int categoryId);
        public Task<bool> IsCategoryInHomePageAsync(int categoryId);
    }
}
