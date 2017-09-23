using HaselOne.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HaselOne
{
    public abstract class ReportPageBase<TFilter> : Page
    {
        public TFilter Filter { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
                SetFilter();
            base.OnLoad(e);
        }

        protected virtual void SetFilter()
        {
            PageHelper.RegisterJs(this.Master, PageHelper.JsonConvert(Filter), "ReportFilter", "startup_scripts");
        }
    }
}