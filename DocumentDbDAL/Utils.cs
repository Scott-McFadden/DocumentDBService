using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{
    public static class Utils
    {
        public static List<string> GetPropertyNames(this object cobject)
        {
           return cobject.GetType().GetProperties().Select(a=> a.Name).ToList();

        }

        public static string ConvertListOfStringsToCommaSep(this List<string> items)
        {
            StringBuilder s = new StringBuilder();
            var comma = "";
            foreach (string i in items)
            {
                s.Append(comma);
                s.Append("[" +i + "]");
                comma = ",";

            }
            return s.ToString();

        }

        
    }

     
}
