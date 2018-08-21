using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RX.Web
{
    public class Domain
    {
        public static string domain = VirtualPathUtility.ToAbsolute("~/");

        public static string domainHttp = domain;



        /// <summary>
        /// 应用程序的主机地址
        /// </summary>
        public static string HostUrl
        {
            get
            {
                string url = "http://"
                + HttpContext.Current.Request.Url.Host
                + (HttpContext.Current.Request.Url.Port == 80 ? "" : ":" + HttpContext.Current.Request.Url.Port)
                + (HttpContext.Current.Request.ApplicationPath == "/" ? "/" : HttpContext.Current.Request.ApplicationPath + "/");

                return url;
            }
        }


        public static string httpFilePath = "http://"
               + HttpContext.Current.Request.Url.Host
               + (HttpContext.Current.Request.Url.Port == 80 ? "" : ":" + HttpContext.Current.Request.Url.Port);
    }
}
