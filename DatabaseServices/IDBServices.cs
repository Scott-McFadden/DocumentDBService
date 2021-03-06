using DocumentDbDAL;
using DocumentDbDAL.Models;

namespace DatabaseServices
{
    public interface IDBServices
    {
        ADOBase<ConnectionModel> connectiondb { get; set; }
        ADOBase<DocumentDBModel> docDb { get; set; }
        ADOBase<DomainLookUpModel> lookupdb { get; set; }
    }
}