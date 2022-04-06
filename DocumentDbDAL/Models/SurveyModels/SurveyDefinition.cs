using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocumentDbDAL.Models.SurveyModels
{
    public class SurveyDefinitionModel : IDataModel
    {
        public string WorkFlowDefinitionId { get; set; } = "undefined";
        public string SurveyDetails { 
            get 
            {  
                return _surveyDefinition.ToString();  
            }  
            set
            {
                GetQueryDefListFromSurveyDefinition(value.ToString());
                _surveyDefinition = JObject.Parse(value); 
            }
        }
        public List<ModificationModel> Modifications { get; set; } = new List<ModificationModel>();
        public List<string> QueryDefNames { get; set; } =   new List<string>();
        public List<KeyValuePair<string, string>> QueryDefItems { get; set; } = new();
        public Dictionary<string, string> ConstantValues = new Dictionary<string, string>();
        public List<ScopeItem> ScopeDetails { get; set; } = new();

        #region Privates
        private JObject _surveyDefinition = new JObject();
        #endregion
        // -----------------------------------------------------------------------

        public static SurveyDefinitionModel Deserialize(string model)
        {
            return JsonConvert.DeserializeObject<SurveyDefinitionModel>(model);
        }

        public override bool Validate()
        {
            ValidateIModel();
            if (String.IsNullOrEmpty(WorkFlowDefinitionId)) Reasons.Add("WorkFlowDefintionId is empty");
            if (!_surveyDefinition.HasValues) Reasons.Add("Survey Details is empty");
            return Reasons.Count > 0;
        }


        /// <summary>
        /// populates the QueryDefNames and QueryDefItems 
        /// </summary>
        /// <param name="definition">Survey Definition</param>
        private void GetQueryDefListFromSurveyDefinition(string definition)
        {  
            Regex rx2 = new Regex("(?:\"@@)(.*(?:\"))",
                RegexOptions.Compiled | RegexOptions.IgnoreCase); 

            MatchCollection match2 = rx2.Matches(definition);
            if (match2.Count > 0)
            {
                foreach (Match match in match2)
                {
                    var data = match.Value.Replace("@@", "").Replace("\"", "");
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(
                        data.Substring(0,data.IndexOf(".") ),
                        data.Substring(data.IndexOf(".") + 1)  
                        ); 

                    if (!QueryDefItems.Contains(pair))
                        QueryDefItems.Add(pair); 
                }

                QueryDefNames = QueryDefItems.Select(a => a.Key).Distinct().ToList();
            }
        }
    }
}
