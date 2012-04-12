using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Hd.Web.Extensions
{
    public class JavaScriptRegistrator
    {
        public static void RegisterEmbeddedScript(Page page, Type type, string scriptName)
        {
            string scriptLocation = page.ClientScript.GetWebResourceUrl(type, "Hd.Web.Extensions.Resources." + scriptName + ".js");

            if (!page.ClientScript.IsClientScriptBlockRegistered(type, scriptName))
                page.ClientScript.RegisterClientScriptInclude(type, scriptName, scriptLocation);
        }
    }
}
