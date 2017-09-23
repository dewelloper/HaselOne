using BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace HaselOne.Util
{
    public static class PageHelper
    {
        public static void RegisterJsFile(MasterPage master, string jsFilePath)
        {
            if (master == null)
                return;
            Control scr = master.FindControl("scriptbase");
            if (scr != null)
                scr.Controls.AddAt(0, new LiteralControl(String.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", jsFilePath)));
        }

        public static void RegisterJs(MasterPage master, string script, string where = "startup_scripts")
        {
            RegisterJs(master, script, null, where);
        }

        public static void RegisterJs(MasterPage master, string script, string var, string where = "startup_scripts")
        {
            if (master == null)
                return;

            Control scr = master.FindControl(where);
            if (scr == null)
                scr = master.FindControl("scriptbase");
            if (scr != null)
            {
                string s = String.Format("<script type=\"text/javascript\">{0}</script>", (var == null ? script : "var " + var + " = " + script));
                scr.Controls.AddAt(0, new LiteralControl(s));
            }
        }

        public static void RegisterJs(MasterPage master, string var, string nullText, string falseText, string trueText, string where = "startup_scripts")
        {
            if (master == null)
                return;

            Control scr = master.FindControl(where);
            if (scr != null)
            {
                var textValues = new TextValue[3];
                textValues[0] = new TextValue() { Value = -1, Text = nullText };
                textValues[1] = new TextValue() { Value = 0, Text = falseText };
                textValues[2] = new TextValue() { Value = 1, Text = trueText };
                var script = JsonConvert(textValues, var + "ValueTypes");
                string s = String.Format("<script type=\"text/javascript\">{0}</script>", script);
                scr.Controls.AddAt(0, new LiteralControl(s));
            }
        }

        public static string JsonConvert(object obj, string var, bool ignoreNullValues = false)
        {
            if (obj != null)
                return "var " + var + " = " + JsonConvert(obj, ignoreNullValues);
            else
                return "var " + var + " = {}";
        }

        public static T JsonConvert<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, new Newtonsoft.Json.JsonSerializerSettings() { DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local });
        }

        public static string JsonConvert(object obj, bool ignoreNullValues = false)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local,
                    NullValueHandling = ignoreNullValues ? Newtonsoft.Json.NullValueHandling.Ignore : Newtonsoft.Json.NullValueHandling.Include,
                   
                });
        }

        public static string JsonConvert<T>(bool ignoreNullValues = false)
        {
            if (typeof(T).IsEnum)
            {
                Dictionary<string, TextValue> values = new Dictionary<string, TextValue>();
                foreach (var item in Enum.GetValues(typeof(T)))
                {
                    var s = item.ToString();
                    values.Add(s, new TextValue() { Value = (int)item, Text = EnumHelper<T>.GetEnumDescription(s) });
                }
                return "var " + typeof(T).Name + " = " + Newtonsoft.Json.JsonConvert.SerializeObject(values,
                    new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local,
                        NullValueHandling = ignoreNullValues ? Newtonsoft.Json.NullValueHandling.Ignore : Newtonsoft.Json.NullValueHandling.Include
                    });
            }
            else
                return null;
        }

        /// <summary>
        /// örnek: var DataSourceTypes = [{"Text":"Name1","Value":0},{"Text":"Name2","Value":1},{"Text":"","Value":-1}]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignoreNullValues"></param>
        /// <returns></returns>
        public static string JsonConvertAsJsObj<T>(bool ignoreNullValues = false, bool? forEditor = null, bool addVar = true)
        {
            if (typeof(T).IsEnum)
            {
                List<TextValue> values = new List<TextValue>();
                var enumValues = Enum.GetValues(typeof(T));
                var list = new Dictionary<int, string>();
                foreach (var item in enumValues)
                {
                    list.Add((int)item, item.ToString());
                }

                foreach (var item in list.OrderBy(_ => _.Key))
                {
                    var s = item.Value;
                    if (forEditor != null)
                    {
                        var propertiesAttr = EnumHelper<T>.GetEnumCustomAttr(s);
                        if (forEditor.Value && !propertiesAttr.IgnoreInEdit) { }
                        else
                            continue;
                    }
                    values.Add(new TextValue() { Value = item.Key, Text = EnumHelper<T>.GetEnumDescription(s) });
                }

                return (addVar ? ("var " + typeof(T).Name + " = ") : "") + Newtonsoft.Json.JsonConvert.SerializeObject(values,
                    new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local,
                        NullValueHandling = ignoreNullValues ? Newtonsoft.Json.NullValueHandling.Ignore : Newtonsoft.Json.NullValueHandling.Include
                    });
            }
            else
                return null;
        }

        public static string UserIp
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]))
                    return HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                return !String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"])
                           ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                           : String.Empty;
            }
        }

        public static void AddScript(Control pg, string sourceFile, Int32 index = -1)
        {
            HtmlGenericControl scriptBlock = new HtmlGenericControl("script");
            scriptBlock.Attributes.Add("type", "text/javascript");
            scriptBlock.Attributes.Add("language", "javascript");
            scriptBlock.Attributes.Add("src", sourceFile);
            if (index > -1)
                pg.Controls.AddAt(index, scriptBlock);
            else
                pg.Controls.Add(scriptBlock);
        }

        public static void WriteStyle(Control pg, string styleText)
        {
            HtmlGenericControl styleBlock = new HtmlGenericControl("style");
            styleBlock.Attributes.Add("text", "text/style");
            styleBlock.Controls.Add(new LiteralControl(styleText));
            pg.Controls.Add(styleBlock);
        }

        public static void AddCss(Control pg, string path)
        {
            AddCss(pg, path, string.Empty);
        }

        public static void AddCss(Control pg, string path, string media, int index = -1)
        {
            HtmlLink cssLink = new HtmlLink { Href = path  /* + "?time=" + DateTime.Now.ToFileTime().ToString()*/ };
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            cssLink.Attributes.Add("media", string.IsNullOrEmpty(media) ? "screen" : media);
            if (index > -1)
                pg.Controls.AddAt(index, cssLink);
            else
                pg.Controls.Add(cssLink);
        }

        public static string GetHostIp(HttpContext cr)
        {
            return cr.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static void LogOut(Page pg, bool deleteKey)
        {
            LogOut(pg, deleteKey, String.Empty);
        }

        public static void LogOut(Page pg, bool deleteKey, string pathToRedirect)
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();

            if (deleteKey)
            {
                FormsAuthentication.SignOut();
            }
            //SecureRedirect( !String.IsNullOrEmpty(pathToRedirect) ? pathToRedirect : pg.ResolveClientUrl("~/login.aspx"), "Oats bağlantınız kapatılıyor.");
            pg.Response.Redirect(pg.ResolveClientUrl("~/login.aspx"));
            //HttpContext.Current.Response.End();
        }
    }
}