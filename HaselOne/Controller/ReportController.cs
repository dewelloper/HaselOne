using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using BusinessObjects;
using HaselOne.Util;

namespace HaselOne.Controler
{
    [Authorize]
    public class ReportController : HaselBaseController
    {
        private readonly IStatsReportService _rs;

        public ReportController(IUnitOfWork uow, IStatsReportService rs) : base(uow)
        {
            _rs = rs;
        }

        [HttpPost]
        public ActionResult GetSalesmanStats(StatsFilter filter)
        {
            try
            {
                var res = _rs.GetSalesmanStats(filter);

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetAreaStats(StatsFilter filter)
        {
            try
            {
                var res = _rs.GetAreaStats(filter);

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetSegmentStats(StatsFilter filter)
        {
            try
            {
                var res = _rs.GetSegmentStats(filter);

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetClassifiedMachineparkCounts()
        {
            try
            {
                var res = _rs.GetClassifiedMachineparkCounts();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetCategories()
        {
            try
            {
                var res = _rs.GetCategories().ToList();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetSalesmans(string keyword)
        {
            try
            {
                var res = _rs.GetSalesmans(keyword).ToList();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetAreas()
        {
            try
            {
                var res = _rs.GetAreas().ToList();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetMachineparkCategories(int? categoryId = null)
        {
            try
            {
                var res = new List<MachineparkCategoryWrapper>();
                if (categoryId.HasValue)
                    res = _rs.GetMachineparkCategories(categoryId.Value).ToList();
                else
                    res = _rs.GetMachineparkCategories().ToList();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetMarks()
        {
            try
            {
                var res = _rs.GetMarks().Select(_ => new TextValue(_.Id, _.MarkName)).ToList();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult GetSegments()
        {
            try
            {
                var res = _rs.GetSegments().ToList();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }
    }
}