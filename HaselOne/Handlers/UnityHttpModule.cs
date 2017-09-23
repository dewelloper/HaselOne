//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;

//namespace HaselOne.Handlers
//{
//    public class UnityHttpModule : IHttpModule
//    {
//        private const string NamespacePrefix = "DIWebFormsExample";

//        public void Init(HttpApplication context)
//        {
//            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
//        }

//        public void Dispose()
//        {
//            // Nothing to dispose
//        }

//        // Get the controls in the page's control tree excluding the page itself
//        private static IEnumerable GetControlTree(Control root)
//        {
//            foreach (Control child in root.Controls)
//            {
//                yield return child;
//                foreach (Control c in GetControlTree(child))
//                {
//                    yield return c;
//                }
//            }
//        }

//        private static void OnPreRequestHandlerExecute(object sender, EventArgs e)
//        {
//            /* Static content */
//            if (HttpContext.Current.Handler == null)
//            {
//                return;
//            }

//            var handler = HttpContext.Current.Handler;
//            Unity.Instance.Container.BuildUp(handler.GetType(), handler);

//            // User Controls are ready to be built up after the page initialization is complete
//            var page = HttpContext.Current.Handler as Page;
//            if (page != null)
//            {
//                page.InitComplete += OnPageInitComplete;
//            }
//        }

//        // Build up each control in the page's control tree
//        private static void OnPageInitComplete(object sender, EventArgs e)
//        {
//            var page = (Page)sender;
//            foreach (Control c in GetControlTree(page))
//            {
//                var typeFullName = c.GetType().FullName ?? string.Empty;
//                var baseTypeFullName = c.GetType().FullName ?? string.Empty;

//                // Filter on namespace to avoid build up of System.Web components
//                if (typeFullName.StartsWith(NamespacePrefix) ||
//                 baseTypeFullName.StartsWith(NamespacePrefix))
//                {
//                    Unity.Instance.Container.BuildUp(c.GetType(), c);
//                }
//            }
//        }
//    }
//}