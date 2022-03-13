using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class Subtitle
    {
        [Key]
        public int SubtitleId { get; set; }

        public bool IsLink { get; set; }
        public string SubtitleLinkOrName { get; set; }

        [Display(Name = "شماره زیرنویس")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int SubtitleNumber { get; set; }

        [Display(Name = "قیمت(به تومان)")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int Price { get; set; }
        public int SeasonId { get; set; }

        //navigation properties
        public virtual ICollection<SubtitlePay> Payments { get; set; }

        [ForeignKey(nameof(SeasonId))]
        public virtual AnimeSeason Season { get; set; }
    }
}
