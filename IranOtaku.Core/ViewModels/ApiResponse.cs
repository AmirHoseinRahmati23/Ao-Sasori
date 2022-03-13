using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ApiResponse<T> where T : class
    {
        public ApiResponse() : this(true , "اروری موجود نیست")
        {

        }

        public ApiResponse(bool isSuccessed , string errorMessage)
        {
            IsSuccessed = isSuccessed;
            ErrorMessage = errorMessage;
        }
        public T Response { get; set; }

        public bool IsSuccessed { get; set; }
        public string ErrorMessage { get; set; }
    }
}
