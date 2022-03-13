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

namespace IranOtaku.Core.UnitOfWork.Services
{
    public class AnimeManagement : IAnimeManagement
    {

        private readonly IranOtakuContext db;
        public AnimeManagement(IranOtakuContext dataBase)
        {
            db = dataBase;
        }
        public async Task<List<Anime>> GetNewAnimesAsync()
        {
            var animes = await db.Animes.Where(a => !a.IsDeleted && a.IsConfirmed)
                .OrderByDescending(a => a.UpdateDate).Take(10)
                .Include(a => a.Categories)
                .AsNoTracking().ToListAsync();

            return animes;
        }
    }
}
