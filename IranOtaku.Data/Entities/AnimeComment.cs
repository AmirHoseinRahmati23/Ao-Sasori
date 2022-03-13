using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class AnimeComment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "نام")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "ایمیل")]
        [EmailAddress]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "نظر")]
        [MaxLength(800, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Text { get; set; }

        public bool Confirmed { get; set; }
        public int AnimeId { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsDeleted { get; set; }

        // Relationsheeps <---> Navigation Properties
        [ForeignKey(nameof(AnimeId))]
        public virtual Anime Anime { get; set; }
    }
}
