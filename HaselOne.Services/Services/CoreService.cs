using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BusinessObjects;

namespace HaselOne.Services.Services
{
    public class CoreService : ICoreService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Gn_Control> _controls;
        private readonly IGRepository<Gn_ControlAuthorities> _controlAuths;
        private readonly IGRepository<Gn_ModulsAndMenus> _userModuls;
        private readonly IGRepository<Cm_MachineparkCategory> _mpCategories;
        private readonly IGRepository<Gn_CategoryDetails> _categoryDetails;

        public CoreService(UnitOfWork uow)
        {
            _uow = uow;
            _controls = _uow.GetRepository<Gn_Control>();
            _controlAuths = _uow.GetRepository<Gn_ControlAuthorities>();
            _userModuls = _uow.GetRepository<Gn_ModulsAndMenus>();
            _mpCategories = _uow.GetRepository<Cm_MachineparkCategory>();
            _categoryDetails = _uow.GetRepository<Gn_CategoryDetails>();
        }

        public IQueryable<Gn_Control> GetControls(int userId, string pageName)
        {
            int? pageId = 0;
            Gn_ModulsAndMenus umm = _userModuls.Where(k => k.PageName == pageName).FirstOrDefault();
            if (umm != null)
                pageId = umm.PageId;

            List<Gn_Control> controls = _controls.Where(k => k.PageId == pageId).ToList();
            List<Gn_ControlAuthorities> auControls = _controlAuths.Where(k => k.UserId == userId).ToList();

            foreach (Gn_Control c in controls)
            {
                Gn_ControlAuthorities oc = auControls.Where(k => k.ControlId == c.Id).FirstOrDefault();
                if (oc != null)
                {
                    c.IsEnable = oc.IsEnable;
                    c.IsVisible = oc.IsVisible;
                }
            }

            return controls.AsQueryable();
        }

        public List<MachineparkCategoryWrapper> GetMachineparkCategories(int? categoryId = null)
        {
            List<MachineparkCategoryWrapper> res = null;
            IEnumerable<int?> catIds = null;
            var cats = _mpCategories.All().Select(_ => new MachineparkCategoryWrapper(_.Id, _.ParentId, _.CategoryName, _.OrderBy)).ToList();
            if (categoryId.HasValue)
            {
                catIds = _categoryDetails.Where(_ => _.CRGId == categoryId).Select(_ => _.CategoryId);
                cats = cats.Where(_ => catIds.Contains(_.Id)).ToList();
            }
            if (cats != null)
                res = new List<MachineparkCategoryWrapper>();
            foreach (var item in cats.Where(_ => _.ParentId == 0))
            {
                GetSubMpCategories(cats, item);
                res.Add(item);
            }
            int k = 0;
            foreach (MachineparkCategoryWrapper mpcw in res)
            {
                res[k++].Categories = mpcw.Categories.OrderBy(h => h.OrderBy).ToList();
            }   
            return res;
        }

        private void GetSubMpCategories(List<MachineparkCategoryWrapper> source, MachineparkCategoryWrapper main)
        {
            main.Categories = source.Where(_ => _.ParentId == main.Id).ToList();
            foreach (var sub in main.Categories)
            {
                GetSubMpCategories(source, sub);
            }
        }

        public bool SetCategoryByNodeId(int sourceId, int destId, int sourceIndex, int destIndex)
        {
            Cm_MachineparkCategory mpCatSource = _mpCategories.Where(k => k.Id == sourceId).FirstOrDefault();
            Cm_MachineparkCategory mpCatDest = _mpCategories.Where(k => k.Id == destId).FirstOrDefault();
            if (mpCatSource.ParentId == 0)
                return false;

            List<int?> crgIds = _categoryDetails.Where(k => k.CategoryId == mpCatDest.Id).Select(m => m.CRGId).ToList();
            List<int?> crgIds2 = _categoryDetails.Where(k => k.CategoryId == mpCatSource.Id).Select(m => m.CRGId).ToList();
            var inter = crgIds.Intersect(crgIds2);

            int route1 = GetMainParentId(mpCatSource);
            int route2 = GetMainParentId(mpCatDest);

            if (inter.Count() > 0 || route1 == route2)
            {
                mpCatSource.ParentId = mpCatDest.Id;
                mpCatSource.OrderBy = destIndex;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        private int GetMainParentId(Cm_MachineparkCategory mpCatDest)
        {
            int destMainParentId = 0;
            if (mpCatDest.ParentId == 0)
                destMainParentId = mpCatDest.Id;
            else
            {
                int dId = mpCatDest.ParentId;
                Cm_MachineparkCategory mpc = new Cm_MachineparkCategory();
                do
                {
                    mpc = _mpCategories.Where(k => k.Id == dId).FirstOrDefault();
                    if (mpc.ParentId == 0)
                        destMainParentId = mpc.Id;
                    dId = mpc.ParentId;
                } while (mpc.ParentId != 0);
            }

            return destMainParentId;
        }

        public bool AddNewCategory(string newCategoryName, int parentId)
        {
            if (parentId == 0)
                return false;

            _mpCategories.Insert(new Cm_MachineparkCategory()
            {
                CategoryName = newCategoryName,
                ParentId = parentId,
            });
            _uow.SaveChanges();
            return true;
        }

        public bool DeleteCategory(int desCategoryId)
        {
            if (desCategoryId == 0)
                return false;

            Cm_MachineparkCategory mpc = _mpCategories.Where(k => k.Id == desCategoryId).FirstOrDefault();
            _mpCategories.Delete(mpc);
            _uow.SaveChanges();
            return true;
        }
    }
}