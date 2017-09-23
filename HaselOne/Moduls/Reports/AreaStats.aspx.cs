using BusinessObjects;
using System;

namespace HaselOne.Moduls.Reports
{
    public partial class AreaStats : ReportPageBase<StatsFilter>
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