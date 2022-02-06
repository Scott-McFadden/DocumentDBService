using DocumentDbDAL;
using DocumentDbDAL.Models; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace DocumentDBService
{

    public class DBServices : IDBServices
    {
        private string ConnectionString = "";

        public ADOBase<DocumentDBModel> docDb { get; set; }
        public ADOBase<DomainLookUpModel> lookupdb { get; set; }
        public ADOBase<ConnectionModel> connectiondb { get; set; }
        private IConfiguration Config; 

        public DBServices(IConfiguration config )
        {
            Config = config;
            ConnectionString = config.GetConnectionString("DocumentDB");

            docDb = new(ConnectionString, "dbo.DocTable");
            lookupdb = new ADOBase<DomainLookUpModel>(ConnectionString, "dbo.LookUpTable");
            connectiondb = new ADOBase<ConnectionModel>(ConnectionString, "dbo.connections");
            ConnectionService.Populate(connectiondb);
            QueryDefService.Populate(docDb);
            Log.Information ("DBServices initialization Complete");

        }
    }
}
