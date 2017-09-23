using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.WebForms;

namespace HaselOne.App_Start
{
    public abstract class BaseService<T> : System.Web.Services.WebService where T : class
    {
        public BaseService()
        {
            InjectDependencies();
        }

        protected virtual void InjectDependencies()
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
                return;


            IUnityContainer container = Application.GetContainer();

            if (container == null)
                throw new InvalidOperationException("Container on Global Application Class is Null. Cannot perform BuildUp.");

            container.BuildUp(this as T);
        }
    }
}