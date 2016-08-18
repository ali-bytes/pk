using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Ui.EStore.Helpers
{
    public static class ExtensionMethods
    {
        public static DateTime ToDefaultLocalTime(this DateTime dt, double offsetInMinutes)
        {
            dt = dt.AddMinutes(offsetInMinutes);
            return dt;
        }

    
        public static string ClearString(this string st)
        {
            if (string.IsNullOrEmpty(st)) return st;
            
            st = st.Replace("&", "-");
            st = st.Replace("<", "-");
            st = st.Replace(">", "-");
            st = st.Replace(":", "-");
            st = st.Replace("*", "-");
            st = st.Replace("%", "-");
            st = st.Replace("\\", "-");
            st = st.Replace("/", "-");
            st = st.Replace(".", "-");
            return st;
        }

        public static StringWriter WriteTsv<T>(this IEnumerable<T> data)
        {
            var output = new StringWriter();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
            return output;
        }
    }
}