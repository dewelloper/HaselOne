using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using BusinessObjects;
using HaselOne.Util;

namespace HaselOne.Controler
{
    public class CategoryManagementController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly ICoreService _cs;

        public CategoryManagementController(IUnitOfWork uow, ICoreService cs)
        {
            _uow = uow;
            _cs = cs;
        }

        [HttpPost]
        public ActionResult GetCategoriesAll()
        {
            try
            {
                var res = _cs.GetMachineparkCategories();

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult SetCategoryByNodeId(int source, int dest, int sourceIndex, int destIndex)
        {
            try
            {
                var res = _cs.SetCategoryByNodeId(source,dest,sourceIndex, destIndex);

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult AddNewCategory(string newCategoryName, int parentId)
        {
            try
            {
                var res = _cs.AddNewCategory(newCategoryName, parentId);

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int desCategoryId)
        {
            try
            {
                var res = _cs.DeleteCategory(desCategoryId);

                return Content(Result.Get(res));
            }
            catch (Exception ex)
            {
                return Content(Result.Get(false, ex.Message));
            }
        }

    }
}