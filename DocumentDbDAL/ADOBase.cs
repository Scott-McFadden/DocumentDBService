using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace DocumentDbDAL
{
    /// <summary>
    /// ADOBase - creates a SQL access service that will access a table that matches the T model perfectly.
    /// </summary>
    /// <typeparam name="T">Model to operate against.  Must implement the IModel interface.</typeparam>
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

        /// <summary>
        /// Create a connection to an sql database for a data model that adheres to the IModel interface.
        /// </summary>
        /// <param name="connection">Connection string to access the database</param>
        /// <param name="tableName">name of the SQL Table that will be used.</param>
        public ADOBase(string connection, string tableName)
        {
            if (!(singleItem is IModel myobj))
                throw new Exception("Selected model does not implement the IModel interface");

            connectionString = connection;
            TableName = tableName;
            fields = singleItem.GetPropertyNames();
            fieldList = fields.ConvertListOfStringsToCommaSep();
            // get properties 
            foreach(var v in fields)
            {
                Properties.Add(v, objectType.GetProperty(v));
            }
        }

       /// <summary>
       /// get a single record based on the id
       /// </summary>
       /// <param name="id">guid that identifies the record</param>
       /// <returns>T type object</returns>
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

        /// <summary>
        /// gets many records of the T type in a list.
        /// </summary>
        /// <param name="whereClause">optional - standard SQL selection criteria for the model presented</param>
        /// <returns>List of objects.</returns>
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
                        if(rdr[fields[a]].GetType()  != typeof(System.DBNull))
                            Properties[fields[a]].SetValue(ret, rdr[fields[a]]);
                    }

                    ret2.Add(ret);
                }
                return ret2;
            }
        }

        /// <summary>
        /// Add new T record to the database
        /// </summary>
        /// <param name="item">completed model of type T</param>
        /// <returns>number of records affected - should only be 1</returns>
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

        /// <summary>
        /// removes a record with the given id from the database
        /// </summary>
        /// <param name="id">guid to use</param>
        /// <returns>number of records removed - should only be 1</returns>
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

        /// <summary>
        /// Updates the selected T record.
        /// </summary>
        /// <param name="item">Completed T model.</param>
        /// <returns>number of records affected - should only be 1</returns>
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

        /// <summary>
        /// populates a T model from the database.  All fields must match model.
        /// </summary>
        /// <param name="rdr">SQLDataReader holding the current record</param>
        /// <returns>Populated T record.</returns>
        protected T populate(ref SqlDataReader rdr)
        {
            T ret = new T();

            for (var a = 0; a < rdr.VisibleFieldCount; a++)
            {
                Properties[fields[a]].SetValue(ret, rdr[fields[a]]);
            }

            return ret;
        } 

        /// <summary>
        /// returns an active SQL Connectiton.
        /// </summary>
        /// <returns>Open SQLConnection object</returns>
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
