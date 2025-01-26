using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class HomePageCategory
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }


        // Navigation Property
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

    }
}
