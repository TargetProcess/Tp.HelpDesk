using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;

namespace Hd.Web.Extensions.Components
{
    /// <summary>
    /// Summary description for Globals.
    /// </summary>
    public class Globals
    {
        public Globals()
        {
        }


		public static readonly string LOGIN_COOKIE = "LOGIN" + Globals.ApplicationHostAndPath;
		public static readonly string PASSWORD_COOKIE = "PASSWORD" + Globals.ApplicationHostAndPath;


        public static string CurrentUrlWithoutQuery
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (context == null)
                    return "";

                return RemoveQueryString(context.Request.Url.AbsoluteUri);
            }
        }

        public static string CurrentUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (context == null)
                    return "";

                return context.Request.Url.AbsoluteUri.ToString();
            }
        }

        public static string CurrentQueryString
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (context == null)
                    return "";

                return context.Request.QueryString.ToString();
            }
        }


        public static string CurrentEncodedUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (context == null)
                    return "";

                return HttpUtility.UrlEncode(context.Request.Url.AbsoluteUri);
            }
        }

        public static Int32 GetIndentityFromQuery(Enum column)
        {
            HttpContext context = HttpContext.Current;

            if (ReferenceEquals(context, null))
                return 0;

            string value = context.Request.QueryString[column.ToString()];

            if (ReferenceEquals(value, null) || value == string.Empty)
                return 0;

            return Int32.Parse(value);
        }

        public static NameValueCollection CurrentQuery
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (context == null)
                    return new NameValueCollection();

                return context.Request.QueryString;
            }
        }


        public static string AppendQueryParameters(string url, string parameters)
        {
            if (url.IndexOf('?') != -1)
            {
                if (url[url.Length - 1] == '?')
                    url += parameters;
                else
                    url += "&" + parameters;
            }
            else
                url += "?" + parameters;

            return url;
        }

        public static string RemoveQueryString(string url)
        {
            if (url.IndexOf("?") > 0)
                url = url.Substring(0, url.IndexOf("?"));

            return url;
        }


        public static string ResolveUrl(string appPath, string url)
        {
            if (url.Length == 0 || url[0] != '~')
                return url; // there is no ~ in the first character position, just return the url
            else
            {
                if (url.Length == 1)
                    return appPath; // there is just the ~ in the URL, return the appPath
                if (url[1] == '/' || url[1] == '\\')
                {
                    // url looks like ~/ or ~\
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(2);
                    else
                        return "/" + url.Substring(2);
                }
                else
                {
                    // url looks like ~something
                    if (appPath.Length > 1)
                        return appPath + "/" + url.Substring(1);
                    else
                        return appPath + url.Substring(1);
                }
            }
        }

        public static string BinPath
        {
            get
            {
                AppDomainSetup information = AppDomain.CurrentDomain.SetupInformation;
                return information.ApplicationBase + information.PrivateBinPath + "/";
            }
        }

        public static string ResolveClientUrl(string url)
        {
            if (url.IndexOf("~/") < 0)
                return url;

            url = url.Replace("~/", "");

            return ApplicationPath + "/" + url;
        }

        public static string ApplicationPath
        {
            get
            {
                HttpContext Context = HttpContext.Current;

                if (Context == null)
                    return "";

                return Context.Request.ApplicationPath == "/"
                           ? ""
                           : Context.Request.ApplicationPath;
            }
        }

        public static bool IsLogOut
        {
            get
            {
                HttpContext context = HttpContext.Current;

                if (context.Session["IsLogOut"] != null)
                    return (bool) context.Session["IsLogOut"];

                return false;
            }
            set
            {
                HttpContext context = HttpContext.Current;
                context.Session["IsLogOut"] = value;
            }
        }

        public static string ApplicationHostAndPath
        {
            get
            {
                HttpContext Context = HttpContext.Current;

                if (Context == null)
                    return "";

                HttpRequest request = Context.Request;
                StringBuilder hostAndPath = new StringBuilder();
                
                hostAndPath.Append(request.Url.Scheme)
                    .Append("://")
                    .Append(request.Url.Host);

                // append Port if required
                int index = CurrentUrl.LastIndexOf(":" + request.Url.Port);
                if (index > -1)
                    hostAndPath.Append(":").Append(request.Url.Port);

                // add application path (do not add last slash if exists)
                string path = request.ApplicationPath;
                if (path != "/" && path != "\\")
                    hostAndPath.Append(path);
                
                return hostAndPath.ToString();

            }
        }
    }
}