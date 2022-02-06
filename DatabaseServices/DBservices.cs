using DocumentDbDAL;
using DocumentDbDAL.Models; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Serilog; 

namespace DatabaseServices
{

    public class DBServices : IDBServices
    {
        private string ConnectionString = "";

        public ADOBase<DocumentDBModel> docDb { get; set; }
        public ADOBase<DomainLookUpModel> lookupdb { get; set; }
        public ADOBase<ConnectionModel> connectiondb { get; set; }
          
        public DBServices(string connectionString )
        {
            ConnectionString = connectionString;

            docDb = new(ConnectionString, "dbo.DocTable");
            lookupdb = new ADOBase<DomainLookUpModel>(ConnectionString, "dbo.LookUpTable");
            connectiondb = new ADOBase<ConnectionModel>(ConnectionString, "dbo.connections");
            ConnectionService.Populate(connectiondb);
            QueryDefService.Populate(docDb);
            Log.Information ("DBServices initialization Complete");

        }
    }
}
