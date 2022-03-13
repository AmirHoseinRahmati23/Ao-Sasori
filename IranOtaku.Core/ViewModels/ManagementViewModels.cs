using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Type = IranOtaku.Data.Entities.Type;

namespace IranOtaku.Core.ViewModels
{
    public class AddViewModel
    {

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "نام")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        [Display(Name = "نام های دیگر")]
        public string OtherNames { get; set; }
        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "خلاصه داستان")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ShortStory { get; set; }
        [Display(Name = "عکس آپلود میشود؟")]
        public bool ImageIsUploaded { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime UpdateDate { get; set; }

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

        [Display(Name = "عکس")]
        public IFormFile ImageFile { get; set; }

        public int TypeId { get; set; }
        public List<Type> AvaibleTypes { get; set; }

    }
    public class EditViewModel
    {
        [Display(Name = "فایل عکس")]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "نام عکس")]
        public string Image { get; set; }
        [Display(Name = "نام های دیگر")]
        public string OtherNames { get; set; }
        public List<Type> AvaibleTypes { get; set; }
        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "نام")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "خلاصه داستان")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ShortStory { get; set; }

        [Display(Name = "رده سنی")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Age { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [MaxLength(50, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string RealeseDate { get; set; }

        [Display(Name = "حجم هر قسمت")]
        [MaxLength(250, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ChapterSize { get; set; }

        [Display(Name = "عکس آپلود میشود؟")]
        public bool ImageIsUploaded { get; set; }
        [Display(Name = "کشور سازنده")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Country { get; set; }
        [Display(Name = "تیم ترجمه")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string TranslatorTeam { get; set; }
        [Display(Name = "مترجم")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Translator { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Status { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Price { get; set; }
        public int TypeId { get; set; }
        public int Id { get; set; }

    }

    public class AddEpsodeViewModel
    {
        public int Number { get; set; }
        public string Link { get; set; }
        public bool IsLink { get; set; }
        public int Price { get; set; }



        public IFormFile File { get; set; }
        public string Quality { get; set; }



        public int SeasonId { get; set; }
    }
    public class SearchViewModel
    {
        [Display(Name = "متن")]
        public string Text { get; set; }
        [Display(Name = "صفحه")]
        public int PageId { get; set; }
        [Display(Name = "نوع")]
        public int TypeId { get; set; }

        public List<Type> Types { get; set; }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
    }
}
