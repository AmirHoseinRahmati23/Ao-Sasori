using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class ChapterImage
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public int ImageNumber { get; set; }

        public int ChapterId { get; set; }

        //navigation properties
        [ForeignKey(nameof(ChapterId))]
        public virtual BookChapter Chapter { get; set; }
    }
}
