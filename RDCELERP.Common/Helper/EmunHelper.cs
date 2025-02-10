
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.Common.Helper
{
    public static class EnumHelper
    {
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        /// <summary>
        /// Method to get the list of enum description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<ListItem> EnumToList<T>()
        {
            var array = (T[])(Enum.GetValues(typeof(T)).Cast<T>());
            var array2 = Enum.GetNames(typeof(T)).ToArray<string>();
            List<ListItem> lst = null;
            for (int i = 0; i < array.Length; i++)
            {
                if (lst == null)
                    lst = new List<ListItem>();
                string name = array2[i];

                T Tvalue = array[i];
                name = Tvalue.DescriptionAttr<T>();
                string value = Convert.ToInt32(array[i]).ToString();
                lst.Add(new ListItem { Name = name, Value = value });
            }
            return lst;
        }
    }

    public class ListItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
