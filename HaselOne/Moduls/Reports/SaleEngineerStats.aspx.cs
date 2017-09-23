using BusinessObjects;
using System;

namespace HaselOne
{
    public partial class SaleEngineerStats : ReportPageBase<StatsFilter>
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