using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class ChapterPay
    {
        [Key]
        public int PayId { get; set; }

        public DateTime PayTime { get; set; }

        public int Price { get; set; }



        public string UserId { get; set; }
        public int ChapterId { get; set; }
        // Navigation Props
        [ForeignKey(nameof(ChapterId))]
        public virtual BookChapter Chapter { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
