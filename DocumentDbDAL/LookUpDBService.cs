using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{
    public class LookUpDbService  : ADOBase<DomainLookUpModel> 
    { 
        public LookUpDbService(string connection, string tableName):base(connection, tableName)
        {
            
        }

        
    }


   
}
