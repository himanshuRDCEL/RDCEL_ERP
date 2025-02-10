using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Datasafe.Common.Helper
{
    public static class GenericConversionHelper
    {

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {

            List<T> list = null;
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(table);
                list = JsonConvert.DeserializeObject<List<T>>(JSONresult);
            }
            catch (Exception ex)
            {
                Logging.WriteErrorToLog("GenericConversionHelper", "DataTableToList", ex);
            }
            return list.ToList();
        }


    }

}
