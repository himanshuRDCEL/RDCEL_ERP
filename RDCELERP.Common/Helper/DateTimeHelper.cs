using System;
using System.ComponentModel;
using System.Reflection;

namespace RDCELERP.Common.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        

       
    }


}
