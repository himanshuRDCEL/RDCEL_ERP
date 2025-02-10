using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Owin.Security.Provider;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Helper
{
    public static class TrimHelper
    {
        public static TEntity TrimAllValuesInModel<TEntity>(this TEntity source) where TEntity : class
        {
            if (source != null)
            {
                PropertyInfo[] Props = source.GetType().GetProperties();
                foreach (PropertyInfo prop in Props)
                {
                    var res = prop.GetValue(source);
                    if (res?.GetType() == typeof(string) && res != null)
                    {
                        prop.SetValue(source, res.ToString()?.TrimStart()?.TrimEnd());
                    }
                }
            }
            return source;
        }      
    }
}
