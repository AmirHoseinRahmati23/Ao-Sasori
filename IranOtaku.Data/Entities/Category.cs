using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} اجباری است")]
        [Display(Name = "نام ژانر")]
        [MaxLength(150, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        // Relationsheeps <---> Navigation Properties
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Anime> Animes { get; set; }
        

    }
}
