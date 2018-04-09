using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LitJson
{
    class JsonUtil
    {   
        /// <summary>
        /// object to json string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        internal static String toJson(Object o)
        {
            return JsonMapper.ToJson(o);
        }
        /// <summary>
        /// json string to jsonObject
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        internal static JsonData toObject(String o)
        {
            return JsonMapper.ToObject(o);
        }
    }
}
