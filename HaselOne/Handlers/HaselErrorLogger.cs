using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using log4net;


namespace HaselOne.Handlers
{

    /// <summary&gt;
    /// The HttpModule catches any unhandled exception by IIS and passes it to Log4NET. 
    /// </summary&gt;
    /// <remarks&gt;
    /// Logging can be disabled by setting 'LogUnhandledExceptions' in app.config or web.config to 'false'. Alternatively, the HttpModule
    /// can simply be removed. It is possible to install the module on IIS as a global managed module, so that all unhandled exceptions
    /// for all methods can be logged. Use the files in the \Install folder to see how.
    /// </remarks&gt;
    public class HaselErrorLogger : IHttpModule
    {
        private bool logUnhandeldExceptions;

        public void Init(HttpApplication context)
        {
            bool success = bool.TryParse(ConfigurationManager.AppSettings["LogUnhandledExceptions"], out logUnhandeldExceptions);
            if (!success) { logUnhandeldExceptions = true; }

            context.Error += new EventHandler(OnError);
        }

        private void OnError(object sender, EventArgs e)
        {
            try
            {
                if (!logUnhandeldExceptions) { return; }

                string userIp;
                string url;
                string exception;

                HttpContext context = HttpContext.Current;

                if (context != null)
                {
                    userIp = context.Request.UserHostAddress;
                    url = context.Request.Url.ToString();

                    // get last exception, but check if it exists
                    Exception lastException = context.Server.GetLastError();

                    if (lastException != null)
                    {
                        string mes = lastException != null ? lastException.Message : "";
                        string innerMess = (lastException != null && lastException.InnerException != null) ? lastException.InnerException.Message : "";
                        string fullException = lastException.ToString();
                        exception = ""+ mes +" innerMes: "+ innerMess;
                    }
                    else
                    {
                        exception = "no error";
                    }
                }
                else
                {
                    userIp = "no httpcontext";
                    url = "no httpcontext";
                    exception = "no httpcontext";
                }

                Logging.Instance.Error("Unhandled exception occured. UserIp [{0}]. Url [{1}]. Exception [{2}]", userIp, url, exception);
            }
            catch (Exception ex)
            {
                Logging.Instance.Error("Exception occured in OnError: [{0}]", ex.ToString());
            }
        }

        public void Dispose()
        {
        }
    }
}