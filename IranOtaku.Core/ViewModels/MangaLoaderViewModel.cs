using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class MangaLoaderViewModel
    {
        public List<string> Images { get; set; }
    }
    public class LoaderViewModel
    {

        public string UserName { get; set; }
        public int DefaultChapterNumber { get; set; }
        public List<string> Images { get; set; }
        public List<UserChapterViewModel> UserChapters { get; set; }
        public List<ChapterViewModel> AllChapters { get; set; }



        public class UserChapterViewModel
        {
            public int ChapterId { get; set; }
            public int ChapterNumber { get; set; }
        }
        public class ChapterViewModel
        {
            public int ChapterId { get; set; }
            public int ChapterNumber { get; set; }
            public int Price { get; set; }
        }
    }
    
    public class ChapterInfo
    {
        public int ChapterId { get; set; }
        public int Price { get; set; }
        public int ChapterNumber { get; set; }
        public string BookName { get; set; }
        public string UserName { get; set; }
    }

}
