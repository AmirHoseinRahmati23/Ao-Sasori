using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class Type
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "نام نوع(مثال : مانگا)")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public string TypeName { get; set; }
        public bool IsDeleted { get; set; }

        //Navigation Properties
        public virtual ICollection<Book> Books { get; set; }
    }
}
