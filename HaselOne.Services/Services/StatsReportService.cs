using BusinessObjects;
using DAL;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DAL.Helper;

namespace HaselOne.Services.Services
{
    public class StatsReportService : ReportServiceBase, IStatsReportService
    {
        private readonly IGRepository<Gn_Category> _categories;
        private readonly IGRepository<Cm_MachineparkCategory> _machineParkCategories;
        private readonly IGRepository<Cm_MachineparkMark> _marks;
        private readonly IGRepository<Gn_Area> _areas;
        private readonly IGRepository<Gn_Segment> _segments;
        private readonly IGRepository<Gn_CategoryDetails> _catgoriDetails;

        public StatsReportService(UnitOfWork uow) : base(uow)
        {
            _categories = _uow.GetRepository<Gn_Category>();
            _machineParkCategories = _uow.GetRepository<Cm_MachineparkCategory>();
            _areas = _uow.GetRepository<Gn_Area>();
            _marks = _uow.GetRepository<Cm_MachineparkMark>();
            _catgoriDetails = _uow.GetRepository<Gn_CategoryDetails>();
            _segments = _uow.GetRepository<Gn_Segment>();
        }

        public IQueryable<CategoryWrapper> GetCategories()
        {
            return _categories.Where(_ => _.IsActive == true && _.IsDeleted == false).OrderBy(_ => _.Title).Select(_ => new CategoryWrapper(_.Id, _.Title, _.IsActive, _.IsDeleted)).AsQueryable();
        }

        public IQueryable<AreaWrapper> GetAreas()
        {
            return _areas.All().DistinctBy(k => new { k.MainAreaId, k.AreaName }).OrderBy(_ => _.AreaName).Select(_ => new AreaWrapper(_.Id, _.AreaName, _.MainAreaId)).AsQueryable();
        }

        public List<TextValue> GetSalesmans(string keyword)
        {
            var res = _uow.SqlQuery<TextValue>(@"SELECT TOP 500 Id As [Value], Name As [Text] FROM dbo.GetSalesmans({0}) ORDER BY [Name]", keyword).ToList();

            return res;
        }

        public List<MachineparkCategoryWrapper> GetMachineparkCategories()
        {
            //List<MachineparkCategoryWrapper> res = null;
            var cats = _machineParkCategories.All().Select(_ => new MachineparkCategoryWrapper(_.Id, _.ParentId, _.CategoryName,_.OrderBy)).ToList();
            //if (cats != null)
            //    res = new List<MachineparkCategoryWrapper>();
            //foreach (var item in cats.Where(_ => _.ParentId == 0))
            //{
            //    GetSubMpCategories(cats, item);
            //    res.Add(item);
            //}
            return cats;
        }

        public List<MachineparkCategoryWrapper> GetMachineparkCategories(int categoryId)
        {
            //List<MachineparkCategoryWrapper> res = null;
            var catIds = _catgoriDetails.Where(_ => _.CRGId == categoryId).Select(_ => _.CategoryId);
            var cats = _machineParkCategories.Where(_ => catIds.Contains(_.Id)).Select(_ => new MachineparkCategoryWrapper(_.Id, _.ParentId, _.CategoryName,_.OrderBy)).ToList();
            //if (cats != null)
            //    res = new List<MachineparkCategoryWrapper>();
            //foreach (var item in cats.Where(_ => _.ParentId == 0))
            //{
            //    GetSubMpCategories(cats, item);
            //    res.Add(item);
            //}
            return cats;
        }

        private void GetSubMpCategories(List<MachineparkCategoryWrapper> source, MachineparkCategoryWrapper main)
        {
            main.Categories = source.Where(_ => _.ParentId == main.Id).ToList();
            foreach (var sub in main.Categories)
            {
                GetSubMpCategories(source, sub);
            }
        }

        public IQueryable<Cm_MachineparkMark> GetMarks()
        {
            return _marks.Where(_ => _.IsActive == true && _.IsDeleted == false).OrderBy(k => k.MarkName).AsQueryable();
        }

        public IQueryable<SegmentWrapper> GetSegments()
        {
            return _segments.All().OrderBy(k => k.MinValue).Select(_ => new SegmentWrapper(_.Id, _.MinValue, _.MaxValue)).AsQueryable();
        }

        public List<GetClassifiedMachineparkCounts_Result> GetClassifiedMachineparkCounts()
        {
            var res = _uow.SqlQuery<GetClassifiedMachineparkCounts_Result>($"SELECT * FROM dbo.GetSegmentStats()").ToList();

            return res;
        }

        public List<GetSalesmanStats_Result> GetSalesmanStats(StatsFilter filter)
        {
            filter.FunctionParams = new string[] {
                filter.Category != null? filter.Category.Id.ToString() :"NULL",
                filter.Areas.Count > 0 ? string.Format("'{0}'", string.Join(",", filter.Areas.Select(_=>_.Id).ToArray())) : "NULL",
                filter.Salesmans.Count > 0? string.Format("'{0}'", string.Join(",",filter.Salesmans.Select(_=>_.Value).ToArray())) : "NULL",
                filter.MachineparkCategories.Count > 0 ?string.Format("'{0}'",string.Join(",", filter.MachineparkCategories.Select(_=>_.Id).ToArray())) : "NULL",
                filter.Marks.Count > 0 ? string.Format("'{0}'",string.Join(",",filter.Marks.Select(_=>_.Value).ToArray())) : "NULL",
                filter.Segments.Count > 0  ? string.Format("'{0}'",string.Join(",",filter.Segments.Select(_=>_.Id).ToArray())) : "NULL"
            };

            var res = _uow.SqlQuery<GetSalesmanStats_Result>($@"SELECT s.SalesmanId
	                                                                   ,s.SalesmanName
	                                                                   ,ISNULL(gs.CustomerCount,0) AS CustomerCount
	                                                                   ,ISNULL(gs.MachineParkCount,0) AS MachineParkCount
	                                                                   ,ISNULL(s.TotalCustomerCount,0) AS TotalCustomerCount
	                                                                   ,ISNULL(s.TotalMachinePark,0) AS TotalMachinePark
                                                                      FROM GetSalesmanStats(NULL, NULL, NULL, NULL, NULL, NULL) s
                                                                LEFT JOIN GetSalesmanStats({string.Join(",", filter.FunctionParams)}) gs ON gs.SalesmanId = s.SalesmanId").ToList();

            return res;
        }

        public List<GetAreaStats_Result> GetAreaStats(StatsFilter filter)
        {
            filter.FunctionParams = new string[] {
                filter.Category != null? filter.Category.Id.ToString() :"NULL",
                filter.Areas.Count > 0 ? string.Format("'{0}'", string.Join(",", filter.Areas.Select(_=>_.Id).ToArray())) : "NULL",
                filter.Salesmans.Count > 0? string.Format("'{0}'", string.Join(",",filter.Salesmans.Select(_=>_.Value).ToArray())) : "NULL",
                filter.MachineparkCategories.Count > 0 ?string.Format("'{0}'",string.Join(",", filter.MachineparkCategories.Select(_=>_.Id).ToArray())) : "NULL",
                filter.Marks.Count > 0 ? string.Format("'{0}'",string.Join(",",filter.Marks.Select(_=>_.Value).ToArray())) : "NULL",
                filter.Segments.Count > 0  ? string.Format("'{0}'",string.Join(",",filter.Segments.Select(_=>_.Id).ToArray())) : "NULL"
            };

            var res = _uow.SqlQuery<GetAreaStats_Result>($@"SELECT s.AreaId
	                                                              ,s.AreaName
	                                                              ,ISNULL(gs.CustomerCount, 0) AS CustomerCount
	                                                              ,ISNULL(gs.MachineParkCount, 0) AS MachineParkCount
	                                                              ,ISNULL(s.TotalCustomerCount, 0) AS TotalCustomerCount
	                                                              ,ISNULL(s.TotalMachinePark, 0) AS TotalMachinePark
                                                             FROM dbo.GetAreaStats(NULL, NULL, NULL, NULL, NULL, NULL) s
                                                             LEFT JOIN dbo.GetAreaStats({string.Join(",", filter.FunctionParams)}) gs ON gs.AreaId = s.AreaId").ToList();

            return res;
        }

        public List<GetSegmentStats_Result> GetSegmentStats(StatsFilter filter)
        {
            filter.FunctionParams = new string[] {
                filter.Category != null? filter.Category.Id.ToString() :"NULL",
                filter.Areas.Count > 0 ? string.Format("'{0}'", string.Join(",", filter.Areas.Select(_=>_.Id).ToArray())) : "NULL",
                filter.Salesmans.Count > 0? string.Format("'{0}'", string.Join(",",filter.Salesmans.Select(_=>_.Value).ToArray())) : "NULL",
                filter.MachineparkCategories.Count > 0 ?string.Format("'{0}'",string.Join(",", filter.MachineparkCategories.Select(_=>_.Id).ToArray())) : "NULL",
                filter.Marks.Count > 0 ? string.Format("'{0}'",string.Join(",",filter.Marks.Select(_=>_.Value).ToArray())) : "NULL",
            };

            var res = _uow.SqlQuery<GetSegmentStats_Result>($"SELECT * FROM dbo.GetSegmentStats({string.Join(",", filter.FunctionParams)})").ToList();

            return res;
        }
    }
}