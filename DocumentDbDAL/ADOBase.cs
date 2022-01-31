using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace DocumentDbDAL
{
    public  class ADOBase<T> where T : new()
    {
        protected string connectionString = "";
        protected SqlConnection conn;
        protected SqlCommand cmd;
        protected SqlDataReader rdr;
        protected string TableName = "";
        protected List <string> fields = new List<string>();
        protected string fieldList = "";
        protected T singleItem = new T();
        protected Type objectType = typeof(T);
        protected Dictionary<string, PropertyInfo> Properties = new Dictionary<string, PropertyInfo>();
        public ADOBase(string connection, string tableName)
        {
            connectionString = connection;
            TableName = tableName;
            fields = singleItem.GetPropertyNames();
            fieldList = fields.ConvertListOfStringsToCommaSep();

            foreach(var v in fields)
            {
                Properties.Add(v, objectType.GetProperty(v));
            }
            
        }
       

        public T Get(Guid id)
        {
            var queryString = "select " + fieldList + " from " + TableName + " where id=@id";
            T ret = new();
            using (conn = GetConnection())
            {
                cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);

                rdr =  cmd.ExecuteReader();

                while (rdr.Read())
                {
                    for(var a = 0; a<rdr.VisibleFieldCount; a++)
                    { 
                        Properties[fields[a]].SetValue(ret, rdr[fields[a]]); 
                    }
                }
                return ret;
            }
            
        }

        public List<T> Get(string whereClause = "")
        {
            List<T> ret2 = new();

            var queryString = "select " + fieldList + " from " + TableName  ;
            if (!String.IsNullOrEmpty(whereClause))
            {
                queryString += " " + whereClause;
            }

            using (conn = GetConnection())
            {
                cmd = new SqlCommand(queryString, conn); 
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    T ret = new();

                    for (var a = 0; a < rdr.VisibleFieldCount; a++)
                    {
                        Properties[fields[a]].SetValue(ret, rdr[fields[a]]);
                    }

                    ret2.Add(ret);
                }
                return ret2;
            }
        }

        public int Add(T item)
        {
            int ret = 0;
            var queryString = "insert into  " + TableName + " (" + fieldList + ") values (" + ((IModel)item).GetInsertValues() + ")";
            using (conn = GetConnection())
            {
                cmd = new SqlCommand(queryString, conn);
                ret = cmd.ExecuteNonQuery();
            }

            return ret;
        }

        public int Delete(Guid id)
        {
            var queryString = "delete from " + TableName + " where id = @id";
            int ret;
            using (conn = GetConnection())
            {
                cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", id);

                ret = cmd.ExecuteNonQuery(); 
            }
            return ret;
        }

        public int Update(  T item)
        {
            int ret = 0;
            var queryString = "update " + TableName + " set " + ((IModel)item).GetSetValues() + " where id=@id";
            using (conn = GetConnection())
            {
                cmd = new SqlCommand(queryString, conn);
                cmd.Parameters.AddWithValue("@id", ((IModel)item).id);

                ret = cmd.ExecuteNonQuery();
            }
            return ret; 
        }

        protected T populate(ref SqlDataReader rdr)
        {
            T ret = new T();

            for (var a = 0; a < rdr.VisibleFieldCount; a++)
            {
                Properties[fields[a]].SetValue(ret, rdr[fields[a]]);
            }

            return ret;
        }

        protected SqlConnection GetConnection()
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new Exception("A Connection String Has Not Been Defined");

            
            conn = new SqlConnection(connectionString);
           
            conn.Open();
            return conn;
        }
    }
}
