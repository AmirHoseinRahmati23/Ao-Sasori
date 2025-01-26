using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class HomeItem
    {
        [Key]
        public int Id { get; set; }


        public int BookId { get; set; }
        //Navigation Property
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
    }
}
