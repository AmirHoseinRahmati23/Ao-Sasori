using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.ViewModels
{
    public class PagingGenericViewModel<T>
    {

        public PagingGenericViewModel(int pageId , int entityCount , int pageCount)
        {
            PageId = pageId;
            PageCount = pageCount;
            EntityCount = entityCount;
            Skip = (pageId - 1) * entityCount;

        }
        public List<T> Entities { get; set; }
        public int PageId { get; set; }
        public int PageCount { get; set; }
        public int EntityCount { get; set; }
        public int Skip { get; set; }
        public string Name { get; set; }

    }
}
