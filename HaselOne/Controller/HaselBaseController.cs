using System.Collections.Generic;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Util;
using System.Text;
using System.Web.Mvc;
using BusinessObjects;
using DAL.Helper;

namespace HaselOne.Controler
{
    public abstract class HaselBaseController : Controller
    {
        protected readonly IUnitOfWork _uow;

        public readonly DictonaryStaticList dicStaticList = new DictonaryStaticList();

        protected CurrentUser CurrentUser { get; }
        protected List<TextValue> Validations { get; set; }

        public HaselBaseController(IUnitOfWork uow)
        {
            _uow = uow;
            Validations = new List<TextValue>(); ;
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            //throw new Exception("oneJson kullaniniz");
            return base.Json(data, contentType, contentEncoding);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _uow.Rollback();
            Logger.Log(filterContext);
            filterContext.Result = ResultService(isSuccess: false, message: "Hata olustu.");
            base.OnException(filterContext);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            // throw new Exception("oneJson kullaniniz");
            return base.Json(data, contentType, contentEncoding, behavior);
        }

        protected JsonResult oneJson(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, behavior);
        }

        public JsonResult oneJson(object data, string contentType, Encoding contentEncoding)
        {
            return base.Json(data, contentType, contentEncoding);
        }

        public ActionResult ResultService(object data)
        {
            return Content(Result.Get(true, "", data));
        }

        public ActionResult ResultService(bool isValid = true, bool isSuccess = true, string message = "", object objectData = null, List<TextValue> validationMessages = null, ResultType resultType = ResultType.Hide)
        {
            if (resultType == ResultType.Info && string.IsNullOrEmpty(message)) message = "İşlem başarılı";
            if (resultType == ResultType.Success && string.IsNullOrEmpty(message)) message = "İşlem başarılı";

            return Content(Result.Get(isValid, isSuccess, message, objectData, validationMessages, resultType));
        }
    }
}