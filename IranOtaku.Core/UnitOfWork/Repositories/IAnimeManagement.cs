using IranOtaku.Core.ViewModels;
using IranOtaku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.UnitOfWork.Repositories
{
    public interface IAnimeManagement
    {
        public Task<List<Anime>> GetNewAnimesAsync();
    }
}
