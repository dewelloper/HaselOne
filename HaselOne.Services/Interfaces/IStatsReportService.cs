using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaselOne.Services.Interfaces
{
    public interface IStatsReportService
    {
        List<GetClassifiedMachineparkCounts_Result> GetClassifiedMachineparkCounts();

        List<GetSalesmanStats_Result> GetSalesmanStats(StatsFilter filter);

        List<GetAreaStats_Result> GetAreaStats(StatsFilter filter);

        List<GetSegmentStats_Result> GetSegmentStats(StatsFilter filter);

        List<TextValue> GetSalesmans(string keyword);

        IQueryable<CategoryWrapper> GetCategories();

        List<MachineparkCategoryWrapper> GetMachineparkCategories();

        List<MachineparkCategoryWrapper> GetMachineparkCategories(int categoryId);

        IQueryable<AreaWrapper> GetAreas();

        IQueryable<Cm_MachineparkMark> GetMarks();

        IQueryable<SegmentWrapper> GetSegments();
    }
}