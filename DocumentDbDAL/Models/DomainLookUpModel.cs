using Newtonsoft.Json;
using System;
using System.Data.SqlClient;

namespace DocumentDbDAL.Models
{
    public class DomainLookUpModel : IModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string Category { get; set; }
        public string Name { get; set; } 
        public string Value { get; set; }

        public string GetInsertValues()
        {
            return $" '{id}', '{Category}',  '{Name}', '{Value}' ";
        }

        public string GetSetValues()
        {
            return $" [Category]='{Category}', [Name]='{Name}', [Value]='{Value}' ";
        }

        public static DomainLookUpModel Populate(ref SqlDataReader rdr)
        {
            DomainLookUpModel ret = new DomainLookUpModel();
            ret.id = new Guid(rdr["id"].ToString());
            ret.Category = rdr["Category"].ToString();
            ret.Name = rdr["Name"].ToString();
            ret.Value = rdr["Value"].ToString();

            return ret;
        }

        /// <summary>
        /// creates an instance of this object from the string provided.
        /// </summary>
        /// <param name="data">json string representing this object</param>
        /// <returns>new ConnectionModel object</returns>
        public static DomainLookUpModel deserialize(string data)
        {
            return JsonConvert.DeserializeObject<DomainLookUpModel>(data);
        }
        /// <summary>
        /// create a json string for the object.
        /// </summary>
        /// <param name="model">ConnectionModel</param>
        /// <returns>json string</returns>
        public string serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }


}
