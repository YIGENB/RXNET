using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;


namespace RX.Web
{
    public static class Json
    {
        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ToJson(this object obj)
        {
            try
            {
                return obj.ToJson(null);
            }
            catch (Exception ex)
            {

                throw new Exception("RX.Ext.JsonExt: " + ex.Message);
            }
        }

        public static string ToJson<TSource, TResult>(this IEnumerable<TSource> varlist, Func<TSource, TResult> selector)
        {
            return varlist.Select<TSource, TResult>(selector).ToJson();
        }

        /// <summary> 
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T FromJson<T>(this string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("RX.Ext.JsonExt: " + ex.Message);
            }
        }

        public static string ToJson(this object obj, IEnumerable<JavaScriptConverter> jsonConverters)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer
            {
                MaxJsonLength = 0x7fffffff
            };
            if (jsonConverters != null)
            {
                if (jsonConverters == null)
                {
                }
                serializer.RegisterConverters((IEnumerable<JavaScriptConverter>)new JavaScriptConverter[0]);
            }
            return serializer.Serialize(obj);
        }



    }
}
