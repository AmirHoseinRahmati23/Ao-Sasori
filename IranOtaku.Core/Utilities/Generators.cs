using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IranOtaku.Core.Utilities
{
    public static class NameGenerator
    {
        public static string CreateName()
        {
            return Guid.NewGuid().ToString().Trim().Replace("-" , "");
        }
        public static string FixControllerNameToRedirect(string controllerName)
        {
            return controllerName.Trim().ToLower().Replace("controller","");
        }
    }
}
