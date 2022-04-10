using DocumentDbDAL;
using DocumentDbDAL.Models;
using Serilog;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using DocumentDbDAL.Services;

namespace DocumentDBService
{
    public class DBServices : IDBServices
    {
        private string ConnectionString = "";
        private string Env = "";
        public ADOBase<DocumentDBModel> docDb { get; set; }
        public ADOBase<DomainLookUpModel> lookupdb { get; set; }
        public ADOBase<ConnectionModel> connectiondb { get; set; }
       
        private JObject cfg;
        
        public   DBServices ( )
        {
            string f = File.ReadAllText(@"appsettings.json");
            cfg = JObject.Parse(f);
            Env = cfg.SelectToken($"data.Env").Value<string>();
            ConnectionString = cfg.SelectToken($"ConnectionStrings.{Env}").Value<string>().ResolveIP("DocumentDb");

              Reload();
        }

        public void Reload()
        {
            docDb = new(ConnectionString, "dbo.DocTable"); 
            lookupdb = new ADOBase<DomainLookUpModel>(ConnectionString, "dbo.LookUpTable");
            connectiondb = new ADOBase<ConnectionModel>(ConnectionString, "dbo.connections");

             
                ConnectionService.Populate(connectiondb);  
                  QueryDefService.Populate(docDb); 
                 DomainLookUpService.Populate(lookupdb);  
                  FeatureFlagService.Populate(lookupdb); 
                 
 
            Log.Information("DBServices  load Complete");

            
        }
    }
}
