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
        /// <summary>
        /// gets the property names from an object - uses reflection
        /// </summary>
        /// <param name="cobject">object to look at</param>
        /// <returns>list of property names.</returns>
        public static List<string> GetPropertyNames(this object cobject)
        {
           return cobject.GetType().GetProperties().Select(a=> a.Name).ToList();

        }

        /// <summary>
        /// takes a list of strings and puts them into what could be recognized as an SQL Insert Field List for SQL Server.
        /// </summary>
        /// <param name="items">list of strings</param>
        /// <returns>comma separated list of strings</returns>
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
