using IranOtaku.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class BooksGroupViewModel
    {
        public string GroupName { get; set; }
        public List<Book> Books { get; set; }
    }
}
