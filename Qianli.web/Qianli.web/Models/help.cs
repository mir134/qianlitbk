using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Qianli.web.Models
{
    public static class help
    {
        /// <summary>
        /// 获取两个字符串之间的字符
        /// </summary>
        /// <returns></returns>
        public static string GetValueAnd(string strStart, string strEnd, string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            string regex = @"^.*" + strStart + "(?<content>.+?)" + strEnd + ".*$";
            Regex rgClass = new Regex(regex, RegexOptions.Singleline);
            Match match = rgClass.Match(text);
            return match.Groups["content"].Value;
        }

        /// <summary>
        /// 获取Json的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();  //注意 要有T类型要有无参构造函数
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}