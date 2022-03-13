using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class HomePageSlider
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "اسم اسلایدر")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public string SliderName { get; set; }


        public virtual ICollection<Book> Items { get; set; }
        public virtual ICollection<Anime> Animes { get; set; }

    }
}
