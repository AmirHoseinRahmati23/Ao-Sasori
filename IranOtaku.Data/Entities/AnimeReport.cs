using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class AnimeReport
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "اسم گزارش")]
        [Required(ErrorMessage = "{0} اجباری است")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string ReportName { get; set; }

        [Display(Name = "مقدار فروش")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public string MoneyCount { get; set; }

        [Display(Name = "تعداد کاربران خرید کرده")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int UsersCount { get; set; }

        public bool IsFinally { get; set; }
        public int AnimeId { get; set; }

        [ForeignKey(nameof(AnimeId))]
        public Book Anime { get; set; }
    }
}
