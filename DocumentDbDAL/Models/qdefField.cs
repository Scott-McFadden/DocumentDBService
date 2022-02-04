using Newtonsoft.Json;
using System.Collections.Generic;

namespace DocumentDbDAL.Models
{ 
    /// <summary>
    /// standard field defintion
    /// </summary>
    public  class QdefFieldModel
    {
        static int depth = 0;
        /// <summary>
        /// name of field - presented to the ui / application
        /// </summary>
        public string name { get; set; } = "";
        /// <summary>
        /// dbName - the name of the field used by the data engine
        /// </summary>
        public string dbName { get; set; } = "";
        /// <summary>
        /// description of field - think of it being used as hover text
        /// </summary>
        public string description { get; set; } = "";
        /// <summary>
        /// data type of the field
        /// </summary>
        public string dataType { get; set; } = "";
        /// <summary>
        /// validation string
        /// </summary>
        public string validation { get; set; } = "";
        /// <summary>
        /// validation Types allowed by the system
        /// </summary>
        public validationTypes validationType { get; set; } = validationTypes.none;
        /// <summary>
        /// input type the control the ui should use
        /// </summary>
        public string inputType { get; set; } =   "" ;


        private IList<QdefFieldModel> children = null;
        public IList<QdefFieldModel> Children { 
            get
            {
                if (children == null)
                {
                    if (QdefFieldModel.depth < 1)    // limit depth
                       children = new List<QdefFieldModel>() { new QdefFieldModel() };
                    QdefFieldModel.depth++;
                }
                    
                return children;
            } 
            set 
            {
                children = value;
            } 
        }   


        /// <summary>
        /// creates an instance of this object from the string provided.
        /// </summary>
        /// <param name="data">json string representing this object</param>
        /// <returns>new modification object</returns>
        public static QdefFieldModel deserialize(string data)
        {
            return JsonConvert.DeserializeObject<QdefFieldModel>(data);
        }
    }
}