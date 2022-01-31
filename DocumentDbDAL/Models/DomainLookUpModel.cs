using System;

namespace DocumentDbDAL.Models
{
    public class DomainLookUpModel : IModel
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string Category { get; set; }
        public string Name { get; set; } 
        public string Value { get; set; }

        public string GetInsertValues()
        {
            return $" '{id}', '{Category}',  '{Name}', '{Value}' ";
        }

        public string GetSetValues()
        {
            return $" [Category]='{Category}', [Name]='{Name}', [Value]='{Value}' ";
        }
    }


}
