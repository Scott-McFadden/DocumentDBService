using DocumentDbDAL;
using DocumentDbDAL.Models;
using System.Threading.Tasks;

namespace DocumentDBService
{
    public interface IDBServices
    {
        ADOBase<ConnectionModel> connectiondb { get; set; }
        ADOBase<DocumentDBModel> docDb { get; set; }
        ADOBase<DomainLookUpModel> lookupdb { get; set; }

        void Reload();
    }
}