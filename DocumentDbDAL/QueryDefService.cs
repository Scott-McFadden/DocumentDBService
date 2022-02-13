using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{
    public static class QueryDefService
    {
        private static Dictionary<string, QueryDefModel> cache = new();
        public static bool PersistChanges = false;  //TODO: Implement Database Cache
        private static ADOBase<DocumentDBModel> DB; 


        static QueryDefService()
        {

        }
        /// <summary>
        /// populates cache from database.
        /// </summary>
        /// <param name="db">ADOBase(DocumentDBModel) that can access the appropriate database table.</param>
        public static void Populate(ADOBase<DocumentDBModel> db )
        {
            DB = db;
            PersistChanges = true;

            List < DocumentDBModel > rawDocs = DB.Get(" where Category='QueryDef'");

            foreach(var d in rawDocs)
            {
                QueryDefModel q = QueryDefModel.deserialize(d.JsonDoc);
                cache.Add(q.name, q);
            }
        }
        /// <summary>
        /// check to make sure that the given name is not in the cache 
        /// </summary>
        /// <param name="name">name of querydef to check for</param>
        /// <returns></returns>
        public static bool DoesNotHave(string name)
        {
            if (!cache.ContainsKey(name))
                return true;
            else
                return false;
        }

        /// <summary>
        /// add a model to the cache
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true is successful</returns>
        public static bool Add(QueryDefModel model)
        {
            bool ret = false;
            try
            {
                if (cache.ContainsKey(model.name))  // key already exists
                    return false;

                cache.Add(model.name, model);
                ret = true;
            } catch (Exception ex)
            {
                throw new Exception("failed to add to querydefmodel cache", ex);
            }
            return ret;
        }

        /// <summary>
        /// gets a named querydefmodel
        /// </summary>
        /// <param name="Name">name to find</param>
        /// <returns>QueryDefModel object</returns>
        public static QueryDefModel Get(string Name)
        {
            return cache[Name];
        }

        /// <summary>
        /// Gets a list of querydefmodel names in the cache
        /// </summary>
        /// <returns>list of names</returns>
        public static List<NameAndDescription> GetName()
        {
            List<NameAndDescription> ret = new();
            foreach(var item in cache)
            {
                ret.Add(NameAndDescription.Fill(item.Value.id, item.Value.name, item.Value.description));
            }
            return ret;
        }

        /// <summary>
        /// remove an entry from the cache
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Delete(string name)
        {
            cache.Remove(name);
            return true;
        }

        /// <summary>
        /// replace a model defintion
        /// </summary>
        /// <param name="model">querydefmodel to cache</param>
        /// <returns>true if successful</returns>
        public static bool Replace(QueryDefModel model)
        {
            cache[model.name] = model;
            return true;
        }
    }
}
