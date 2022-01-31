using System;

namespace DocumentDbDAL.Models
{
    public interface IModel
    {
        Guid id { get; set; }
        string GetSetValues();
        string GetInsertValues();
    }


}
