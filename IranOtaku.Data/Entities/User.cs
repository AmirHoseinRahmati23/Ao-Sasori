using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Image { get; set; }

        public int Wallet { get; set; }

        public bool IsVip { get; set; }
        public DateTime EndVipTime { get; set; }
        public DateTime StartVipTime { get; set; }



        //Navigation Properties

        public virtual ICollection<Book> Book { get; set; }
        public virtual ICollection<BookChapter> Chapters { get; set; }
        public virtual ICollection<Subtitle> Subtitles { get; set; }
        public virtual ICollection<AnimeEpsode> Epsodes { get; set; }


        public virtual ICollection<LikeOrDislike> LikeOrDislikes { get; set; }
    }
}
