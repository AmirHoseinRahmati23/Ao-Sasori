using IranOtaku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class PayViewModel
    {
        public BookChapter Chapter { get; set; }
        public string BookName { get; set; }
        public int BookId { get; set; }
    }
}
