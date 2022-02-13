using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL.Models
{
    public class NameAndDescription
    {
        public object id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static NameAndDescription Fill (object id, string name, string description)
        {
            return new NameAndDescription() { id = id, Name = name, Description = description };
        }
    }
}
