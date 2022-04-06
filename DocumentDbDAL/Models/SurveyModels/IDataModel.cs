using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL.Models.SurveyModels
{
    public abstract class IDataModel
    {
        [Key]
        
        public Guid id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        [JsonIgnore]
        public List<string> Reasons = new List<string>();    // used to collect validation errors

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public void ValidateIModel()
        {
            Reasons = new();
            if (String.IsNullOrEmpty(Name)) Reasons.Add("Name cannot be empty");
            if (String.IsNullOrEmpty(Description)) Reasons.Add("Description cannot be empty");
            if (String.IsNullOrEmpty(Tags)) Reasons.Add("Tags cannot be empty");
            if (String.IsNullOrEmpty(Roles)) Reasons.Add("Roles cannot be empty");
        }

        public abstract bool Validate();

        
    }
}
