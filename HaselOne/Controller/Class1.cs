using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaselOne.Controler;
using HaselOne.Domain.UnitOfWork;

namespace HaselOne.Controller
{
    public class Model:HaselBaseController
    {
        public Model(IUnitOfWork uow) : base(uow)
        {
        }

        public ActionResult List(int modelId)
        {
            return null;
        }
    }
}