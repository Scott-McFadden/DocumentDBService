using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL
{
    public static class DomainLookUpService
    {
        private static Dictionary<string, DomainLookUpModel> cache = new(); 
        private static ADOBase<DomainLookUpModel> DB;


        static DomainLookUpService()
        {

        }

        /// <summary>
        /// populates cache from database.
        /// </summary>
        /// <param name="db">ADOBase(ConnectionModel) that can access the appropriate database table.</param>
        public static void Populate(ADOBase<DomainLookUpModel> db)
        {
            DB = db;
            

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
            List<DomainLookUpModel> rawDocs = DB.Get();
            cache = new();

            foreach (var d in rawDocs)
            {
                cache.Add(d.Key(), d);
            }
        }

        /// <summary>
        /// add a model to the cache
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true is successful</returns>
        public static bool Add(DomainLookUpModel model)
        {
            bool ret;
            try
            {
                if (cache.ContainsKey(model.Key()))  // key already exists
                    return false;

                cache.Add(model.Key(), model);


                DB.Add(model);
                ret = true;
            } catch (Exception ex)
            {
                throw new Exception("failed to add to DomainLookUpModel cache", ex);
            }
            return ret;
        } 

        /// <summary>
        /// Gets a list of DomainLookUpModel Values for a given category in the cache
        /// </summary>
        /// <returns>list of names</returns>
        public static List<string> GetValues(string cat, string name)
        {
            return cache.Where(a => a.Value.Category == cat  && a.Value.Name == name).Select(a => a.Value.Value).ToList();
        }

        /// <summary>
        /// Gets a list of category names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCategories()
        {
            return cache.Select(a => a.Value.Category).Distinct().ToList();
        }
        /// <summary>
        /// remove an entry from the cache
        /// </summary>
        /// <param name="Key">Category_Name or model.key</param>
        /// <returns></returns>
        public static bool Delete(string Key)
        {
            Task.Run(() => DB.Delete(cache[Key].id)).Wait();
            cache.Remove(Key);

            return true;
        }
        /// <summary>
        /// removes an entry for the cache and db
        /// </summary>
        /// <param name="guid">guid id for target</param>
        /// <returns>true if removed - or exception is something goes wrong</returns>
        public static bool Delete(Guid guid)
        {
            Task.Run(() => DB.Delete(guid)).Wait();
            cache.Remove(Get(guid).Key());

            return true;
        }
        /// <summary>
        /// replace a model defintion
        /// </summary>
        /// <param name="model">querydefmodel to cache</param>
        /// <returns>true if successful</returns>
        public static bool Replace(DomainLookUpModel model)
        {
            cache[model.Key()] = model;


            DB.Update(model);

            return true;
        }
        /// <summary>
        /// returns by id 
        /// </summary>
        /// <param name="id"> Guid</param>
        /// <returns>selected item </returns>
        public static DomainLookUpModel Get(Guid id)
        {
            try
            {
                return cache.Where(a => a.Value.id == id).Select(a => a.Value).First();
            }
            catch
            {
                return new DomainLookUpModel();
            }

        }



        /// <summary>
        /// returns DomainLookUpModel based on Cat and Name
        /// </summary>
        /// <param name="cat">category</param>
        /// <param name="name">name</param>
        /// <returns></returns>
        public static DomainLookUpModel Get(string cat, string name, string value)
        {
            return cache[cat + "_" + name + "_" + value];
        }

        /// <summary>
        /// gets a named DomainLookUpModel based on key (CATEGORY_NAME)
        /// </summary>
        /// <param name="key">name to find</param>
        /// <returns>QueryDefModel object</returns>
        public static DomainLookUpModel Get(string Key)
        {
            return cache[Key];
        }

        public static List<DomainLookUpModel> Get()
        {
            return cache.Values.ToList();
        }
    }
}
