using DocumentDbDAL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{

    /// <summary>
    /// base class for execute query defintion.   
    /// 
    /// Will be used to create new Execute Query Definition Base 
    /// </summary>
    public abstract class ExecuteQueryDefBase : IExecuteQueryDef
    {
        protected ConnectionModel connectionModel;
        protected QueryDefModel queryDefModel;
        protected string connectionString = "";


        protected string queryDefName = "";
        
        /// <summary>
        /// QueryDefName - sets the querydef for the class. Loads QueryDefModel and ConnectionModel
        /// </summary>
        public string QueryDefName
        {
            get { return queryDefName; }
            set
            {
                queryDefName = value;
                if (QueryDefService.DoesNotHave(queryDefName))
                    throw new Exception($"query ({queryDefName}) was not found in defintions");
                queryDefModel = QueryDefService.Get(queryDefName);
                connectionModel = ConnectionService.Get(queryDefModel.connection);
            }
        }

        protected abstract string getConnectionString() ;

        public abstract JObject GetOne(string id);
        public abstract JArray Get(string criteria);
        public abstract JArray GetLimit(string criteria = "", int page = 0, int pageSize = 0);
        public abstract int Delete(string id);
        public abstract int Update(JObject model);
        public abstract int Insert(JObject model);

    }

    public  interface IExecuteQueryDef
    {
        string QueryDefName { get; set; }

        JObject GetOne(string id);
        JArray Get(string criteria);

        int Delete(string id);
        int Update(JObject model);
        int Insert(JObject model);
    }
}
