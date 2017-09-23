using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HaselOne.Moduls.Reports
{
    public partial class SegmentStats : ReportPageBase<StatsFilter>
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void SetFilter()
        {
            Filter = new StatsFilter();
            base.SetFilter();
        }
    }
}