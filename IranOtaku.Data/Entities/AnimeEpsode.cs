using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class AnimeEpsode
    {
        [Key]
        public int EpsodeId { get; set; }

        public bool IsLink { get; set; }


        public string Quality { get; set; }

        public string EpsodeLinkOrName { get; set; }
        [Display(Name = "شماره این قسمت")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int EpsodeNumber { get; set; }

        [Display(Name = "قیمت(به تومان)")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int Price { get; set; }

        public int SeasonId { get; set; }

        //navigation properties
        public virtual ICollection<EpsodePay> Payments { get; set; }

        [ForeignKey(nameof(SeasonId))]
        public virtual AnimeSeason Season { get; set; }
    }
}
