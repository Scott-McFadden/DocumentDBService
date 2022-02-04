using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{
    public static class ConnectionService
    {
        private static Dictionary<string, ConnectionModel> cache = new();
        public static bool PersistChanges = false;  //TODO: Implement Database Cache
        private static ADOBase<ConnectionModel> DB; 


        static ConnectionService()
        {

        }
        /// <summary>
        /// populates cache from database.
        /// </summary>
        /// <param name="db">ADOBase(ConnectionModel) that can access the appropriate database table.</param>
        public  static void Populate(ADOBase<ConnectionModel> db )
        {
            DB = db;
            PersistChanges = true;
            LoadFromDB();
            
        }
        /// <summary>
        /// asks system to refresh the cache
        /// </summary>
        public static void Refresh()
        {
            if (DB == null)
                return;
            LoadFromDB();
        }
        private static void LoadFromDB()
        {
            List <ConnectionModel> rawDocs = DB.Get();
            cache = new();
            foreach(var d in rawDocs)
            {
                 
                cache.Add(d.name, d);
            }
        }
        /// <summary>
        /// add a model to the cache
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true is successful</returns>
        public static bool Add(ConnectionModel model)
        {
            bool ret = false;
            try
            {
                if (cache.ContainsKey(model.name))  // key already exists
                    return false;

                cache.Add(model.name, model);
                 

                DB.Add(model);
                ret = true;
            } catch (Exception ex)
            {
                throw new Exception("failed to add to querydefmodel cache", ex);
            }
            return ret;
        }

        /// <summary>
        /// gets a named ConnectionModel
        /// </summary>
        /// <param name="Name">name to find</param>
        /// <returns>QueryDefModel object</returns>
        public static ConnectionModel Get(string Name)
        {
            return cache[Name];
        }

        /// <summary>
        /// Gets a list of querydefmodel names in the cache
        /// </summary>
        /// <returns>list of names</returns>
        public static List<string> GetName()
        {
            return cache.Select(a => a.Value.name).ToList();
        }

        /// <summary>
        /// remove an entry from the cache
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Delete(string name)
        {
            cache.Remove(name);
             

            DB.Delete(cache[name].id);
            return true;
        }

        /// <summary>
        /// replace a model defintion
        /// </summary>
        /// <param name="model">querydefmodel to cache</param>
        /// <returns>true if successful</returns>
        public static bool Replace(ConnectionModel model)
        {
            cache[model.name] = model;
            

            DB.Update(model);

            return true;
        }
    }
}
