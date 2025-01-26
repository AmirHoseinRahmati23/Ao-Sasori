using IranOtaku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class HomePageViewModel
    {
        public List<BooksGroupViewModel> Categories { get; set; }
        public List<Book> Items { get; set; }
        public List<HomePageSlider> Sliders { get; set; }

    }
}
