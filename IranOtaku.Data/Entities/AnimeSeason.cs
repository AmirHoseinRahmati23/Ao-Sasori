using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Data.Entities
{
    public class AnimeSeason
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "نام فصل")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public string SeasonName { get; set; }
        [Display(Name = "شماره فصل")]
        [Required(ErrorMessage = "{0} اجباری است")]
        public int SeasonNumber { get; set; }
        public int AnimeId { get; set; }

        public bool IsDeleted { get; set; }
        //Relationsheeps <---> Navigation Properties
        public virtual ICollection<Subtitle> Subtitles { get; set; }
        public virtual ICollection<AnimeEpsode> Epsodes { get; set; }


        [ForeignKey(nameof(AnimeId))]
        public virtual Anime Anime { get; set; }
    }
}
