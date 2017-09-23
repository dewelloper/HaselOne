using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace HaselOne.Moduls.HelpContents
{
    public partial class HelpForm : System.Web.UI.Page
    {

        [Dependency]
        public ICustomerService _cs { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadHelpContent();
            }
        }

        private void LoadHelpContent()
        {
            int modulId = Convert.ToInt32(Session["ModulId"]);
            int pageId = Convert.ToInt32(Session["PageId"]);
            var contentManagement = _cs.GetContentByArg(modulId, pageId);

            helpContainer.InnerHtml = contentManagement == null ? string.Empty : contentManagement.Content;
        }
    }
}