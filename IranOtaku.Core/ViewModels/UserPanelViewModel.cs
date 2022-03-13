using IranOtaku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class UserPanelViewModel
    {
        public User User { get; set; }



        public PagingGenericViewModel<BookChapter> Chapters { get; set; }

    }
}
