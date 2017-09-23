using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HaselOne.Handlers
{

    /// <summary>
    /// Uses Log4NET to log messages through a Singleton. The configuration is taken from an embedded log4net.config file
    /// </summary>
    public class Logging
    {
        private static Logging instance;
        private static object instanceLock = new object();
        private ILog logger;

        private Logging()
        {
            Stream configStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("HaselOne.Handlers.log4net.config");
            XmlConfigurator.Configure(configStream);
            logger = LogManager.GetLogger("HaselOne ErrorHandling HttpModule");
        }

        public static Logging Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new Logging();
                        }
                    }
                }

                return instance;
            }
        }

        public void Error(string message, params string[] args)
        {
            logger.Error(string.Format(message, args));
        }
    }
}