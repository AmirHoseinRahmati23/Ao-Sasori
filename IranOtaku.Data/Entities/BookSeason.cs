using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class BookSeason
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "نام فصل")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public string SeasonName { get; set; }
        [Display(Name = "شماره فصل")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int SeasonNumber { get; set; }
        public int BookId { get; set; }

        public bool IsDeleted { get; set; }
        //Relationsheeps <---> Navigation Properties
        public virtual ICollection<BookChapter> Chapters { get; set; }
        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; }

    }
}
