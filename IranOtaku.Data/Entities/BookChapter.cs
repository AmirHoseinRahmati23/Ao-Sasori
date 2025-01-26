using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class BookChapter
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "شماره چپتر")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int ChapterNumber { get; set; }

        [Display(Name = "قیمت(به تومان)")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int Price { get; set; }
        public int SeasonId { get; set; }

        public bool IsDeleted { get; set; }

        // Relationsheeps <---> Navigation Properties
        public virtual ICollection<ChapterImage> Images { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ChapterPay> Payments { get; set; }
        [ForeignKey(nameof(SeasonId))]
        public virtual BookSeason Season { get; set; }
    }
}
