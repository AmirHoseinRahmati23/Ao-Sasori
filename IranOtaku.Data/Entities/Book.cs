using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "نام کتاب")]
        [MaxLength(150,ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }
        [Display(Name = "نام های دیگر")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string OtherNames { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "خلاصه داستان")]
        public string ShortStory { get; set; }
        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "عکس")]
        public string Image { get; set; }
        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "عکس آپلود میشود؟")]
        public bool ImageIsUploaded { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int Views { get; set; }

        [Display(Name = "رده سنی")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Age { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string RealeseDate { get; set; }

        [Display(Name = "حجم هر قسمت")]
        [MaxLength(250, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ChapterSize { get; set; }

        [Display(Name = "تیم ترجمه")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string TranslatorTeam { get; set; }
        [Display(Name = "مترجم")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Translator { get; set; }

        [Display(Name = "کشور سازنده")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Country { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Status { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Price { get; set; }

        public int TypeId { get; set; }



        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }


        // Relationsheeps <---> Navigation Properties
        public virtual ICollection<BookSeason> Seasons { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<BookComment> Comments { get; set; }
        public virtual ICollection<LikeOrDislike> LikeOrDislikes { get; set; }
        public virtual ICollection<BookReport> Reports { get; set; }
        public virtual ICollection<User> Translators { get; set; }
        public virtual ICollection<HomePageSlider> Sliders { get; set; }
        public virtual ICollection<HomeItem> HomeItems { get; set; }
        

        [ForeignKey(nameof(TypeId))]
        public virtual Type Type { get; set; }


    }
}
