using DocumentDbDAL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
            return cobject.GetType().GetProperties().Select(a => a.Name).ToList();

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
                s.Append("[" + i + "]");
                comma = ",";
            }
            return s.ToString();
        }

        /// <summary>
        /// extention method to determine is the string is null or empty
        /// </summary>
        /// <param name="target">string to test</param>
        /// <returns>boolean</returns>
        public static bool IsEmpty(this string target)
        {
            return String.IsNullOrEmpty(target);
        }
        /// <summary>
        /// This searches the source for any replacement values (${xxx}) and extracts the replacement value name.
        /// </summary>
        /// <param name="source">string to search</param>
        /// <param name="pattern">regex pattern - only necessary if it is different than the default</param>
        /// <returns>List&lt;string&gt; of results.</string></returns>
        public static IList<string> GetReplacementParameters(this string source, string pattern = @"\${(.*?)\}")
        {

            List<string> ret = new List<string>();

            foreach (Match m in Regex.Matches(source, pattern))
            {

                ret.Add(m.Value.Replace("${", "").Replace("}", ""));
            }
            return ret;
        }

        /// <summary>
        /// resolves all word replacements (${xxx}) in the string with corresponding values in the parameters list provided.
        /// used for monog selection defintions
        /// </summary>
        /// <param name="criteria">the string containing work replacement requirements</param>
        /// <param name="parameters">key value pair of replacement values</param>
        /// <returns>resolved version of the criteria string</returns>
        public static string ResolveCriteria(this string criteria, Dictionary<string, string> parameters)
        {
            IList<string> parms = criteria.GetReplacementParameters();

            if (parms.Count() > 0)
            {
                if (parameters == null)
                    return "{}";
                if (parameters.Count() != parms.Count())
                    throw new Exception("the parameter requirements of the querydef getQuery do not match the number of parameters provided");
                for (int a = 0; a < parms.Count(); a++)
                {
                    var source = "${" + parms[a] + "}";
                    if (!parameters.ContainsKey(parms[a]))
                        throw new Exception("the required criteria key (" + parms[a] + ") was not found in the parameters provided");
                    criteria = criteria.Replace(source, parameters[parms[a]]);
                }
            }
            return criteria;
        }

        /// <summary>
        /// resolves all word replacements (${xxx}) in the string with corresponding values in the parameters list provided.
        /// </summary>
        /// <param name="criteria">the string containing work replacement requirements</param>
        /// <param name="parameters">key value pair of replacement values</param>
        /// <returns>resolved version of the criteria string</returns>
        public static string ResolveCriteria(this string criteria, JObject parameters)
        {
            IList<string> parms = criteria.GetReplacementParameters();

            if (parms.Count() > 0)
            {
                if (parameters == null)
                    return "{}";
                if (parameters.Properties().Count() != parms.Count())
                    throw new Exception("the parameter requirements of the querydef getQuery do not match the number of parameters provided");
                for (int a = 0; a < parms.Count(); a++)
                {
                    var source = "${" + parms[a] + "}";
                    if (!parameters.ContainsKey(parms[a]))
                        throw new Exception("the required criteria key (" + parms[a] + ") was not found in the parameters provided");
                    criteria = criteria.Replace(source, (string)parameters[parms[a]]);
                }

            }
            return criteria;
        }

       /// <summary>
       /// returns the currect ip address 
       /// </summary>
       /// <returns>ip address</returns>
        public static string GetIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }

        public static Dictionary<string, string> IpList = new();
        public static string ResolveIP(this string connectionString, string name)
        {
            if (IpList.Count == 0)
            {
                string f = File.ReadAllText(@"appsettings.json");
                JObject config = JObject.Parse(f);
                //ConnectionString = config.SelectToken("ConnectionStrings.LocalDev").Value<string>().Replace("IPADDRESS", Utils.GetIPAddress());
                var d = (JArray) config["DataSources"];
                IpList.Add("IPADDRESS", Utils.GetIPAddress());
                foreach (var e in d)
                    IpList.Add(e["name"].ToString(), e["address"].ToString());
            }
            if (IpList.ContainsKey(name))
                return connectionString.Replace("*" + name + "*", IpList[name].ToString());
            else
                return connectionString;
        }

        //public static string SelectFieldList(this IList<QdefFieldModel> values)
        //{
        //    StringBuilder s = new();
        //    string comma = "";

        //    for (int a = 0; a < values.Count(); a++)
        //    {
        //        if (!values[a].dbName.IsEmpty())
        //        {
        //            s.Append(comma);
        //            s.Append(" [" + values[a].dbName + "]");
        //            comma = ",";
        //        } 
        //    }
        //    return s.ToString();
        //}
    }

     
}
