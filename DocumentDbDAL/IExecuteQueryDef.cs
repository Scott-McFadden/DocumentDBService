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
    public abstract class ExecuteQueryDefBase : IExecuteQueryDef
    {
        protected ConnectionModel connectionModel;
        protected QueryDefModel queryDefModel;
        protected string connectionString = "";


        protected string queryDefName = "";
        public string QueryDefName
        {
            get { return queryDefName; }
            set
            {
                queryDefName = value;
                queryDefModel = QueryDefService.Get(queryDefName);
                connectionModel = ConnectionService.Get(queryDefModel.connection);
            }
        }

        protected abstract string getConnectionString() ;

        public abstract JObject GetOne(string id);
        public abstract JArray Get(string criteria);

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
