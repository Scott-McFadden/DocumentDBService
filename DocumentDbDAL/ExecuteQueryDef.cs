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
    public class ExecuteQueryDef : ExecuteQueryDefBase, IExecuteQueryDef
    {
        
        private SqlConnection conn; 

       


        
        public override JObject GetOne(string id)
        {
            if (!queryDefModel.canGet)
                throw new Exception("Current QueryDef does not allow Get Operations");
            JObject ret = new();

            using (conn = GetConnection())
            {
                var query = queryDefModel.getOneQuery.Replace("|fields|", SelectedFieldsList());
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                var Result = cmd.ExecuteReader();
                if (Result.HasRows)
                {
                    Result.Read();
                    for (int a = 0; a < Result.FieldCount; a++)
                    {
                        ret.Add(new JProperty(Result.GetName(a), Result[a]));

                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Get - gets all records in the named query - unless a reducing criteria is specified.  
        /// the sql when criteria ie.  category = 'this' and name='that' 
        /// </summary>
        /// <param name="criteria">the sql when criteria ie.  category = 'this' and name='that' or "" (string empty) for everything  </param>
        /// <returns>JArray of Json objects that represent the rows matching the criteria and the columns matching the fields in the query definition field list </returns>
        public override JArray Get (string criteria = "")
        {
            if (!queryDefModel.canGet)
                throw new Exception("Current QueryDef does not allow Get Operations");
            JArray ret = new();

            using (conn = GetConnection())
            {
                var query = queryDefModel.getQuery.Replace("|fields|", SelectedFieldsList());
                if(!criteria.IsEmpty())
                    query = query + " where " + criteria;

                var cmd = new SqlCommand(query, conn);

                var Result = cmd.ExecuteReader();
                while (Result.Read())
                {
                    JObject curNode = new();
                    
                    for (int a = 0; a < Result.FieldCount; a++)
                    {
                        curNode.Add(new JProperty(Result.GetName(a), Result[a]));
                    }

                    ret.Add(curNode);
                }
            }
            return ret;
        }

        public override int Delete(string id)
        {
            if (!queryDefModel.canDelete)
                throw new Exception("Current QueryDef does not allow delete Operations");
            int ret =0;

            using (conn = GetConnection())
            {
                var query = queryDefModel.deleteQuery;
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                ret = cmd.ExecuteNonQuery();
                
            }
            return ret;
        }

        public override int Update(JObject model)
        {
            if (!queryDefModel.canUpdate)
                throw new Exception("Current QueryDef does not allow update Operations");
            if (!model.ContainsKey("id"))
                throw new Exception("The model does not contain an id property.");

            int ret = 0;
            var query = queryDefModel.updateQuery.Replace("|updateFields|", UpdateFieldString(model) );

            using (conn = GetConnection())
            { 
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", model["id"].Value<string>()); 
                ret = cmd.ExecuteNonQuery();

            }
            return ret;
        }

       
        public override int Insert(JObject model)
        {
            if (!queryDefModel.canAdd)
                throw new Exception("Current QueryDef does not allow update Operations");
            if (!model.ContainsKey("id"))
                throw new Exception("The model does not contain an id property.");

            int ret = 0;
            var query = queryDefModel.updateQuery
                .Replace("|fields|", SelectedFieldsList())
                .Replace("|values|oh", InsertValuesList (model));

            using (conn = GetConnection())
            {
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", model["id"].Value<string>());
                ret = cmd.ExecuteNonQuery();

            }
            return ret;
        }


        /*
         *   UTILITY METHODS
         *   
         */
        

        protected string UpdateFieldString(JObject model)
        {
            StringBuilder s = new();
            string comma = "";
            foreach (var field in queryDefModel.fields)
            {
                if (model.ContainsKey(field.dbName))
                {
                    s.Append(comma);
                    s.Append(" [" + field.dbName + "] = '" + model[field.dbName].Value<string>() + "'");
                    comma = ",";
                }
            }

            return s.ToString();
        }

        protected  string InsertValuesList(JObject model)
        {
            StringBuilder s = new();
            string comma = "";
            foreach (var field in queryDefModel.fields)
            {
                if (model.ContainsKey(field.dbName))
                {
                    s.Append(comma);
                    s.Append(" '" + model[field.dbName].Value<string>() + "'");
                    comma = ",";
                }
            }

            return s.ToString();
        }

        protected string SelectedFieldsList()
        {
            StringBuilder s = new();
            string comma = "";
            foreach (var field in queryDefModel.fields)
            {
                if (!field.dbName.IsEmpty())
                {
                    s.Append(comma);
                    s.Append(" [" + field.dbName + "]");
                    comma = ",";
                }
            }

            return s.ToString();
        }



        /// <summary>
        /// builds correct connection string based on the connectionModel definition
        /// </summary>
        /// <returns>connection string</returns>
        protected override string getConnectionString()
        {
            string ret = connectionModel.connectionString;

            if (connectionModel.AuthModel == "MSSQL")
            {
                ret = ret + $" User Id={connectionModel.UserId} ; Password={connectionModel.Password}; ";
            }
            else if (connectionModel.AuthModel == "Integrated")
            {
                ret = ret + " Integrated Security=true;";
            }
            else if (connectionModel.AuthModel == "Trust")
            {
                ret = ret + " Trusted_Connection=True;";
            }
            else
            {
                throw new Exception("Connection .AuthModel not recognized - only MSSQL, Integrated, or trust allowed for MSSQL engine Type");
            }

            ret = ret.Replace("databaseName", connectionModel.dbName);


            return ret;

        }

        /// <summary>
        /// returns an active SQL Connectiton.
        /// </summary>
        /// <returns>Open SQLConnection object</returns>
        protected SqlConnection GetConnection()
        {
            connectionString = getConnectionString();

            if (String.IsNullOrEmpty(connectionString))
                throw new Exception("A Connection String Has Not Been Defined");

            conn = new SqlConnection(connectionString);

            conn.Open();
            return conn;
        }
    }

}
