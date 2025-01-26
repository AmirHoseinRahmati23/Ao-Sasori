using IranOtaku.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = IranOtaku.Data.Entities.Type;

namespace IranOtaku.Data.Context
{
    public class IranOtakuContext : IdentityDbContext
    {
        public IranOtakuContext(DbContextOptions options):base(options)
        {

        }


        public DbSet<Book> Books { get; set; }
        public DbSet<BookChapter> BookChapters { get; set; }
        public DbSet<ChapterImage> ChapterImages { get; set; }
        public DbSet<BookSeason> BookSeasons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookComment> BookComments { get; set; }
        public DbSet<HomePageCategory> HomePageCategories { get; set; }
        public DbSet<LikeOrDislike> Likes { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<BookReport> BookReports { get; set; }
        public DbSet<HomePageSlider> Sliders { get; set; }
        public DbSet<HomeItem> HomeItems { get; set; }

        public DbSet<Anime> Animes { get; set; }
        public DbSet<AnimeSeason> AnimeSeasons { get; set; }
        public DbSet<AnimeEpsode> AnimeEpsodes { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }

        public DbSet<ChapterPay> ChapterPayments { get; set; }
        public DbSet<SubtitlePay> SubtitlePayments { get; set; }
        public DbSet<EpsodePay> EpsodePayments { get; set; }
        public DbSet<AnimeReport> AnimeReports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SeedTypes(builder);

            SeedUsers(builder);

            SeedRoles(builder);

            SeedUserRoles(builder);
        }





        private void SeedTypes(ModelBuilder builder)
        {
            builder.Entity<Type>()
                .HasData(
                new Type() { TypeName = "مانگا", Id = 1, IsDeleted = false },
                new Type() { TypeName = "مانهوا", Id = 2, IsDeleted = false }
                );
        }


        #region IdentitySeeds
        private void SeedUsers(ModelBuilder builder)
        {
            var firstAdmin = new User()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5 ",
                UserName = "",
                Name = "امیرحسین",
                Family = "رحمتی",
                NormalizedUserName = "AMIRHOSEIN_RAHMATI_233_GODMODE",
                Email = "www.amir233@gmail.com",
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                ConcurrencyStamp = "4f5f066d-b118-4d17-9420-37edf9cccb59",
                SecurityStamp = "3dd750e2-a940-43b3-aefc-c677f6a68ca9",
                PasswordHash = "AQAAAAEAACcQAAAAEM3HumZQwETRIbUvyBu0iN1Cg2BqrHKdfsYJvZAPOyShb1FUIzp0UpovEPrjtwvqpA=="
            };
            var secoundAdmin = new User()
            {
                Id = "fac1b74d-4840-95c2-4840-a14da6895711",
                UserName = "Arman",
                NormalizedUserName = "ARMAN",
                Name = "آرمان",
                Family = "دزفولی",
                Email = "arman0dh@gamil.com",
                LockoutEnabled = false,
                PhoneNumber = "1234567890",
                ConcurrencyStamp = "8f4edc0f-18ec-4601-a803-8d35da8dd4c8",
                SecurityStamp = "98fcd59f-be7a-42ff-a996-fc935b1569bd",
                PasswordHash = "AQAAAAEAACcQAAAAELSc6346gKyDJLjQ5Zp9TvvIWzS9q8sVCncaiiWRxrNUVnsDEdKP6nXMX96nPwvKLg=="
            };

            builder.Entity<User>().HasData(firstAdmin, secoundAdmin);
        }

        private void SeedRoles(ModelBuilder builder)
        {

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "c7b013f0-5201-4317-abd8-c211f91b7330",
                    Name = "Admin",
                    ConcurrencyStamp = "2",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "fc25q7n6-2075-4317-abwp-c217n61b7rt4",
                    Name = "Manager",
                    ConcurrencyStamp = "3",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole()
                {
                    Id = "b74d13f0-4a65-abd8-abwp-a14da6wp89ab",
                    Name = "Translator",
                    ConcurrencyStamp = "4",
                    NormalizedName = "TRANSLATOR"
                },
                new IdentityRole()
                {
                    Id = "b74d13f0-5201-4840-95c2-a14da6895711",
                    Name = "User",
                    ConcurrencyStamp = "5",
                    NormalizedName = "USER"
                });
        }
        private void SeedUserRoles(ModelBuilder builder)
        {

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = "fab4fac1-c546-41de-aebc-a14da6895711",
                    UserId = "b74ddd14-6340-4840-95c2-db12554843e5"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "c7b013f0-5201-4317-abd8-c211f91b7330",
                    UserId = "fac1b74d-4840-95c2-4840-a14da6895711"
                });
        }
        #endregion



    }
}
