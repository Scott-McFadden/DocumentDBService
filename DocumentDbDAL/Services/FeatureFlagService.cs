using DocumentDbDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentDbDAL.Services
{
    public static class FeatureFlagService
    {
        private static Dictionary<string, DomainLookUpModel> _cache;
        private static ADOBase<DomainLookUpModel> DB;
        private static Dictionary<string, DomainLookUpModel> cache = new Dictionary<string, DomainLookUpModel>();
         
        /// <summary>
        /// populates cache from database.
        /// </summary>
        /// <param name="db">ADOBase(ConnectionModel) that can access the appropriate database table.</param>
        public static void Populate(ADOBase<DomainLookUpModel> db)
        {
            DB = db; 
            LoadFromDB();
        }

        public static object Get()
        {
            return cache.Values;
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
            List<DomainLookUpModel> rawDocs = DB.Get().Where(a=> a.Category == "FeatureFlags").ToList();
            cache = new();

            foreach (var d in rawDocs)
            {
                cache.Add(d.Name, d);
            }
        }

        public static bool IsOn(string name)
        {
            return cache[name].Value.ToLower() == "true";
        }

        public static bool IsOff(string name)
        {
            return cache[name].Value.ToLower() != "true";
        }

        public static string GetValue(string name)
        {
            try
            {
                return cache[name].Value;
            }
            catch
            {
                return "undefined";
            }
           
        }

        public static bool Set(string name, string value, bool persist = false)
        {
            try
            {
                cache[name].Value = value;
                if (persist)
                {
                    DomainLookUpModel model = cache[name];
                    model.Value = value;
                    DB.Update(model);
                }
            }
            catch
            {
                DomainLookUpModel model = new DomainLookUpModel();
                model.Name = name;
                model.Value = value;
                model.Category = "FeatureFlags";
                cache.Add(model.Name, model);
                DB.Add(model);
                
            }
            return true;
        }
        public static bool Remove(string name,   bool persist = false)
        {
            try
            { 
                var model = cache[name];
                if (persist)
                {
                    DB.Delete(model.id);
                }
                cache.Remove(name);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("Featureflag remove error", ex );
                // nothing to remove
            }
            return true;
        }
    }
}
