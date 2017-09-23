using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using AutoMapper;
using BusinessObjects.Base;
using DAL;
using DAL.Helper;
using HaselOne.Util;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using HaselOne.Ioc;
using HaselOne.Moduls.Customer;

namespace HaselOne
{
    public class Global : HttpApplication
    {
        private static Dictionary<string, UserKnowledge> _sessionInfo;
        private static readonly object padlock = new object();

        public static Dictionary<string, UserKnowledge> Sessions
        {
            get
            {
                lock (padlock)
                {
                    if (_sessionInfo == null)
                    {
                        _sessionInfo = new Dictionary<string, UserKnowledge>();
                    }
                    return _sessionInfo;
                }
            }
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    if (!Sessions.ContainsKey(Session.SessionID))
        //        Sessions.Add(Session.SessionID, new UserKnowledge());
        //}

        //protected void Session_End(object sender, EventArgs e)
        //{
        //    Sessions.Remove(Session.SessionID);
        //}

        private void Application_Start(object sender, EventArgs e)
        {
            DisableApplicationInsightsOnDebug();

            Helper.StaticGuid = Helper.GenGuidStaticLife().ToString();
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            OneMap.Config();

            log4net.Config.XmlConfigurator.Configure();

            //var list=  new CheckDb().Check();
            //if(list.Count>0)
            //    throw new Exception("uyumsuz tablo"+ list.ToArray());
        }

        private void Application_Error(object sender, EventArgs e)
        {
            var app = ((HttpApplication)sender);
            // if(sender)
            //  if (app.Context.Request.Url.Host.IndexOf("localhost", StringComparison.Ordinal) == -1)
            {
                Exception exc = Server.GetLastError();

                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;
                var ai = new TelemetryClient();
                ai.TrackException(exc);
                var a = app.Context.Request.Url;
                string u = "kullanici id :" + CurrentUser.CurrentUserId;
                Logger.Log(new ExceptionCustom() { Ex = exc, UserInfo = GetClientInfo(), Request = a, UserId = u });
                //Server.Transfer("500.aspx");
            }
        }

        protected string GetClientInfo()
        {
            var context = HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            string s =
                  "Browser Capabilities"
                + " Type = " + browser.Type
                + " Name = " + browser.Browser
                + " Version = " + browser.Version
                + " Major Version = " + browser.MajorVersion
                + " Minor Version = " + browser.MinorVersion
                + " Platform = " + browser.Platform
                + " Is Beta = " + browser.Beta
                + " Is Crawler = " + browser.Crawler
                + " Is AOL = " + browser.AOL
                + " Is Win16 = " + browser.Win16
                + " Is Win32 = " + browser.Win32
                + " Supports Frames = " + browser.Frames
                + " Supports Tables = " + browser.Tables
                + " Supports Cookies = " + browser.Cookies
                + " Supports VBScript = " + browser.VBScript
                + " Supports JavaScript = " + browser.EcmaScriptVersion.ToString()
                + " Supports Java Applets = " + browser.JavaApplets
                + " Supports ActiveX Controls = " + browser.ActiveXControls

                + "Supports JavaScript Version = " +
                    browser["JavaScriptVersion"];

            return "</br>" + context.Request.ServerVariables["REMOTE_ADDR"] + s + "</br>";
        }

        [Conditional("DEBUG")]
        private static void DisableApplicationInsightsOnDebug()
        {
            TelemetryConfiguration.Active.DisableTelemetry = true;
        }
    }
}