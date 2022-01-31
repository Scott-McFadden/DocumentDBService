using DocumentDbDAL.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{
    public class DocumentDB  : ADOBase<DocumentDBModel> 
    { 
        /// <summary>
        /// This sets up connection to a DocumentDB table in SQL based on the DocumentDBModel
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        public DocumentDB (string connection, string tableName):base(connection, tableName)
        {
            
        }

        
    }
}
