using BusinessObjects;
using DAL;
using DAL.Helper;
using DAL_Dochuman;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BusinessObjects.Base;

namespace HaselOne.Services.Services
{
    public class CustomerService : ServiceBase, ICustomerService
    {
        private readonly IGRepository<Cm_Customer> _customer;
        private readonly IGRepository<Cm_MachineparkCategory> _categories;
        private readonly IGRepository<Cm_MachineparkMark> _marks;
        private readonly IGRepository<Gn_Area> _areas;
        private readonly IGRepository<Gn_AreaCity> _regions;
        private readonly IGRepository<Cm_CustomerLocations> _locations;
        private readonly IGRepository<Cm_CustomerAuthenticators> _authenticators;
        private readonly IGRepository<Gn_User> _users;
        private readonly IGRepository<Gn_Category> _category;
        private readonly IGRepository<Cm_CustomerSalesmans> _salesmans;
        private readonly IGRepository<Cm_MachineparkYear> _years;
        private readonly IGRepository<Cm_MachineparkOwnership> _ownerships;
        private readonly IGRepository<Cm_CustomerMachineparks> _machineParks;
        private readonly IGRepository<Gn_CategoryDetails> _categoryDetails;
        private readonly IGRepository<Gn_DepartmentRoles> _departmentRules;
        private readonly IGRepository<Gn_UserRoles> _userRoles;
        private readonly IGRepository<Gn_Area> _oneAreas;
        private readonly IGRepository<Gn_RoleRelation> _roleRelations;
        private readonly IGRepository<Gn_Sector> _sectors;
        private readonly IGRepository<Cm_CustomerInterviews> _customerInterviews;
        private readonly IGRepository<Cm_Interview> _interview;
        private readonly IGRepository<Gn_InterviewImportant> _interviewImportant;
        private readonly IGRepository<Gn_ContentTypes> _contentTypes;
        private readonly IGRepository<Gn_ContentManagement> _contentManagement;
        private readonly IGRepository<Gn_ModulsAndMenus> _modulAndMenus;
        private readonly IGRepository<Gn_UserModuls> _userModuls;
        private readonly IGRepository<Gn_Notifications> _notifications;
        private readonly IGRepository<Cm_MachineparkMark> _MachineparkMark;
        private readonly IGRepository<Pr_MachineModel> _machineModel;
        private readonly IGRepository<Cm_CustomerRequest> _CustomerRequest;
        private readonly IGRepository<Gn_Combos> _combos;

        private DCHEntities _dContext = new DCHEntities();

        public CustomerService(UnitOfWork uow) : base(uow)
        {
            _customer = _uow.GetRepository<Cm_Customer>();
            _categories = _uow.GetRepository<Cm_MachineparkCategory>();
            _sectors = _uow.GetRepository<Gn_Sector>();
            _marks = _uow.GetRepository<Cm_MachineparkMark>();
            _areas = _uow.GetRepository<Gn_Area>();
            _regions = _uow.GetRepository<Gn_AreaCity>();
            _locations = _uow.GetRepository<Cm_CustomerLocations>();
            _authenticators = _uow.GetRepository<Cm_CustomerAuthenticators>();
            _users = _uow.GetRepository<Gn_User>();
            _category = _uow.GetRepository<Gn_Category>();
            _salesmans = _uow.GetRepository<Cm_CustomerSalesmans>();
            _years = _uow.GetRepository<Cm_MachineparkYear>();
            _ownerships = _uow.GetRepository<Cm_MachineparkOwnership>();
            _machineParks = _uow.GetRepository<Cm_CustomerMachineparks>();
            _categoryDetails = _uow.GetRepository<Gn_CategoryDetails>();
            _departmentRules = _uow.GetRepository<Gn_DepartmentRoles>();
            _userRoles = _uow.GetRepository<Gn_UserRoles>();
            _oneAreas = _uow.GetRepository<Gn_Area>();
            _roleRelations = _uow.GetRepository<Gn_RoleRelation>();
            _customerInterviews = _uow.GetRepository<Cm_CustomerInterviews>();
            _interview = _uow.GetRepository<Cm_Interview>();
            _contentTypes = _uow.GetRepository<Gn_ContentTypes>();
            _contentManagement = _uow.GetRepository<Gn_ContentManagement>();
            _modulAndMenus = _uow.GetRepository<Gn_ModulsAndMenus>();
            _userModuls = _uow.GetRepository<Gn_UserModuls>();
            _notifications = _uow.GetRepository<Gn_Notifications>();
            _interviewImportant = _uow.GetRepository<Gn_InterviewImportant>();
            _MachineparkMark = _uow.GetRepository<Cm_MachineparkMark>();
            _machineModel = _uow.GetRepository<Pr_MachineModel>();
            _CustomerRequest = _uow.GetRepository<Cm_CustomerRequest>();
            _combos = _uow.GetRepository<Gn_Combos>();
        }

        public IQueryable<Cm_Customer> GetCustomers()
        {
            return _customer.All().AsQueryable();
        }

        public IQueryable<Cm_Customer> GetCustomersWhere(string criteria)
        {
            criteria = criteria.ToUpper();
            return
                _customer.Where(
                    k =>
                        k.Name.Contains(criteria) || k.NetsisHaselCode.Contains(criteria) ||
                        k.NetsisRentliftCode.Contains(criteria)).AsQueryable();
        }

        public IQueryable<Cm_Customer> GetCustomersWhere(string criteria, int pageSkip, int pageSize)
        {
            criteria = criteria.ToUpper();
            IQueryable<Cm_Customer> cariler =
                _customer.Where(
                    k =>
                        k.Name.Contains(criteria) || k.NetsisHaselCode.Contains(criteria) ||
                        k.NetsisRentliftCode.Contains(criteria)).AsQueryable();
            return cariler.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).AsQueryable();
        }

        public IQueryable<Cm_Customer> GetCustomersLast20()
        {
            return
                _customer.Where(m => m.StatusId == 0)
                    .OrderByDescending(k => k.Id)
                    .Take(20)
                    .AsQueryable();
        }

        public IQueryable<CustomerWrapper> GetCustomers(CustomerFilter filter)
        {
            IQueryable<CustomerWrapper> custs = _uow.SqlQuery<CustomerWrapper>(@" DECLARE @UserId INT = {0}
                                                       EXEC Cm_GetCustomersByUserId @UserId", filter.UserId).AsQueryable();
            //IQueryable<Cm_Customer> custs = customerQuery.IncludeMultiple<Cm_Customer>(k => k.Cm_CustomerLocations,
            //                                       k => k.Cm_CustomerSalesmans,
            //                                       k => k.Cm_CustomerMachineparks,
            //                                       k => k.Cm_CustomerAuthenticators,
            //                                       k => k.Cm_CustomerInterviews,
            //                                       k => k.Cm_CustomerRequest,
            //                                       k => k.Cm_MachineparkRental).AsQueryable();

            //IQueryable<Cm_Customer> cls = _customer.Where(k => k.Id == 1072062).AsQueryable();
            //IQueryable<Cm_Customer> cmcc = cls.IncludeMultiple(k => k.Cm_CustomerLocations);

            // List<Int32> cids = customerQuery.Select(k => k.Id).ToList();
            //IQueryable<Cm_Customer> custs = _customer.Where(k => cids.Contains(k.Id)).AsQueryable();

            if (filter.Id == null || filter.Id == 1)  //   1-Tüm Cariler
            {
                custs = custs.OrderByDescending(k => k.Id).AsQueryable();
            }
            if (filter.Id == 2) //   2-Onaysız Cariler
            {
                custs = custs.Where(m => m.StatusId == 0).ToList().OrderByDescending(k => k.Id).AsQueryable();
            }

            //if (filter.Id == 3)  //   3-Tamamlanmayan Cariler
            //{
            //    List<int> cIds = custs.Select(k => k.Id).ToList();
            //    List<int> ulIds = _locations.All().Select(k => (int)k.CustomerId).Distinct().ToList();
            //    List<int> aIds = _authenticators.All().Select(k => (int)k.CustomerId).Distinct().ToList();
            //    List<int> sIds = _salesmans.All().Select(k => (int)k.CustomerId).Distinct().ToList();
            //    List<int> mIds = _machineParks.All().Select(k => (int)k.CustomerId).Distinct().ToList();
            //    List<int> iIds = _customerInterviews.All().Select(k => (int)k.CustomerId).Distinct().ToList();
            //    List<int> rIds = _CustomerRequest.All().Select(k => (int)k.CustomerId).Distinct().ToList();

            //    List<int> unlList = cIds.Except(ulIds).ToList();
            //    List<int> unaList = cIds.Except(aIds).ToList();
            //    List<int> unsList = cIds.Except(sIds).ToList();
            //    List<int> unmList = cIds.Except(mIds).ToList();
            //    List<int> uniList = cIds.Except(iIds).ToList();
            //    List<int> unrList = cIds.Except(rIds).ToList();

            //    List<int> allUn = unlList;
            //    allUn.AddRange(unaList);
            //    allUn.AddRange(unsList);
            //    allUn.AddRange(unmList);
            //    allUn.AddRange(uniList);
            //    allUn.AddRange(unrList);

            //    custs = custs.Where(k => allUn.Contains(k.Id)).AsQueryable();
            //}

            if (filter.Id == 4) //4-Lokasyonu Olmayan Cariler
            {
                List<int> cIds = custs.Select(k => k.Id).ToList();
                List<int> ulIds = _locations.All().Select(k => (int)k.CustomerId).Distinct().ToList();
                List<int> unlList = cIds.Except(ulIds).ToList();
                custs = custs.Where(k => unlList.Contains(k.Id)).AsQueryable();
                //List<int> all = custs.Where(k => !k.Cm_CustomerLocations.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                //custs = custs.Where(k => all.Contains(k.Id)).AsQueryable();
            }

            if (filter.Id == 5) //5-Makinesi  Olmayan Cariler
            {
                List<int> cIds = custs.Select(k => k.Id).ToList();
                List<int> mIds = _machineParks.All().Select(k => (int)k.CustomerId).Distinct().ToList();
                List<int> unmList = cIds.Except(mIds).ToList();
                custs = custs.Where(k => unmList.Contains(k.Id)).AsQueryable();
                //List<int> all = custs.Where(k => !k.Cm_CustomerMachineparks.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                //custs = custs.Where(k => all.Contains(k.Id)).AsQueryable();
            }

            if (filter.Id == 6)//6-Satıcısı Olmayan Cariler
            {
                List<int> cIds = custs.Select(k => k.Id).ToList();
                List<int> sIds = _salesmans.All().Select(k => (int)k.CustomerId).Distinct().ToList();
                List<int> unsList = cIds.Except(sIds).ToList();
                custs = custs.Where(k => unsList.Contains(k.Id)).AsQueryable();
                //List<int> all = custs.Where(k => !k.Cm_CustomerSalesmans.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                //custs = custs.Where(k => all.Contains(k.Id)).AsQueryable();
            }

            if (filter.Id == 7) //7-Yetkilisi Olmayan Cariler
            {
                List<int> cIds = custs.Select(k => k.Id).ToList();
                List<int> aIds = _authenticators.All().Select(k => (int)k.CustomerId).Distinct().ToList();
                List<int> unaList = cIds.Except(aIds).ToList();
                custs = custs.Where(k => unaList.Contains(k.Id)).AsQueryable();
                //List<int> all = custs.Where(k => !k.Cm_CustomerAuthenticators.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                //custs = custs.Where(k => all.Contains(k.Id)).AsQueryable();
            }

            if (filter.Id == 8) //7-Görüşmesi Olmayan Cariler
            {
                List<int> cIds = custs.Select(k => k.Id).ToList();
                List<int> iIds = _customerInterviews.All().Select(k => (int)k.CustomerId).Distinct().ToList();
                List<int> uniList = cIds.Except(iIds).ToList();
                custs = custs.Where(k => uniList.Contains(k.Id)).AsQueryable();
                //List<int> all = custs.Where(k => !k.Cm_CustomerInterviews.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                //custs = custs.Where(k => all.Contains(k.Id)).AsQueryable();
            }

            if (filter.Id == 9) //7-Talebi Olmayan Cariler
            {
                List<int> cIds = custs.Select(k => k.Id).ToList();
                List<int> rIds = _CustomerRequest.All().Select(k => (int)k.CustomerId).Distinct().ToList();
                List<int> unrList = cIds.Except(rIds).ToList();
                custs = custs.Where(k => unrList.Contains(k.Id)).AsQueryable();
                //List<int> all = custs.Where(k => !k.Cm_CustomerRequest.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                //custs = custs.Where(k => all.Contains(k.Id)).AsQueryable();
            }

            IQueryable<CustomerWrapper> cs = custs.Where(m => m.IsDeleted != true).OrderByDescending(m => m.Id).ToList().AsQueryable();
            return cs;
        }

        public List<CustomerWrapper> GetCustomersLast202(int page, int pageSize, string sortIndex, string sortDirection)
        {
            List<Cm_Customer> custs =
                _customer.Where(m => m.StatusId == 0).OrderByDescending(k => k.Id).Take(20).ToList();
            List<CustomerWrapper> cr = new List<CustomerWrapper>();
            foreach (Cm_Customer c in custs)
                cr.Add(new CustomerWrapper()
                {
                    Id = c.Id,
                    NetsisHaselCode = c.NetsisHaselCode,
                    NetsisRentliftCode = c.NetsisRentliftCode
                });
            return cr.ToList();
        }

        public IQueryable<Cm_MachineparkCategory> GetCategories()
        {
            return _categories.All().AsQueryable();
        }

        public IQueryable<Cm_MachineparkMark> GetListMachineparkMark()
        {
            return _MachineparkMark.All().AsQueryable();
        }

        public IQueryable<Pr_MachineModel> GetListMachineModel()
        {
            return _machineModel.All().AsQueryable();
        }

        public IQueryable<Gn_Category> GetCategoryRightGroups()
        {
            return _category.All().AsQueryable();
        }

        public IQueryable<Gn_Sector> GetSectors()
        {
            return _sectors.All().AsQueryable();
        }

        public Int32 InsertCustomer(Cm_Customer cari, int uid)
        {
            //Gn_User user = _users.Where(k => k.Id == uid).FirstOrDefault();
            cari.Name = cari.Name.ToUpper();
            cari.NetsisHaselCode = cari.NetsisHaselCode.ToUpper();
            Int32 cid = _customer.Insert(cari);

            #region HSL_CARI #DFS -> insert

            HSL_CARI hcari = new HSL_CARI()
            {
                HSL_CARIKOD = cari.NetsisHaselCode != "" ? cari.NetsisHaselCode : cari.NetsisHaselCode,
                HSL_VD = cari.TaxOffice,
                HSL_VN = cari.TaxNumber,
                HSL_CARIISIM = cari.Name,
                HSL_KISALTMA = cari.ShortName,
                HSL_FIRMA = cari.IsHasel == true ? "HASEL" : "RENTLIFT",
                HSL_CARIKODO = cari.NetsisRentliftCode,
                HSL_CARIKODH = cari.NetsisHaselCode,
                HSL_SEKTORID = cari.SectorId,
                HSL_WEB = cari.Web,
                HSL_DURUM = cari.StatusId == 1 ? "Onaylı" : "",
                CreateDate = DateTime.Now,
                CreateUser = GetDFSUserSet_Id(uid),
                IsDeleted = false,
                OneId = cid
            };
            _dContext.HSL_CARI.Add(hcari);
            _dContext.SaveChanges();

            #endregion HSL_CARI #DFS -> insert

            List<int?> drIds = _userRoles.Where(k => k.UserId == uid).Select(m => m.DepartmentRuleId).ToList();
            List<int?> roleIds = _departmentRules.Where(k => drIds.Contains(k.Id)).Select(m => m.RuleId).ToList();
            if (roleIds.Contains(2))
            //2=> is salesman role /// @BilalBey: Bölge Müdürü diye bir kavram yok aslında sadece satıcı var.
            {
                _salesmans.Insert(new Cm_CustomerSalesmans()
                {
                    CreateDate = DateTime.Now,
                    CreatorName = uid.ToString(),
                    CustomerId = cid,
                    Flag = true,
                    IsActive = true,
                    IsDeleted = false,
                    SalesmanId = uid,
                    CRGID = 1
                });
            }
            return cid;
        }

        public bool SaveCustomer(Cm_Customer cari, int uid)
        {
            cari.NetsisHaselCode = cari.NetsisHaselCode.ToUpper();
            cari.Name = cari.Name.ToUpper();
            _customer.Update(cari);

            #region HSL_CARI #DFS -> update

            HSL_CARI hcari = _dContext.HSL_CARI.Where(k => k.OneId == cari.Id).FirstOrDefault();
            if (hcari != null)
            {
                hcari.HSL_CARIKOD = cari.NetsisHaselCode != "" ? cari.NetsisHaselCode : cari.NetsisHaselCode;
                hcari.HSL_VD = cari.TaxOffice;
                hcari.HSL_VN = cari.TaxNumber;
                hcari.HSL_CARIISIM = cari.Name;
                hcari.HSL_KISALTMA = cari.ShortName;
                hcari.HSL_FIRMA = cari.IsHasel == true ? "HASEL" : "RENTLIFT";
                hcari.HSL_CARIKODO = cari.NetsisRentliftCode;
                hcari.HSL_CARIKODH = cari.NetsisHaselCode;
                //hcari.HSL_SEKTORID = cari.SectorId;
                hcari.HSL_WEB = cari.Web;

                hcari.HSL_DURUM = cari.StatusId == 1 ? "Onaylı" : "";
                hcari.UpdateDate = DateTime.Now;
                hcari.UpdateUser = GetDFSUserSet_Id(uid);
                hcari.IsDeleted = Convert.ToBoolean(cari.IsDeleted);
            }

            #endregion HSL_CARI #DFS -> update

            _dContext.SaveChanges();
            return true;
        }

        public Cm_Customer GetCustomerById(int customerId)
        {
            return _customer.Where(k => k.Id == customerId && k.IsDeleted != true).FirstOrDefault();
        }

        public List<Cm_MachineparkMark> GetMarks()
        {
            return _marks.All().OrderBy(k => k.MarkName).ToList();
        }

        public IQueryable<Gn_User> GetSaleEngineersByCustomerId(int cid)
        {
            List<int?> cuids =
                _salesmans.Where(k => k.CustomerId == cid && k.IsDeleted == false && k.IsActive == true)
                    .Select(m => m.SalesmanId)
                    .ToList();
            return _users.Where(k => cuids.Contains(k.Id)).AsQueryable();
        }

        public List<Gn_Area> GetAreas()
        {
            return _areas.All().DistinctBy(k => new { k.MainAreaId, k.AreaName }).ToList();
        }

        public List<Gn_AreaCity> GetAreasForCitiesAll()
        {
            //KOD => Id
            return _regions.All().DistinctBy(k => new { k.Id, k.CityName }).ToList();
        }

        public List<Gn_AreaCity> GetCitiesAll()
        {
            return _regions.All().DistinctBy(k => new { k.Id, k.CityName }).ToList();
        }

        public List<Gn_AreaCity> GetCities(string kod)
        {
            int kd = Convert.ToInt32(kod);
            return _regions.Where(m => m.Id == kd).DistinctBy(k => new { k.Id, k.CityName }).ToList();
        }

        public List<Gn_AreaCity> GetRegions(string ilkod)
        {
            int kd = Convert.ToInt32(ilkod);
            return _regions.Where(m => m.Id == kd).ToList();
        }

        public List<Gn_AreaCity> GetRegionsByCityName(string ilname)
        {
            Gn_AreaCity ililce = _regions.Where(m => m.CityName == ilname.ToUpper()).FirstOrDefault();
            if (ililce != null)
            {
                return _regions.Where(k => k.Id == ililce.Id).ToList();
            }
            return null;
        }

        public List<Gn_AreaCityRegions> GetDistrictByCityName(string ilname)
        {
            if (ilname.ToLower().Contains("seçiniz"))
            {
                return new List<Gn_AreaCityRegions>();
            }
            ilname = ilname.ToUpper();
            var ililce = _regions.Where(m => m.CityName == ilname).ToList();
            if (ililce.Count == 0)
            {
                return new List<Gn_AreaCityRegions>();
            }
            var result = from o in ililce[0].Gn_AreaCityRegions
                         select new Gn_AreaCityRegions()
                         {
                             RegionName = o.RegionName,
                             CityId = o.CityId,
                             Id = o.Id
                         };
            return result.ToList();
        }

        public Cm_CustomerLocations GetLocationById(int locId)
        {
            return _locations.Where(k => k.Id == locId).FirstOrDefault();
        }

        public List<Cm_CustomerLocations> GetLocationByCustomerId(int customerId)
        {
            return _locations.Where(k => k.CustomerId == customerId && k.IsDeleted != true).ToList();
        }

        public List<Cm_CustomerLocations> GetLocationBy(LocationFilter filter)
        {
            bool state = false;
            IQueryable<Cm_CustomerLocations> query = _locations.WhereQuery(m => m.IsDeleted == filter.IsDelete);
            if (filter != null)
            {
                if (filter.CustomerId != null)
                {
                    state = true;
                    query = query.Where(m => m.CustomerId == filter.CustomerId);
                }
                if (filter.Id != null)
                {
                    state = true;
                    query = query.Where(m => m.Id == filter.Id);
                }
                if (!String.IsNullOrEmpty(filter.Name))
                {
                    state = true;
                    query = query.Where(m => m.Name.Contains(filter.Name));
                }
            }
            if (state)
                return query.ToList();

            return new List<Cm_CustomerLocations>();
        }

        public List<Cm_CustomerAuthenticators> GetAuthenticatorByCustomerId(int customerId)
        {
            return _authenticators.Where(k => k.CustomerId == customerId && k.IsDeleted != true).ToList();
        }

        public IQueryable<SalesmanWraper> GetSalesmanById(int customerId)
        {
            List<SalesmanWraper> sws = new List<SalesmanWraper>();
            IQueryable<Cm_CustomerSalesmans> ses =
                _salesmans.Where(k => k.CustomerId == customerId && k.IsDeleted != true).AsQueryable();
            foreach (Cm_CustomerSalesmans sm in ses)
            {
                Gn_Category smt = _category.Where(k => k.Id == sm.CRGID).FirstOrDefault();
                SalesmanWraper sw = new SalesmanWraper();
                Gn_User u = _users.Where(k => k.Id == sm.SalesmanId).FirstOrDefault();
                Gn_Area area = _areas.Where(k => k.Id == u.AreaId).FirstOrDefault();
                if (u != null)
                {
                    sw.Name = (u.Name == null ? "" : u.Name) + " " + (u.Surname == null ? "" : u.Surname)
                              + "  (" + (area == null ? "" : area.AreaName) + ")";
                }

                sw.Type = smt.Title;
                sw.Id = sm.Id;
                sw.IsActive = sm.IsActive;
                sw.IsDeleted = sm.IsDeleted;
                sw.SalesmanId = sm.SalesmanId;
                sw.SalesmanTypeId = sm.CRGID;
                sw.Flag = sm.Flag;
                sws.Add(sw);
            }

            return sws.AsQueryable();
        }

        public Gn_User HasAreaDirector(int customerId)
        {
            List<int?> ocsmIds = _salesmans.Where(k => k.CustomerId == customerId).Select(m => m.SalesmanId).ToList();
            //Gn_User areaDir = _users.Where(k => k.IsAreaDirector == true && ocsmIds.Contains(k.Id)).FirstOrDefault();
            Gn_User areaDir = _users.Where(k => ocsmIds.Contains(k.Id)).FirstOrDefault();
            return areaDir;
        }

        public bool UpdateCustomerCoLocation(int locId, string Longitude, string Latitude)
        {
            Cm_CustomerLocations location = _locations.Where(k => k.Id == locId).FirstOrDefault();
            if (location != null)
            {
                location.Longitude = Longitude;
                location.Latitude = Latitude;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Cm_CustomerLocations SaveNewLocation(int customerId, string location, string region, string address,
            string shordtdef, string tel, string fax, string longitude, string latitude, bool faturakesebilirmi, int uid,
            bool isLocActive, bool isLocDeleted, bool workingmode)
        {
            try
            {
                location = location.ToUpper();
                region = region.ToUpper();
                address = address.ToUpper();
                shordtdef = shordtdef.ToUpper();

                if (workingmode == false)
                {
                    Cm_CustomerLocations ll = _locations.Where(k => k.Id == customerId).FirstOrDefault();
                    ll.CityName = location;
                    ll.RegionName = region;
                    ll.Address = address;
                    ll.Name = shordtdef;
                    ll.Phone = tel;
                    ll.Fax = fax;
                    ll.Longitude = longitude;
                    ll.Latitude = latitude;
                    ll.ModifierId = uid;
                    ll.ModifyDate = DateTime.Now;
                    ll.IsDeleted = isLocDeleted;
                    _uow.SaveChanges();

                    Cm_CustomerLocations hloc = new Cm_CustomerLocations();
                    hloc.Id = ll.Id;
                    hloc.CityName = location;
                    hloc.RegionName = region;
                    hloc.Address = address;
                    hloc.Name = shordtdef;
                    hloc.Phone = tel;
                    hloc.Fax = fax;
                    hloc.Longitude = longitude;
                    hloc.Latitude = latitude;
                    hloc.IsFat = ll.IsFat;
                    hloc.ModifierId = uid;
                    hloc.ModifyDate = DateTime.Now;
                    hloc.IsDeleted = false;

                    #region #DFS HSL_LOKASYON -> update Location

                    HSL_LOKASYON hlck = _dContext.HSL_LOKASYON.Where(k => k.OneId == ll.Id).FirstOrDefault();
                    if (hlck != null)
                    {
                        hlck.LOK_ADI = hloc.Name;
                        hlck.LOK_ADRES = hloc.Address;
                        hlck.LOK_FAT = hloc.IsFat;
                        hlck.LOK_FAX = hloc.Fax;
                        hlck.LOK_IL = hloc.CityName;
                        hlck.LOK_ILCE = hloc.RegionName;
                        hlck.LOK_TEL = hloc.Phone;
                        hlck.UpdateDate = DateTime.Now;
                        hlck.UpdateUser = GetDFSUserSet_Id(uid);
                    }

                    #endregion #DFS HSL_LOKASYON -> update Location

                    _dContext.SaveChanges();
                    return hloc;
                }

                Cm_CustomerLocations l = new Cm_CustomerLocations()
                {
                    CityName = location,
                    RegionName = region,
                    Address = address,
                    Name = shordtdef,
                    Phone = tel,
                    Fax = fax,
                    Longitude = longitude,
                    Latitude = latitude,
                    IsFat = faturakesebilirmi,
                    CustomerId = customerId,
                    CreatorId = uid,
                    CreateDate = DateTime.Now,
                    IsDeleted = isLocDeleted
                };
                _locations.Insert(l);
                _uow.SaveChanges();

                #region #DFS HSL_LOKASYON -> Insert Location

                HSL_CARI hcari = _dContext.HSL_CARI.Where(k => k.OneId == customerId).FirstOrDefault();
                if (hcari != null)
                {
                    HSL_LOKASYON hlocation = new HSL_LOKASYON()
                    {
                        LOK_ADI = l.Name,
                        LOK_ADRES = l.Address,
                        LOK_FAT = l.IsFat,
                        LOK_FAX = l.Fax,
                        LOK_IL = l.CityName,
                        LOK_ILCE = l.RegionName,
                        LOK_TEL = l.Phone,
                        CreateDate = DateTime.Now,
                        CreateUser = GetDFSUserSet_Id(uid),
                        MasterId = hcari.Id,
                        OneId = l.Id
                    };
                    _dContext.HSL_LOKASYON.Add(hlocation);
                    _dContext.SaveChanges();
                }

                #endregion #DFS HSL_LOKASYON -> Insert Location

                return l;
            }
            catch (Exception ex)
            {
                string innerMes = "";
                if (ex.InnerException != null)
                    innerMes = ex.InnerException.Message;
                Cm_CustomerLocations errHLoc = new Cm_CustomerLocations()
                {
                    Name = ex.Message + " innerExxception: " + innerMes
                };
                return errHLoc;
            }
        }

        public object LocationDelete(int locationIdTo)
        {
            try
            {
                Cm_CustomerLocations loc = _locations.Where(k => k.Id == locationIdTo).FirstOrDefault();
                if (loc.Name.Trim().ToUpper() == "MERKEZ")
                {
                    int merkLokCount = _locations.Where(k => k.Name.ToUpper() == "MERKEZ" && k.CustomerId == loc.CustomerId &&
                                 k.IsDeleted != true).Count();
                    if (merkLokCount <= 1)
                    {
                        return "Merkez lokasyonu enaz 1 tane olmak zorundadır. Merkez lokasyonunu değiştirmek istiyorsanız öncelikle başka bir merkez lokasyon belirlemeniz gerekmektedir...";
                    }
                }
                if (IsUseLocationForAuth(locationIdTo))
                {
                    return "Bu lokasyon yetkili tanimlamada kullanilmistir. Ilk once yetkilisini siliniz.";
                }
                if (IsUseLocationForMP(locationIdTo))
                {
                    return "Bu lokasyon makina parki tanimlamada kullanilmistir. Ilk once makine parkini siliniz.";
                }
                if (loc != null)
                {
                    loc.IsDeleted = true;
                    _uow.SaveChanges();

                    #region #DFS Location delete

                    HSL_LOKASYON hl = _dContext.HSL_LOKASYON.Where(k => k.OneId == loc.Id).FirstOrDefault();
                    if (hl != null)
                    {
                        hl.IsDeleted = true;
                        hl.OneId = loc.Id;
                        _dContext.SaveChanges();
                    }

                    #endregion #DFS Location delete

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsUseLocationForAuth(int locationIdTo)
        {
            var item = _authenticators.Where(m => m.CustomerLocationId == locationIdTo && m.IsDeleted.Value == false).Count();
            if (item > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsUseLocationForMP(int locationIdTo)
        {
            var item = _machineParks.Where(m => m.LocationId == locationIdTo && m.IsDeleted == false).Count();
            if (item > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object SalesmanDelete(int salesmanId, int customerId, string operationType)
        {
            //int? salesmanUserId = _salesmans.Where(k => k.Id == salesmanId).Select(m => m.SalesmanId).FirstOrDefault();
            Gn_Category crg = _category.Where(k => k.Title == operationType).FirstOrDefault();
            bool isOk = true;

            if (crg != null) // if operationtype is other then crg will be null and validation will remove
            {
                var x = IsMachineparkSalesmanControlOk(Convert.ToInt32(crg.Id), customerId, salesmanId);
                if (Convert.ToInt32(x) > 0)
                    isOk = false;
            }

            Cm_CustomerSalesmans sm = _salesmans.Where(k => k.Id == salesmanId).FirstOrDefault();
            int sSameTypeCount = _salesmans.Where(k => k.CustomerId == customerId && k.IsDeleted != true && k.CRGID == sm.CRGID).Count();

            if (sSameTypeCount < 2 && !isOk)
                return "Dikkat! Bu kategoride daha önce makine parkı eklediğiniz için bu satıcıyı silemezsiniz...";

            int sCount = _salesmans.Where(k => k.CustomerId == customerId && k.IsDeleted != true).Count();
            if (sCount == 1)
                return "Dikkat! Bu kategoride tek satıcı kaldığı için silme işlemi yapılamaz...";

            if (sm != null)
            {
                sm.IsDeleted = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AuthDelete(int authenticatorId)
        {
            Cm_CustomerAuthenticators yetkili = _authenticators.Where(k => k.Id == authenticatorId).FirstOrDefault();
            if (yetkili != null)
            {
                yetkili.IsDeleted = true;
                _uow.SaveChanges();

                #region #DFS delete authenticator

                HSL_YETKILI hyet = _dContext.HSL_YETKILI.Where(k => k.OneId == authenticatorId).FirstOrDefault();
                if (hyet != null)
                {
                    hyet.IsDeleted = true;
                    hyet.OneId = yetkili.Id;
                    _dContext.SaveChanges();
                }

                #endregion #DFS delete authenticator

                return true;
            }
            return false;
        }

        public bool ChangeLocation(int locId)
        {
            Cm_CustomerLocations location = _locations.Where(k => k.Id == locId).FirstOrDefault();
            if (location != null)
            {
                bool targetStatus = !Convert.ToBoolean(location.IsFat);
                List<Cm_CustomerLocations> allCustomerLocations =
                    _locations.Where(k => k.CustomerId == location.CustomerId).ToList();
                foreach (Cm_CustomerLocations lo in allCustomerLocations)
                    lo.IsFat = false;
                location.IsFat = targetStatus;
                _uow.SaveChanges();
                FDSChangeLocation(locId);
                return targetStatus;
            }

            return false;
        }

        public string TaxNumberValid(string str, int customerId)
        {
            if (String.IsNullOrEmpty(str.Trim()))
            {
                return "Vergi numarası boş.";
            }

            return _customer.Where(m => m.TaxNumber == str.Trim() && m.Id != customerId && m.IsDeleted != true).Any() ? "Bu vergi numarası başka bir cariye aittir" : "";
        }

        private void FDSChangeLocation(int locId)
        {
            var location = _dContext.HSL_LOKASYON.FirstOrDefault(k => k.OneId == locId);
            if (location != null)
            {
                bool targetStatus = !Convert.ToBoolean(location.LOK_FAT);

                var allCustomerLocations =
                    _dContext.HSL_LOKASYON.Where(k => k.MasterId == location.MasterId).ToList();

                foreach (var lo in allCustomerLocations)
                    lo.LOK_FAT = false;

                location.LOK_FAT = targetStatus;
                _dContext.SaveChanges();
                // return targetStatus;
            }
        }

        private int SalesmanCopyInsert(int salesmanId)
        {
            var item = _salesmans.Where(m => m.Id == salesmanId).FirstOrDefault();
            if (item != null)
            {
                try
                {
                    item.CreateDate = DateTime.Now;
                    var temp = _salesmans.Insert(item);
                    _uow.SaveChanges();

                    SalesmanrowDelete(salesmanId);

                    return temp;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return -1;
        }

        private void SalesmanrowDelete(int salesmanId)
        {
            var lastItem = _salesmans.Where(m => m.Id == salesmanId).FirstOrDefault();
            lastItem.IsDeleted = true;
            lastItem.ModifyDate = DateTime.Now;
            _salesmans.Update(lastItem); //eski kayit disable cikilir
            _uow.SaveChanges();
        }

        public FlagDto ChangeSalesmanFlag(int salesmanId, int flagTypeId)
        {
            //her bayrak degisiminde yeni insert olucak.
            salesmanId = SalesmanCopyInsert(salesmanId);
            var flagDto = new FlagDto()
            {
                CmCustomerSalesmanId = salesmanId
            };

            Cm_CustomerSalesmans salesMan = _salesmans.Where(k => k.Id == salesmanId).FirstOrDefault();
            if (salesMan != null)
            {
                //bool targetStatus = !Convert.ToBoolean(salesMan.Flag);

                List<Cm_CustomerSalesmans> allTypeFlags = _salesmans.Where(k => k.CustomerId == salesMan.CustomerId
                                                                                    && k.CRGID == flagTypeId
                                                                                    && k.IsDeleted.Value != true).ToList();

                Cm_CustomerSalesmans salesManTrueFlag = allTypeFlags.Where(k => k.Flag == true).FirstOrDefault();

                if (allTypeFlags.Count == 1)
                {
                    salesManTrueFlag.Flag = true;
                    _uow.SaveChanges();
                    flagDto.FlagStatus = true;
                    return flagDto;
                }

                foreach (Cm_CustomerSalesmans lo in allTypeFlags)
                {
                    lo.Flag = false;
                }

                var item = _salesmans.Where(m => m.Id == salesmanId).FirstOrDefault();
                item.Flag = true;
                _uow.SaveChanges();
                flagDto.FlagStatus = true;
                return flagDto;

                //if (salesManTrueFlag == null || salesManTrueFlag.Id == flagId)
                //{
                //    var item=_salesmans.Where(m => m.Id == flagId).FirstOrDefault();
                //    item.Flag = true;
                //    _uow.SaveChanges();
                //    return true;
                //}

                //salesMan.Flag = true;
                //_uow.SaveChanges();
                //return true;
            }
            flagDto.FlagStatus = false;
            return flagDto;
        }

        public Cm_CustomerAuthenticators AuthenticatorSave(int customerId, int authLocation, string authName, string authGsm,
            string authPhone, string authFax, string authEmail, string authTitle, int workingmode, int uid)
        {
            try
            {
                authName = authName.ToUpper();
                authTitle = authTitle.ToUpper();

                if (workingmode == 0)
                {
                    Cm_CustomerAuthenticators auth = _authenticators.Where(k => k.Id == customerId).FirstOrDefault();
                    // customerId has been authenticatorId when it updates
                    Cm_CustomerAuthenticators newHslYetkili = new Cm_CustomerAuthenticators();
                    if (auth != null)
                    {
                        //auth.YETKILI_LOKID = authLocation;
                        auth.CustomerLocationId = authLocation;
                        auth.Name = authName;
                        auth.Gsm = authGsm;
                        auth.Phone1 = authPhone;
                        auth.Fax = authFax;
                        auth.Email = authEmail;
                        auth.Title = authTitle;
                        auth.ModifierId = uid;
                        auth.ModifyDate = DateTime.Now;
                        auth.IsDeleted = false;

                        newHslYetkili.Id = auth.Id;
                        newHslYetkili.CustomerLocationId = authLocation;
                        newHslYetkili.Name = authName;
                        newHslYetkili.Gsm = authGsm;
                        newHslYetkili.Phone1 = authPhone;
                        newHslYetkili.Fax = authFax;
                        newHslYetkili.Email = authEmail;
                        newHslYetkili.Title = authTitle;
                        newHslYetkili.ModifierId = uid;
                        newHslYetkili.ModifyDate = DateTime.Now;
                        newHslYetkili.IsDeleted = false;

                        _uow.SaveChanges();

                        #region #DFS HSL_YETKILI -> update

                        HSL_YETKILI hyetkili = _dContext.HSL_YETKILI.Where(k => k.OneId == auth.Id).FirstOrDefault();
                        if (hyetkili != null)
                        {
                            hyetkili.YETKILI_ADI = auth.Name;
                            hyetkili.YETKILI_EMAIL = auth.Email;
                            hyetkili.YETKILI_FAX = auth.Fax;
                            hyetkili.YETKILI_GSM = auth.Gsm;
                            hyetkili.YETKILI_LOKID = GetHslLokasyon(auth.CustomerLocationId);
                            hyetkili.YETKILI_TEL1 = auth.Phone1;
                            hyetkili.YETKILI_TEL2 = auth.Phone2;
                            hyetkili.YETKILI_UNVAN = auth.Title;
                            hyetkili.UpdateDate = DateTime.Now;
                            hyetkili.UpdateUser = GetDFSUserSet_Id(uid);
                            _dContext.SaveChanges();
                        }

                        #endregion #DFS HSL_YETKILI -> update

                        return newHslYetkili;
                    }
                }
                else if (workingmode == 1)
                {
                    Cm_CustomerAuthenticators aut = new Cm_CustomerAuthenticators()
                    {
                        CustomerLocationId = authLocation,
                        Name = authName,
                        Gsm = authGsm,
                        Phone1 = authPhone,
                        Fax = authFax,
                        Email = authEmail,
                        Title = authTitle,
                        CustomerId = customerId,
                        CreatorId = uid,
                        CreateDate = DateTime.Now,
                        IsDeleted = false
                    };
                    _authenticators.Insert(aut);
                    _uow.SaveChanges();

                    #region #DFS HSL_YETKILI -> Insert

                    HSL_CARI hcr = _dContext.HSL_CARI.Where(k => k.OneId == customerId).FirstOrDefault();
                    if (hcr != null)
                    {
                        HSL_YETKILI hyetkili = new HSL_YETKILI()
                        {
                            YETKILI_ADI = aut.Name,
                            YETKILI_EMAIL = aut.Email,
                            YETKILI_FAX = aut.Fax,
                            YETKILI_GSM = aut.Gsm,
                            YETKILI_LOKID = GetHslLokasyon(aut.CustomerLocationId),
                            YETKILI_TEL1 = aut.Phone1,
                            YETKILI_TEL2 = aut.Phone2,
                            YETKILI_UNVAN = aut.Title,
                            MasterId = hcr.Id,
                            OneId = aut.Id,
                            CreateUser = GetDFSUserSet_Id(uid),
                            CreateDate = DateTime.Now
                        };
                        _dContext.HSL_YETKILI.Add(hyetkili);
                        _dContext.SaveChanges();
                    }

                    #endregion #DFS HSL_YETKILI -> Insert

                    return aut;
                }
            }
            catch (Exception ex)
            {
                string innerMes = "";
                if (ex.InnerException != null)
                    innerMes = ex.InnerException.Message;
                Cm_CustomerAuthenticators errYet = new Cm_CustomerAuthenticators()
                {
                    Name = ex.Message + " innerException: " + innerMes
                };
                return errYet;
            }
            return null;
        }

        private int? GetHslLokasyon(int? autCustomerLocationId)
        {
            try
            {
                if (autCustomerLocationId != null)
                {
                    var item = _dContext.HSL_LOKASYON.Where(m => m.OneId == autCustomerLocationId).FirstOrDefault();
                    if (item != null)
                    {
                        return item.Id;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        public IQueryable<Gn_User> GetUserIfAreaDirectors()
        {
            List<Int32> drids = _departmentRules.Where(k => k.RuleId == 2).Select(m => m.Id).ToList();
            List<Int32?> ndrids = new List<int?>();
            foreach (int d in drids)
                ndrids.Add((Int32?)d);
            List<Int32?> gur = _userRoles.Where(k => ndrids.Contains(k.DepartmentRuleId)).Select(m => m.UserId).ToList();
            return _users.Where(k => gur.Contains(k.Id)).AsQueryable();
        }

        public IQueryable<Gn_User> GetUserIfSaleEngineers()
        {
            List<Int32> drids = _departmentRules.Where(k => k.RuleId == 2).Select(m => m.Id).ToList();
            List<Int32?> ndrids = new List<int?>();
            foreach (int d in drids)
                ndrids.Add((Int32?)d);
            List<Int32?> gur = _userRoles.Where(k => ndrids.Contains(k.DepartmentRuleId)).Select(m => m.UserId).ToList();
            return _users.Where(k => gur.Contains(k.Id)).AsQueryable();
        }

        public IQueryable<Gn_Category> GetUserSalesmanTypes(int uid)
        {
            //Gn_User user = _users.Where(k => k.Id == uid).FirstOrDefault();
            //List<int?> catIds = _salesmanCategoryGroupUsers.Where(k => k.UserId == uid).Select(m => m.CRGID).ToList();
            List<Gn_Category> ccs = _category.All().ToList();
            //_salesmanCategoryRightGroups.Where(k => catIds.Contains(k.Id)).ToList();
            List<Gn_Category> ssts = new List<Gn_Category>();
            foreach (Gn_Category ss in ccs)
            {
                Gn_Category sst = new Gn_Category()
                {
                    Id = ss.Id,
                    Title = ss.Title
                };
                ssts.Add(sst);
            }
            return ssts.AsQueryable();
        }

        private List<Gn_DepartmentRoles> GetGroupIds(int uid)
        {
            return _departmentRules.Query($@"select * from Gn_DepartmentRoles where Id in
															  (select DepartmentRuleId from Gn_UserRoles where UserId = {uid})").ToList();
        }

        public IQueryable<Gn_Category> GetUserSalesmanTypesRoles_Old(int uid)
        {
            List<Gn_Category> ccs = _category.All().ToList();
            List<Gn_Category> ssts = new List<Gn_Category>();
            foreach (Gn_Category ss in ccs)
            {
                Gn_Category sst = new Gn_Category
                {
                    Id = ss.Id,
                    Title = ss.Title
                };
                ssts.Add(sst);
            }
            return ssts.AsQueryable();
        }

        public IQueryable<Gn_Category> GetUserSalesmanTypesRoles(int uid)
        {
            //iay kullaniciyla butun op geliyor cunku departmentrules de null deger var.
            //ebru.caglar da linda ve platform geliyor.
            List<Gn_Category> ccs;
            bool? isAdmin = _users.Where(m => m.Id == uid).FirstOrDefault()?.IsAdmin;
            if (isAdmin != null && (bool)isAdmin)
            {
                ccs = _category.All().ToList();
            }
            else
            {
                var gnDepartmentRoles = GetGroupIds(uid);
                if (gnDepartmentRoles.Count(m => m.GroupId == null) > 0)
                {
                    ccs = _category.All().ToList();
                }
                else //1001279
                {
                    ccs = _category.Query($@"select * from Gn_Category where Id in(select GroupId from Gn_DepartmentRoles where Id in
																			(select DepartmentRuleId from Gn_UserRoles where UserId = {uid}))").ToList();
                }
            }

            List<Gn_Category> ssts = new List<Gn_Category>();
            foreach (Gn_Category ss in ccs)
            {
                Gn_Category sst = new Gn_Category()
                {
                    Id = ss.Id,
                    Title = ss.Title
                };
                ssts.Add(sst);
            }
            return ssts.AsQueryable();
        }

        public List<Gn_User> GetSalesmanListForAreaAndOperationTypeForLoad(int uid)
        {
            /**[TEST] iay kullaniciyla butun op geliyor cunku departmentrules de null deger var.
			   ebru.caglar da linda ve platform geliyor **/
            List<int> userIds = new List<int>();

            //getoperation
            List<Gn_Category> gnCatList;
            bool? isAdmin = _users.Where(m => m.Id == uid).FirstOrDefault()?.IsAdmin;
            if (isAdmin != null && (bool)isAdmin)//adminse butun kaategori
            {
                gnCatList = _category.All().ToList();
                userIds = GetSalesmansByOperationTypeId(gnCatList.Select(m => m.Id).ToArray()).Select(k => k.Id).ToList();
            }
            else
            {
                var gnDepartmentRoles = GetGroupIds(uid);
                if (gnDepartmentRoles.Count(m => m.GroupId == null) > 0)
                {
                    gnCatList = _category.All().ToList();
                    userIds = GetSalesmansByOperationTypeId(gnCatList.Select(m => m.Id).ToArray()).Select(k => k.Id).ToList();
                }
                else //1001279
                {
                    #region sql

                    string strSql = $@"
declare @UserId int = {uid}
if((select count(*) from Gn_UserRoles where UserId=@UserId and AreaId is null) <= 0)
begin
	   select distinct UserId,AreaId,DepartmentRuleId,Id  from Gn_UserRoles where DepartmentRuleId in(
			 select Id from Gn_DepartmentRoles where RuleId = 2 and GroupId in  (select Id from Gn_Category where Id in(select GroupId from Gn_DepartmentRoles where Id in
(select DepartmentRuleId from Gn_UserRoles where UserId = @UserId)))
	   )
	   and AreaId in (select AreaId from Gn_UserRoles where UserId=@UserId)
end
else
begin
	   select distinct UserId,AreaId,DepartmentRuleId,Id  from Gn_UserRoles where DepartmentRuleId in(
			 select Id from Gn_DepartmentRoles where RuleId = 2 and  GroupId in  (select Id from Gn_Category where Id in(select GroupId from Gn_DepartmentRoles where Id in
		(select DepartmentRuleId from Gn_UserRoles where UserId = @UserId)))
	   )
end
";

                    #endregion sql

                    userIds = _userRoles.Query(strSql).Where(m => m.UserId != null).Select(m => (int)m.UserId).ToList();
                }
            }
            return UserListWidthArea(userIds);
        }

        public IQueryable<Gn_User> GetSalesmansByOperationTypeIdForPopup(int operationId, int userId)
        {
            //verilen ornekde merve.aksoy linde 47 kullanici gelmektedir.
            string strq = $@"declare @uid  int
set @uid = {userId}
declare @operationId int
set @operationId = {operationId}

if((select count(*) from Gn_UserRoles where UserId=@uid and AreaId is null) <= 0)
begin
	   select distinct UserId,AreaId,DepartmentRuleId,Id  from Gn_UserRoles where DepartmentRuleId in(
			 select Id from Gn_DepartmentRoles where RuleId = 2 and GroupId = @operationId
	   )
	   and AreaId in (select AreaId from Gn_UserRoles where UserId=@uid)
end
else
begin
	   select distinct UserId,AreaId,DepartmentRuleId,Id  from Gn_UserRoles where DepartmentRuleId in(
			 select Id from Gn_DepartmentRoles where RuleId = 2 and GroupId = @operationId
	   )
end
";
            bool? isAdmin = _users.Where(m => m.Id == userId).FirstOrDefault()?.IsAdmin;
            if (isAdmin != null && (bool)isAdmin)
            {
                return GetSalesmansByOperationTypeId(operationId);
            }
            List<int> listUserId = _userRoles.Query(strq).Where(m => m.UserId != null).Select(m => (int)m.UserId).ToList();

            return UserListWidthArea(listUserId).AsQueryable();
        }

        //private List<Gn_User> GetUserListWithOperationFormat(List<int> listUserId)
        //{
        //    List<Gn_User> listUser = _users.Where(k => listUserId.Contains(k.Id)).Distinct().ToList();
        //    List<Gn_User> usrs = new List<Gn_User>();
        //    foreach (Gn_User ou in listUser)
        //    {
        //        usrs.Add(new Gn_User() { Id = ou.Id, UserName = ou.Name + " " + ou.Surname + " (" + ou.UserName + ")" });
        //    }
        //    return usrs.OrderBy(k => k.UserName).AsQueryable().ToList();

        //}
        public IQueryable<Gn_User> GetSalesmansByOperationTypeId(int otid)
        {
            List<int> drIds = _departmentRules.Where(k => k.GroupId == otid && k.RuleId == 2).Select(m => m.Id).ToList();
            List<int?> drIdsNull = new List<int?>();
            foreach (var drId in drIds)
            {
                drIdsNull.Add(drId);
            }

            List<int?> userIds =
                _userRoles.Where(k => drIdsNull.Contains(k.DepartmentRuleId))
                    .Select(m => m.UserId)
                    .Distinct()
                    .ToList();

            var usrs = UserListWidthArea(userIds);

            return usrs.AsQueryable();
        }

        private List<Gn_User> UserListWidthArea(List<int?> userRoleIds)
        {
            List<Gn_User> ous = _users.Where(k => userRoleIds.Contains(k.Id)).Distinct().ToList();
            List<Gn_User> usrs = new List<Gn_User>();
            ous = ous.OrderBy(k => k.AreaId).ToList();
            foreach (Gn_User ou in ous)
            {
                Gn_Area area = _areas.Where(k => k.Id == ou.AreaId).FirstOrDefault();
                if (area != null && usrs.Where(k => k.UserName == area.AreaName).Count() <= 0)
                {
                    usrs.Add(new Gn_User() { Id = 0, UserName = area.AreaName });
                }
                usrs.Add(new Gn_User() { Id = ou.Id, UserName = ou.Name + " " + ou.Surname + " (" + ou.UserName + ")" });
            }
            return usrs;
        }

        private List<Gn_User> UserListWidthArea(List<int> userRoleIds)
        {
            List<Gn_User> ous = _users.Where(k => userRoleIds.Contains(k.Id)).Distinct().ToList();
            List<Gn_User> usrs = new List<Gn_User>();
            ous = ous.OrderBy(k => k.AreaId).ToList();
            foreach (Gn_User ou in ous)
            {
                Gn_Area area = _areas.Where(k => k.Id == ou.AreaId).FirstOrDefault();
                if (area != null && usrs.Where(k => k.UserName == area.AreaName).Count() <= 0)
                {
                    usrs.Add(new Gn_User() { Id = 0, UserName = area.AreaName });
                }
                usrs.Add(new Gn_User() { Id = ou.Id, UserName = ou.Name + " " + ou.Surname + " (" + ou.UserName + ")" });
            }
            return usrs;
        }

        public IQueryable<Gn_User> GetSalesmansByOperationTypeId(int[] otid)
        {
            List<int?> list = new List<int?>();
            foreach (int? i in otid)
            {
                list.Add(i);
            }

            List<int> drIds = _departmentRules.Where(k => list.Contains(k.GroupId) && k.RuleId == 2).Select(m => m.Id).ToList();
            List<int?> drIdsNullable = new List<int?>();
            foreach (int k in drIds)
                drIdsNullable.Add((int?)k);
            List<int?> userRoleIds =
                _userRoles.Where(k => drIdsNullable.Contains(k.DepartmentRuleId))
                    .Select(m => m.UserId)
                    .Distinct()
                    .ToList();
            return UserListWidthArea(userRoleIds).AsQueryable();
        }

      

        public Gn_Category GetSalesmanTypeById(int salestypeId)
        {
            return _category.Where(k => k.Id == salestypeId).FirstOrDefault();
        }

        public object SalesmanSave(int customerId, int selesDirector, int salesMan, int salesType, bool salesFlag, bool salesAktivity, bool saleDeleted, int workingmode, int uid, int rowId)
        {
            try
            {
                SalesmanWraper sw = new SalesmanWraper();
                if (workingmode == 0)
                {
                    bool isOk = true;
                    var x = IsMachineparkSalesmanControlOk(salesType, 0, customerId);
                    if (Convert.ToInt32(x) > 0)
                        isOk = false;

                    if (!isOk)
                    {
                        if (onlySalesTypeChange(rowId, salesType))//operasyon tipi degismissse bu calisacak. kisi degistirginde calismicak.
                        {
                            return
                                "Dikkat! Bu kategoride daha önce makine parkı eklediğiniz için bu satıcının operasyonunu güncelleyemezsiniz!";
                        }
                    }

                    Cm_CustomerSalesmans sm = _salesmans.Where(k => k.Id == customerId).FirstOrDefault();
                    salesFlag = Convert.ToBoolean(sm.Flag);
                    sm.Flag = false;
                    sm.IsDeleted = true;
                    sm.ModifierName = uid.ToString();
                    sm.ModifyDate = DateTime.Now;
                    // customerId comes as customerId when it in insert mode otherwise in update mode it will record id
                    // then we have to get customerId from the record
                    customerId = Convert.ToInt32(sm.CustomerId);
                }

                List<Cm_CustomerSalesmans> sms =
                    _salesmans.Where(k => k.CustomerId == customerId && k.IsDeleted == false).ToList();
                if (sms.Count > 0 && salesFlag == true)
                {
                    foreach (Cm_CustomerSalesmans sm in sms)
                        if (sm.CRGID == salesType)
                            sm.Flag = false;
                }
                if (sms.Where(k => k.CRGID == salesType).Count() <= 0)
                    salesFlag = true; // First record according the customer must be flaged

                Cm_CustomerSalesmans sman = new Cm_CustomerSalesmans()
                {
                    CustomerId = customerId,
                    SalesmanId = salesMan,
                    Flag = salesFlag,
                    IsActive = salesAktivity,
                    IsDeleted = saleDeleted,
                    CRGID = salesType,
                    CreatorName = uid.ToString(),
                    CreateDate = DateTime.Now
                };
                _salesmans.Insert(sman);
                _uow.SaveChanges();
                sw.Id = sman.Id;
                sw.IsActive = sman.IsActive;
                sw.IsDeleted = sman.IsDeleted;
                sw.SalesmanId = sman.SalesmanId;
                sw.SalesmanTypeId = sman.CRGID;
                Gn_User u = _users.Where(k => k.Id == salesMan).FirstOrDefault();
                sw.Name = u.Name;
                //sw.IsAreaDirector = Convert.ToBoolean(u.IsAreaDirector);
                sw.Type = _category.Where(k => k.Id == salesType).FirstOrDefault().Title;
                sw.Flag = salesFlag;
                return sw;
            }
            catch (Exception ex)
            {
                string innerMes = "Dikkat! : err : ";
                if (ex.InnerException != null)
                    innerMes += ex.InnerException.Message;

                return innerMes;
            }
        }

        /// <summary>
        /// gelen yeni kaydin operasyonuna bakar. esitse true doner
        /// </summary>
        /// <param name="rowId"></param>
        /// <param name="salesType"></param>
        /// <returns></returns>
        private bool onlySalesTypeChange(int rowId, int salesType)
        {
            var salesmanrow = _salesmans.Where(m => m.Id == rowId).FirstOrDefault();

            if (salesmanrow.CRGID != salesType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IQueryable<Cm_MachineparkYear> GetYears()
        {
            return _years.All().AsQueryable();
        }

        public IQueryable<Cm_MachineparkOwnership> GetOwnerships()
        {
            return _ownerships.All().AsQueryable();
        }

        /*   public MachineparkWrapper MachineparkSave(int customerId, int categoryId, int markId, string modelName,
               string serialNo, int year,
               DateTime? saleDate, int mpCount, int ownerShip, int mplocation, int workingmode, int uid,
               DateTime? releaseDate, DateTime? planedReleaseDate)
           {
               try
               {
                   modelName = modelName.ToUpper();
                   serialNo = serialNo.ToUpper();

                   MachineparkWrapper sw = new MachineparkWrapper();

                   if (workingmode == 1)
                   {
                       // int realCustomerId =
                       //      _machineParks.Where(m => m.Id == customerId).Select(k => k.CustomerId).FirstOrDefault();
                       int result = Convert.ToInt32(IsMachineparkSalesmanControForMachinepark(categoryId, customerId));
                       if (result <= 0)
                       {
                           sw.ErrMsg =
                               "Bu kategoride makine parkı girebilmeniz için, aynı kategoride bir satıcı tanımlaması yapmanız gerekmektedir.";
                           sw.Id = -1;
                           return sw;
                       }
                   }

                   Cm_CustomerMachineparks sm = _machineParks.Where(k => k.Id == customerId).FirstOrDefault();
                   // customerId has been authenticatorId when it updates
                   if (sm != null)
                   {
                       sm.UpdateUserId = uid;
                       sm.UpdateDate = DateTime.Now;
                       sm.IsActive = false;
                       sm.IsDeleted = true;
                   }
                   if (workingmode == 0)
                       customerId = sm.CustomerId;

                   Gn_User u = _users.Where(k => k.Id == uid).FirstOrDefault();
                   Cm_CustomerLocations lok = _locations.Where(k => k.Id == mplocation).FirstOrDefault();
                   if (lok != null)
                       sw.LocationName = lok.Name;
                   Cm_MachineparkOwnership owns = _ownerships.Where(k => k.Id == ownerShip).FirstOrDefault();
                   if (owns != null)
                       sw.OwnerName = owns.Name;
                   Cm_MachineparkCategory cat = _categories.Where(k => k.Id == categoryId).FirstOrDefault();
                   if (cat != null)
                       sw.CategoryName = cat.CategoryName;
                   Cm_MachineparkMark mrk = _marks.Where(k => k.Id == markId).FirstOrDefault();
                   if (mrk != null)
                       sw.MarkName = mrk.MarkName;

                   sw.CustomerId = customerId;
                   sw.CreatorName = u.UserName;
                   sw.CategoryId = categoryId;
                   sw.MarkId = markId;
                   sw.Model = modelName;
                   sw.SerialNo = serialNo;
                   sw.ManufactureYear = year;
                   sw.SaleDate = saleDate;
                   sw.ReleaseDate = releaseDate;
                   sw.PlanedReleaseDate = planedReleaseDate;
                   sw.Quantity = mpCount;
                   sw.OwnerId = ownerShip;
                   sw.LocationId = mplocation;
                   sw.IsDeleted = false;

                   int refId = 0;
                   if (sm != null)
                       refId = sm.Id;

                   Cm_CustomerMachineparks sman = new Cm_CustomerMachineparks()
                   {
                       CustomerId = customerId,
                       CategoryId = categoryId,
                       MarkId = markId,
                       //Model = modelName,
                       SerialNo = serialNo,
                       ManufactureYear = year,
                       SaleDate = saleDate,
                       ReleaseDate = releaseDate,
                       PlanedReleaseDate = planedReleaseDate,
                       Quantity = mpCount,
                       OwnerId = ownerShip,
                       LocationId = mplocation,
                       CreateUserId = uid,
                       CreateDate = DateTime.Now,
                       IsActive = true,
                       IsDeleted = false,
                       RefRowId = refId
                   };
                   _machineParks.Insert(sman);
                   if (sm != null)
                       sw.Id = sm.Id; // will be remove from frontend if exist..
                   else sw.Id = sman.Id;

                   _uow.SaveChanges();
                   return sw;
               }
               catch (Exception ex)
               {
                   string innerMes = "";
                   if (ex.InnerException != null)
                       innerMes = ex.InnerException.Message;
                   MachineparkWrapper errMPark = new MachineparkWrapper()
                   {
                       CategoryName = ex.Message + " innerException: " + innerMes
                   };
                   return errMPark;
               }
           }
           */

        public IQueryable<MachineparkWrapper> GetMachineparksById(int customerId)
        {
            List<MachineparkWrapper> mpws = new List<MachineparkWrapper>();
            List<Cm_CustomerMachineparks> sms =
                _machineParks.Where(k => k.CustomerId == customerId && k.IsDeleted != true && k.RequestId == null).ToList();
            foreach (Cm_CustomerMachineparks sm in sms)
            {
                if (sm.LocationId == null)
                    sm.LocationId = 0;

                MachineparkWrapper mpw = new MachineparkWrapper();
                int uid = Convert.ToInt32(sm.CreateUserId);
                Gn_User u = _users.Where(k => k.Id == uid).FirstOrDefault();
                if (sm.LocationId > 0)
                {
                    Cm_CustomerLocations lok = _locations.Where(k => k.Id == sm.LocationId).FirstOrDefault();
                    if (lok != null)
                        mpw.LocationName = lok.Name;
                }
                if (sm.OwnerId > 0)
                {
                    Cm_MachineparkOwnership owns = _ownerships.Where(k => k.Id == sm.OwnerId).FirstOrDefault();
                    if (owns != null)
                        mpw.OwnerName = owns.Name;
                }
                if (sm.CategoryId > 0)
                {
                    Cm_MachineparkCategory cat =
                        _categories.Where(k => k.Id == sm.CategoryId).FirstOrDefault();
                    if (cat != null)
                        mpw.CategoryName = cat.CategoryName;
                }
                if (sm.MarkId > 0)
                {
                    Cm_MachineparkMark mrk = _marks.Where(k => k.Id == sm.MarkId).FirstOrDefault();
                    if (mrk != null)
                        mpw.MarkName = mrk.MarkName;
                }

                mpw.Id = sm.Id;
                mpw.CustomerId = sm.CustomerId;
                //mpw.CreatorName = u.UserName;
                mpw.CategoryId = sm.CategoryId;
                mpw.MarkId = sm.MarkId;
                // mpw.Model = sm.Model;
                mpw.SerialNo = sm.SerialNo;
                mpw.ManufactureYear = sm.ManufactureYear;
                mpw.SaleDate = sm.SaleDate;
                mpw.Quantity = sm.Quantity;
                mpw.OwnerId = sm.OwnerId;
                mpw.LocationId = Convert.ToInt32(sm.LocationId);
                mpw.IsDeleted = sm.IsDeleted;
                mpw.ReleaseDate = sm.ReleaseDate;
                // mpw.PlanedReleaseDate = sm.PlanedReleaseDate;
                mpws.Add(mpw);
            }

            return mpws.AsQueryable();
        }

        public object DeleteMachinePark(int rowId, int uid)
        {
            var row = _machineParks.Where(k => k.Id == rowId).FirstOrDefault();
            int customerId = row.CustomerId;
            int activeCustomerMachineparkCount =
                _machineParks.Where(k => k.CustomerId == customerId && k.IsDeleted != true).Count();
            if (activeCustomerMachineparkCount == 1)
                return "Aktif tek park kaldığı için bu kayıt silinemez..";
            row.IsDeleted = true;
            row.IsActive = false;
            row.UpdateUserId = uid;
            row.UpdateDate = DateTime.Now;
            _uow.SaveChanges();
            return row.Id;
        }

        public string[] GetPossibleCustomers(string key)
        {
            var keyValue = KeyHasId(key);
            IEnumerable<Cm_Customer> enumCustomer;

            if (keyValue.Item1)
            {
                var hslCari = _dContext.HSL_CARI.FirstOrDefault(m => m.Id == keyValue.Item2);

                if (hslCari != null)
                {
                    enumCustomer = _customer.Where(m => m.Id == keyValue.Item2 || m.Id == hslCari.OneId.Value);
                }
                else
                {
                    enumCustomer = _customer.Where(m => m.Id == keyValue.Item2);
                }
            }
            else
            {
                //normal arama
                key = key.ToUpper();
                enumCustomer = _customer.Where(k => k.IsDeleted.Value != true &&
                                         (k.Name.Contains(key)
                                          || k.NetsisHaselCode.Contains(key)
                                          || k.NetsisRentliftCode.Contains(key)
                                          || k.TaxNumber.Contains(key)
                                         ));
            }

            List<string> sbts = enumCustomer.Select(
                     m =>
                         m.Name + "|" + m.Id + "|" + m.NetsisHaselCode + "|" + m.NetsisRentliftCode + "|" +
                         m.StatusId + "|" + m.ShortName + "|" + m.TaxOffice + "|" + m.TaxNumber + "|" + m.SectorId)
                 .Take(50).ToList();

            List<string> sbts2 = new List<string>();
            if (!keyValue.Item1)
            {
                sbts2 = _dContext.HSV_CASABIT.Where(
                           k => k.CARI_ISIM.Contains(key) || k.CARI_KOD.Contains(key) || k.VERGI_NUMARASI.Contains(key))
                       .Select(
                           m =>
                               m.CARI_ISIM + "|" + m.CARI_KOD + "|" + m.CARI_ADRES + "|" + m.CARI_IL + "|" +
                               m.CARI_ILCE + "|" + m.CARI_TEL + "|" + m.CKOD + "|" + m.FAX + "|" + m.VERGI_DAIRESI +
                               "|" +
                               m.VERGI_NUMARASI)
                       .Take(50)
                       .ToList();
            }

            sbts.AddRange(sbts2);
            if (sbts.Count > 0)
            {
                return sbts.ToArray();
            }

            return new[] { "Bulunamadı | 0 | 0" };
        }

        private static Tuple<bool, int> KeyHasId(string key)
        {
            string smallKey = key.Trim().ToLower();
            if (smallKey.Length > 3)
            {
                if (smallKey.Substring(0, 3) == "id:")
                {
                    int resultint;
                    if (int.TryParse(smallKey.Substring(3, smallKey.Length - 3), out resultint))
                    {
                        return new Tuple<bool, int>(true, resultint);
                    }
                }
            }
            return new Tuple<bool, int>(false, 0);
        }

        public string IsApplicableCustomer(int customerId, int uid, bool isAuthenticated)
        {
            // the user has an authority to have applying this customer
            string result = "Kayıt onaylama yetkisi gerekmektedir";

            if (isAuthenticated)
                result = "";
            // the record is ready to apply
            var loc = _locations.Where(k => k.CustomerId == customerId).FirstOrDefault();
            if (loc == null)
                result = "<br>Lokasyon tanımı yapılmalı";
            var se = _salesmans.Where(k => k.CustomerId == customerId).FirstOrDefault();
            if (se == null)
                result += "<br>Satış Mühendisi tanımı yapılmalı";
            var mp = _machineParks.Where(k => k.CustomerId == customerId).FirstOrDefault();
            if (mp == null)
                result += "<br>Makine Parkı tanımı yapılmalı";

            if (!_locations.Where(k => k.CustomerId == customerId && !((bool)k.IsDeleted) && (bool)k.IsFat).Any())
            {
                result += "<br>Bir adet fatura adresi olmalı";
            }
            return result;
        }

        public bool IsEditableAndInsertableCustomerForYou(int customerId, int uid, List<Int32?> relatedIds)
        {
            // for customer manager.. will always be true.
            Gn_UserRoles isCAs = _userRoles.Where(k => k.UserId == uid && k.DepartmentRuleId == 78).FirstOrDefault();
            if (isCAs != null)
                return true;

            //Gn_User you = _users.Where(k => k.Id == uid).FirstOrDefault();
            List<int?> smans = _salesmans.Where(k => k.CustomerId == customerId && k.IsDeleted != true).Select(m => m.SalesmanId).ToList();
            if (smans.Count <= 0)
                return false;

            if (smans.Contains(uid))
                return true;

            bool amIAssistantOrManager = relatedIds.Count == 0 ? false : true;
            bool amIInSameArea = false;
            if (amIAssistantOrManager == false)
                return false;
            else
            {
                var myAreaId = _userRoles.Where(k => k.UserId == uid).Select(m => m.AreaId);
                if (myAreaId.Count() == 0)
                    return true;

                if (myAreaId.Count() > 0)
                {
                    List<int?> recordedUserAreaIds =
                        _userRoles.Where(k => smans.Contains(k.UserId)).Select(m => m.AreaId).ToList();
                    foreach (int? result in myAreaId.Select(m => m).Distinct())
                    {
                        if (recordedUserAreaIds.Contains(result))
                            amIInSameArea = true;
                    }
                }
            }
            if (amIInSameArea == false)
                return false;

            List<int?> myRolIds =
                _userRoles.Where(k => k.UserId == uid).Select(m => m.DepartmentRuleId).Distinct().ToList();
            List<int?> myOperationIds = _departmentRules.Where(k => myRolIds.Contains(k.Id)).Select(m => m.GroupId).Distinct().ToList();

            if (myOperationIds.Contains(null))
                return true;

            List<int?> smanRoldIds =
                _userRoles.Where(k => smans.Contains(k.UserId)).Select(m => m.DepartmentRuleId).Distinct().ToList();
            List<int?> smanOperationIds = _departmentRules.Where(k => smanRoldIds.Contains(k.Id)).Select(m => m.GroupId).Distinct().ToList();

            bool isFound = myOperationIds.Intersect(smanOperationIds).Any();
            return isFound;
        }

        public IQueryable<Gn_Category> GetOperationsAll()
        {
            return _category.All().AsQueryable();
        }

        public bool InsertorUpdateCategoryRightDetails(int id, int CategoryId, int CRGId, bool isActive)
        {
            try
            {
                if (id > 0)
                {
                    var item = GetCategoryRightDetails().FirstOrDefault(m => m.Id == id);
                    item.CategoryId = CategoryId;
                    item.CRGId = CRGId;
                    item.IsActive = isActive;
                    _categoryDetails.Update(item);
                }
                else
                {
                    var insertItem = new Gn_CategoryDetails();
                    insertItem.CategoryId = CategoryId;
                    insertItem.CRGId = CRGId;
                    insertItem.IsActive = true;
                    insertItem.IsDeleted = false;
                    _categoryDetails.Insert(insertItem);
                }

                _uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertorUpdateCategoryRightGroups(int id, string Title)
        {
            if (id > 0)
            {
                var item = GetCategoryRightGroups().FirstOrDefault(m => m.Id == id);
                item.Title = Title;

                _category.Update(item);
            }
            else
            {
                var insertItem = new Gn_Category();
                insertItem.Title = Title;
                insertItem.IsActive = true;
                insertItem.IsDeleted = false;
                _category.Insert(insertItem);
            }

            _uow.SaveChanges();
            return true;
        }

        public IQueryable<Gn_CategoryDetails> GetCategoryRightDetails()
        {
            return _categoryDetails.All().AsQueryable();
        }

        public IQueryable<Gn_Area> GetAreasAll()
        {
            return _oneAreas.All().AsQueryable();
        }

        public IQueryable<Gn_UserRoles> GetRolesByOperationTypeId(int operationTypeId)
        {
            List<Gn_DepartmentRoles> depRoles =
                _departmentRules.Where(k => k.GroupId == operationTypeId)
                    .OrderBy(m => m.DepartmentId)
                    .OrderBy(n => n.RuleId)
                    .ToList();
            List<Int32> drIds = depRoles.Select(k => k.Id).ToList();
            List<Int32?> drIdsN = new List<int?>();
            foreach (int k in drIds)
                drIdsN.Add((Int32?)k);
            List<Gn_UserRoles> usrRules = _userRoles.Where(k => drIdsN.Contains(k.DepartmentRuleId)).ToList();

            return usrRules.AsQueryable();
        }

        public object IsMachineparkSalesmanControlOk(int categoryId, int custId, int salesmanId)
        {
            try
            {
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "CustomerMachineparkCategoriesId";
                p1.SqlDbType = SqlDbType.Int;
                p1.SqlValue = categoryId;
                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "CustomerId";
                p2.SqlDbType = SqlDbType.Int;
                p2.SqlValue = custId;
                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "SalesmanId";
                p3.SqlDbType = SqlDbType.Int;
                p3.SqlValue = salesmanId;
                SqlParameter p4 = new SqlParameter();
                p4.ParameterName = "ReturnScalar";
                p4.SqlDbType = SqlDbType.Int;
                p4.SqlValue = 0;
                p4.Direction = ParameterDirection.Output;
                SqlParameter[] pars = new SqlParameter[4];
                pars[0] = p1;
                pars[1] = p2;
                pars[2] = p3;
                pars[3] = p4;
                var xxx =
                    _uow.ExecuteSqlCommand(
                        "Cm_CustomerMachineparksalesmanControl @CustomerMachineparkCategoriesId,@CustomerId,@SalesmanId,@ReturnScalar OUTPUT",
                        pars);
                int resultConvert = 0;
                if (int.TryParse(p4.Value.ToString(), out resultConvert))
                {
                    if (resultConvert > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                return "0";

                //  return p3.Value.ToString() == "1" ? 1 : 0; birden fazla geldiginde 0 result yapiyor
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public object IsMachineparkSalesmanControForMachinepark(int categoryId, int custId)
        {
            try
            {
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "CustomerMachineparkCategoriesId";
                p1.SqlDbType = SqlDbType.Int;
                p1.SqlValue = categoryId;
                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "CustomerId";
                p2.SqlDbType = SqlDbType.Int;
                p2.SqlValue = custId;
                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "ReturnScalar";
                p3.SqlDbType = SqlDbType.Int;
                p3.SqlValue = 0;
                p3.Direction = ParameterDirection.Output;
                SqlParameter[] pars = new SqlParameter[3];
                pars[0] = p1;
                pars[1] = p2;
                pars[2] = p3;
                var xxx =
                    _uow.ExecuteSqlCommand(
                        "Cm_MachineParkSalesmanControlForMachinepark @CustomerMachineparkCategoriesId,@CustomerId,@ReturnScalar OUTPUT",
                        pars);
                int resultConvert = 0;
                if (int.TryParse(p3.Value.ToString(), out resultConvert))
                {
                    if (resultConvert > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                return "0";

                //  return p3.Value.ToString() == "1" ? 1 : 0; birden fazla geldiginde 0 result yapiyor
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static List<Gn_CategoryDetails> cachedListGnCategoryDetails = new List<Gn_CategoryDetails>();
        private static List<Gn_Category> cachedListCategory = new List<Gn_Category>();

        public string GetCatNameById(int catId)
        {
            if (cachedListGnCategoryDetails.Count == 0)
            {
                cachedListGnCategoryDetails = _categoryDetails.Where(m => true).ToList();
            }

            List<int?> crdIds = cachedListGnCategoryDetails.Where(k => k.CategoryId == catId).Select(m => m.CRGId).ToList();
            if (crdIds.Count == 0)
                return "";

            if (cachedListCategory.Count == 0)
            {
                cachedListCategory = _category.Where(m => true).ToList();
            }
            List<Gn_Category> crg = cachedListCategory.Where(k => crdIds.Contains(k.Id)).ToList();
            string titles = "";
            if (crg != null)
                foreach (Gn_Category cr in crg)
                    titles += cr.Title + ",";
            return titles;
        }

        public bool InsertorUpdateCustomerMachineparkCategories(int id, int parentId, string categoryName, bool isActive)
        {
            if (id > 0)
            {
                var item = _categories.Where(m => m.Id == id).FirstOrDefault();
                item.CategoryName = categoryName;
                item.ParentId = parentId;
                _categories.Update(item);
            }
            else
            {
                var insertItem = new Cm_MachineparkCategory();
                insertItem.CategoryName = categoryName;
                insertItem.ParentId = parentId;
                _categories.Insert(insertItem);
            }

            _uow.SaveChanges();
            return true;
        }

        public string DeleteEntity(int id, string tableName)
        {
            string isDeleteColumnname = "IsDelete";

            if (tableName == "Gn_CategoryDetails")
            {
                isDeleteColumnname = "IsDeleted";
            }

            string str = String.Format("update {0} set {2}=1 where id={1}", tableName, id, isDeleteColumnname);

            try
            {
                _uow.ExecuteSqlCommand(str);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="DeleteorRecovery"><true olursa siler false olursa geri alir/param>
        /// <returns></returns>
        public string CustomerDeleteorUndo(int customerId, int userId, bool DeleteorRecovery = true)
        {
            try
            {
                /*
					@CustomerId int ,
					@UserId int,
					@IsDelete int
				 */
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "CustomerId";
                p1.SqlDbType = SqlDbType.Int;
                p1.SqlValue = customerId;

                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "DeleteorRecovery";
                p2.SqlDbType = SqlDbType.Int;
                p2.SqlValue = DeleteorRecovery ? 1 : 0;

                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "UserId";
                p3.SqlDbType = SqlDbType.Int;
                p3.SqlValue = userId;

                var pars = new SqlParameter[3]
                {
                    p1,p2,p3
                };
                try
                {
                    _uow.ExecuteSqlCommand("Cm_CustomerDelete  @CustomerId  , @UserId , @DeleteorRecovery", pars);
                    return CariDelete(customerId, userId);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string CariDelete(int customerId, int userId)
        {
            try
            {
                /*
					@CustomerId int ,
					@UserId int,
					@IsDelete int
				 */
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "CustomerId";
                p1.SqlDbType = SqlDbType.Int;
                p1.SqlValue = customerId;

                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "UserId";
                p3.SqlDbType = SqlDbType.Int;
                p3.SqlValue = userId;

                SqlParameter[] pars =
                {
                    p1,p3
                };
                try
                {
                    _uow.ExecuteSqlCommand("Dfs_CariDelete  @CustomerId  , @UserId ", pars);
                    return "";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private int GetDFSUserSet_Id(int uid)
        {
            var item = GetListDFSUserSet(m => m.OneId == uid).FirstOrDefault();
            if (item == null)
            {
                throw new Exception($"{uid} oneid si DFSUserSet de bulunmamaktadir.");
            }
            return item.Id;
        }

        private List<DFSUserSet> GetListDFSUserSet(Expression<Func<DFSUserSet, bool>> expr)
        {
            return _dContext.DFSUserSet.Where(expr).ToList();
        }

        /*
		   private readonly IGRepository<Cm_CustomerInterviews> _CustomerInterviews;
		private readonly IGRepository<Cm_Interview> _Interview;
		 */

      
        public ServiceResponse<Gn_Notifications> GetListNotifications(NotificationsFilter filter)
        {
            try
            {
                var query = _notifications.Where(m => m.IsActive.Value);

                query = GnNotificationsesFilterBind(filter, query);

                return new ServiceResponse<Gn_Notifications>
                {
                    List = query.ToList(),
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Gn_Notifications>
                {
                    HasError = true,
                    ErrorDetail = e.Message
                };
            }
        }

        public int GetCountNotifications(NotificationsFilter filter)
        {
            var query = _notifications.Where(m => m.IsActive.Value);
            query = GnNotificationsesFilterBind(filter, query);

            return query.Count();
        }

        private static IEnumerable<Gn_Notifications> GnNotificationsesFilterBind(NotificationsFilter filter, IEnumerable<Gn_Notifications> query)
        {
            if (filter != null)
            {
                if (filter.SenderUserId != 0)
                {
                    query = query.Where(m => m.SenderUserId == filter.SenderUserId);
                }
                if (filter.Desc_Id)
                {
                    query = query.OrderByDescending(m => m.Id);
                }
            }
            return query;
        }

        /*
        public ServiceResponse<Cm_CustomerInterviews> CustomerInterviewsInsertorUpdate(Cm_CustomerInterviews entity)
        {
            if (entity.Id > 0)
            {
                var dbEnt = _customerInterviews.Where(m => m.Id == entity.Id).FirstOrDefault();

                dbEnt.CustomerId = entity.CustomerId;
                dbEnt.UserId = entity.UserId;
                dbEnt.AuthenticatorId = entity.AuthenticatorId;
                dbEnt.InterviewTypeId = entity.InterviewTypeId;
                dbEnt.InterviewDate = entity.InterviewDate;
                dbEnt.PlannedInterviewDate = entity.PlannedInterviewDate;
                dbEnt.Note = entity.Note;

                dbEnt.ModifyDate = DateTime.Now;

                dbEnt.IsDeleted = entity.IsDeleted;
                _customerInterviews.Update(dbEnt);
            }
            else
            {
                entity.IsDeleted = false;
                entity.CreateDate = DateTime.Now;
                _customerInterviews.Save(entity);
            }
            return new ServiceResponse<Cm_CustomerInterviews>()
            {
                List = new List<Cm_CustomerInterviews>()
                {
                    _customerInterviews.Where(m => m.Id == entity.Id).FirstOrDefault()
                }
            };
        }
        */

        public List<Gn_ContentTypes> GetContentTypesAll()
        {
            return _contentTypes.All().ToList();
        }

        public Gn_ContentManagement GetContentByArg(int modulId, int pageId)
        {
            return _contentManagement.Where(k => k.ModulId == modulId && k.PageId == pageId).FirstOrDefault();
        }

        public List<Gn_UserModuls> GetModuls()
        {
            return _userModuls.All().ToList();
        }

        public List<Gn_ModulsAndMenus> GetMenusByModulId(int modulId)
        {
            List<Gn_ModulsAndMenus> mmenus = new List<Gn_ModulsAndMenus>();

            List<Gn_ModulsAndMenus> mms = _modulAndMenus.Where(k => k.LinkedModulId == modulId).ToList();
            foreach (Gn_ModulsAndMenus mm in mms)
            {
                Gn_ModulsAndMenus mome = new Gn_ModulsAndMenus()
                {
                    MenuName = mm.MenuName,
                    Id = Convert.ToInt32(mm.PageId)
                };
                mmenus.Add(mome);
            }
            return mmenus;
        }

        public bool SaveCMContent(int modulId, int pageId, string content, int contentTypeId, int creatorId)
        {
            try
            {
                Gn_ContentManagement cnt = _contentManagement.Where(k => k.ModulId == modulId && k.PageId == pageId && k.ContentType == contentTypeId).FirstOrDefault();
                if (cnt == null)
                {
                    _contentManagement.Insert(new Gn_ContentManagement()
                    {
                        Content = content,
                        ContentType = contentTypeId,
                        CreateDate = DateTime.Now,
                        CreatorId = creatorId,
                        IsDeleted = false,
                        ModulId = modulId,
                        PageId = pageId
                    });
                }
                else
                {
                    cnt.ModifierId = creatorId;
                    cnt.ModifyDate = DateTime.Now;
                    cnt.Content = content;
                }

                _uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetContentIfExist(int contentTypeId, int modulId, int menuId)
        {
            var ise = _contentManagement.Where(k => k.ContentType == contentTypeId && k.ModulId == modulId && k.PageId == menuId).FirstOrDefault();
            if (ise != null)
                return ise.Content;
            else return "";
        }

        public CustomerRequestWrapper CustomerRequestInsertOrUpdate(CustomerRequestWrapper vm)
        {
            return OneMap.mapper.Map<CustomerRequestWrapper>(_CustomerRequest.Save(OneMap.mapper.Map<Cm_CustomerRequest>(vm)));
        }

        public List<Cm_CustomerRequest> GetListCustomerRequest(CustomerRequestFilter filter)
        {
            //_CustomerRequest.GetDatabase().Log = s => Debug.WriteLine(s);
            // _dContext.Database
            //var sp = new Stopwatch();
            // sp.Start();
            if (filter.CustomerId == 0 && filter.Id == 0) return _CustomerRequest.Where(m => m.Id == -1).ToList();

            IQueryable<Cm_CustomerRequest> query = _CustomerRequest.GetContext().Where(m => m.IsDeleted == filter.IsDeleted);

            if (filter.CustomerId != 0)
            {
                query = query.Where(m => m.CustomerId == filter.CustomerId);
            }
            if (filter.Id != 0)
            {
                query = query.Where(m => m.Id == filter.Id);
            }
            if (filter.OpenClose == RequestOpenCloseState.Open)
            {
                query = query.Where(m => m.ResultType == (int)eResultType.Bekliyor);
            }
            if (filter.OpenClose == RequestOpenCloseState.Close)
            {
                query = query.Where(m => m.ResultType != (int)eResultType.Bekliyor);
            }

            //sp.Stop();
            //Debug.WriteLine(sp.Elapsed.Seconds);
            //  _CustomerRequest.GetPaged(, new[] { "" }, null, null, new SortExpression<Cm_CustomerRequest>(m => m.Id, ListSortDirection.Ascending));

            //return query.Select(Ex.CreateNewStatement<Cm_CustomerRequest>("Id,CustomerId")).ToList();
            return query.ToList();
            //   var a = query.Where(m => m.Id > -1).Select(Ex.CreateNewStatement<Cm_CustomerRequest>("Id,CustomerId")).ToList();
        }

        public IQueryable<Gn_Combos> GetCombos(int comboId)
        {
            return _combos.Where(k => k.ComboTypeId == comboId).AsQueryable();
        }

     

    }
}

public static class IQueryableExtensions
{
    public static string ToTraceQuery<T>(this IQueryable<T> query)
    {
        ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

        var result = objectQuery.ToTraceString();
        foreach (var parameter in objectQuery.Parameters)
        {
            var name = "@" + parameter.Name;
            var value = "'" + parameter.Value.ToString() + "'";
            result = result.Replace(name, value);
        }

        return result;
    }

    public static string ToTraceString<T>(this IQueryable<T> query)
    {
        ObjectQuery<T> objectQuery = GetQueryFromQueryable(query);

        var traceString = new StringBuilder();

        traceString.AppendLine(objectQuery.ToTraceString());
        traceString.AppendLine();

        foreach (var parameter in objectQuery.Parameters)
        {
            traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
        }

        return traceString.ToString();
    }

    private static System.Data.Entity.Core.Objects.ObjectQuery<T> GetQueryFromQueryable<T>(IQueryable<T> query)
    {
        var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
        var internalQuery = internalQueryField.GetValue(query);
        var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
        return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery<T>;
    }
}