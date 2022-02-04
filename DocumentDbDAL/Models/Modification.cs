using Newtonsoft.Json;
using System;

namespace DocumentDbDAL.Models
{
    public class ModificationModel
    {
        /// <summary>
        /// When did it happen
        /// </summary>

        public DateTime dateModified { get; set; } =   DateTime.Now;
        /// <summary>
        /// Who did it
        /// </summary>
        public string who { get; set; } = "";
        /// <summary>
        /// Work Ticket Number
        /// </summary>
        public string jiraTicket { get; set; } = "";
        /// <summary>
        /// What did they do
        /// </summary>
        public string description { get; set; } = "";
        /// <summary>
        /// creates an instance of this object from the string provided.
        /// </summary>
        /// <param name="data">json string representing this object</param>
        /// <returns>new modification object</returns>
        public static ModificationModel deserialize(string data)
        {
            return JsonConvert.DeserializeObject<ModificationModel>(data);
        }
    }
}