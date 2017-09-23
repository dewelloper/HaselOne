using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace HaselOne.Handlers
{
    public class CustomPageHandlerFactory : PageHandlerFactory
    {
        private static object GetInstance(Type type)
        {
            // TODO: Get instance using your favorite DI library.
            // for instance using the Common Service Locator:
            return Microsoft.Practices.ServiceLocation
                .ServiceLocator.Current.GetInstance(type);
        }

        public override IHttpHandler GetHandler(HttpContext cxt,
            string type, string vPath, string path)
        {
            var page = base.GetHandler(cxt, type, vPath, path);

            if (page != null)
            {
                // Magic happens here ;-)
                InjectDependencies(page);
            }

            return page;
        }

        private static void InjectDependencies(object page)
        {
            Type pageType = page.GetType().BaseType;

            var ctor = GetInjectableCtor(pageType);

            if (ctor != null)
            {
                object[] arguments = (
                    from parameter in ctor.GetParameters()
                    select GetInstance(parameter.ParameterType)).ToArray();

                ctor.Invoke(page, arguments);
            }
        }

        private static ConstructorInfo GetInjectableCtor(
            Type type)
        {
            var overloadedPublicConstructors = (
                from constructor in type.GetConstructors()
                where constructor.GetParameters().Length > 0
                select constructor).ToArray();

            if (overloadedPublicConstructors.Length == 0)
            {
                return null;
            }

            if (overloadedPublicConstructors.Length == 1)
            {
                return overloadedPublicConstructors[0];
            }

            throw new Exception(string.Format(
                "The type {0} has multiple public " +
                "ctors and can't be initialized.", type));
        }
    }
}