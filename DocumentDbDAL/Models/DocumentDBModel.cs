using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentDbDAL.Models
{

    public class DocumentDBModel : IModel
    {
        
        public Guid id { get; set; } = Guid.NewGuid();
        public string category { get; set; } = "";
        public string JsonDoc { get; set; } = "";
        public string Owner { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateLastChanged { get; set; } = DateTime.UtcNow;

        public string GetInsertValues()
        {
            return $"'{id}', '{category}', '{JsonDoc}', '{Owner}', '{DateCreated.ToString("yyyy-MM-dd HH:mm:ss.fff")}',  '{DateLastChanged.ToString("yyyy-MM-dd HH:mm:ss.fff")}' ";
            
        }

        public string GetSetValues()
        {
            return $"category = '{category}' , JsonDoc = '{JsonDoc}', Owner = '{Owner}', DateLastChanged = '{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") }' ";
        }
    }


}
