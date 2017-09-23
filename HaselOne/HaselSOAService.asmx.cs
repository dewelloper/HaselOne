using BusinessObjects;
using DAL;
using HaselOne.App_Start;
using HaselOne.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using HaselOne.Util;
using Newtonsoft.Json;
using JsonSerializer = Microsoft.ApplicationInsights.Extensibility.Implementation.JsonSerializer;

namespace HaselOne
{
    /// <summary>
    /// Summary description for HaselSOAService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class HaselSOAService : BaseService<HaselSOAService>
    {

        [Dependency]
        public ICustomerService _cs { get; set; }

        [Dependency]
        public IUserService _us { get; set; }

        public HaselSOAService() : base()
        {
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string LoadRegions(string bolVal)
        {
            var res = _cs.GetRegions(bolVal).OrderBy(k => k.CityName).ToList();
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(res);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string LoadDistrict(string cityName)
        {
            var res = _cs.GetDistrictByCityName(cityName).OrderBy(k => k.RegionName).ToList();
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            return TheSerializer.Serialize(res);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRegionsByCityName(string bolVal)
        {
            JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
            var res = _cs.GetRegionsByCityName(bolVal);
            if (res != null)
            {
                res = res.OrderBy(k => k.CityName).ToList();

                return TheSerializer.Serialize(res);
            }
            return "";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveCustomer(int customerId, bool isMainCustomer, string haselStatus,
            string customerName, string customerCode, string hslShort, string taxOffice, string taxNumber,
            string sectorId, int firmType, string hslCustomerCode, string hslCustomerCodeH, string webSite, int uid)
        {
             int sectorIdInt = 0;
            if (sectorId != "" && sectorId != "0")
                sectorIdInt = Convert.ToInt32(sectorId);
            //DCH_SEKTOR sektor = _cs.GetSectors().Where(k => k.Id == sectorIdInt).FirstOrDefault();
            //if (sektor != null)
            //    sectorIdInt = sektor.Id;

            Cm_Customer cari = null;
            if (customerId == 0)
            {
                cari = new Cm_Customer()
                {
                    Name = customerName,
                    ShortName = hslShort,
                    TaxOffice = taxOffice,
                    TaxNumber = taxNumber,
                    IsHasel = firmType == 1 ? true : false,
                    NetsisHaselCode = hslCustomerCode,
                    NetsisRentliftCode = hslCustomerCodeH,
                    Web = webSite,
                    SectorId = sectorIdInt,
                    StatusId = haselStatus == "Onaylı" ? 1 : 0,
                    CreatorId = uid,
                    CreateDate = DateTime.Now
                };
                return _cs.InsertCustomer(cari, uid).ToString();
            }
            else
            {
                cari = _cs.GetCustomerById(customerId);
                cari.Name = customerName;
                cari.ShortName = hslShort;
                cari.TaxOffice = taxOffice;
                cari.TaxNumber = taxNumber;
                cari.SectorId = sectorIdInt;
                cari.IsHasel = firmType == 1 ? true : false;
                cari.NetsisRentliftCode = hslCustomerCode;
                cari.NetsisHaselCode = hslCustomerCodeH;
                cari.Web = webSite;
                cari.ModifierId = uid;
                cari.StatusId = haselStatus == "Onaylı" ? 1 : 0;
                cari.ModifyDate = DateTime.Now;
                _cs.SaveCustomer(cari, uid);
                return "updated";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object[] GetLocationByCustomerId(int customerId)
        {
            var res = _cs.GetLocationByCustomerId(customerId).ToList();
            return res.Select(m => new { m.Longitude, m.Latitude, m.Id }).ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool UpdateCustomerCoLocation(int locId, string Longitude, string Latitude)
        {
            return _cs.UpdateCustomerCoLocation(locId, Longitude, Latitude);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Cm_CustomerLocations SaveNewLocation(int customerId, string location, string region, string address, string shordtdef, string tel, string fax, string longitude,
            string latitude, bool faturakesebilirmi, int uid, bool isLocActive, bool isLocDeleted, bool workingmode)
        {
            return _cs.SaveNewLocation(customerId, location, region, address, shordtdef, tel, fax, longitude, latitude, faturakesebilirmi, uid, isLocActive, isLocDeleted, workingmode);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object LocationDelete(int locationIdTo)
        {
            return _cs.LocationDelete(locationIdTo);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SalesmanDelete(int salesmanId, int customerId, string operationType)
        {
            return _cs.SalesmanDelete(salesmanId, customerId, operationType);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool AuthDelete(int authenticatorId)
        {
            return _cs.AuthDelete(authenticatorId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeLocation(int locId)
        {
            return _cs.ChangeLocation(locId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public FlagDto ChangeSalesmanFlag(int flagId, int flagTypeId)
        {
            return _cs.ChangeSalesmanFlag(flagId, flagTypeId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public IQueryable<Gn_Category> GetUserSalesmanTypes(int uid)
        {
            return _cs.GetUserSalesmanTypes(uid);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public IQueryable<Gn_User> GetSalesmansByOperationTypeId(int operationId)
        {
            return _cs.GetSalesmansByOperationTypeId(operationId: operationId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public IQueryable<Gn_User> GetSalesmansByOperationTypeIdForPopup(int operationId, int userId)
        {
            return _cs.GetSalesmansByOperationTypeIdForPopup(operationId: operationId, userId: userId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool Logout(int valu)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.RemoveAll();
                HttpContext.Current.Session.Abandon();

            }
            return true;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveUser(int userId, string userGroup, string name, string surname, string username, int level, string email, string position, string phone, string gsm, bool aktivepassivemi, int workingmode,
            string fax, int branchCode, string mainArea, string subArea, string businessGroup, string department, bool areaDirector, bool salesman, int uid, string fileName, bool isadmin, string password)
        {
            try
            {
   var item = _us.SaveUser(userId, userGroup, name, surname, username, level, email, position, phone, gsm, aktivepassivemi, workingmode, fax, branchCode, mainArea, subArea, businessGroup, department, areaDirector, salesman, uid, fileName, isadmin, password);

            }
            catch (Exception e)
            {
                Log(e.Message,"","","");
                return e.Message;
            }

            return "ok";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool DeleteUser(int userId)
        {
            return _us.DeleteUser(userId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUser(int userId)
        {
            return _us.ChangeActivePassiveUser(userId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUserUG(int userGroupId)
        {
            return _us.ChangeActivePassiveUserUG(userGroupId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Gn_Category GetUserGroupById(int id)
        {
            return _us.GetUserGroupById(id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Gn_Category GetGnCategoriById(int id)
        {
            return _us.GetGnCategoriById(id);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveUserGroup(int userGroupId, string userGroupName, bool aktivepassivemi, int workingmode)
        {
            return _us.SaveUserGroup(userGroupId, userGroupName, aktivepassivemi, workingmode);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUserModul(int modulId)
        {
            return _us.ChangeActivePassiveUserModuls(modulId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Gn_UserModuls GetModulById(int id)
        {
            return _us.GetUserModulById(id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveModul(int modulId, string modulName, bool aktivepassivemi, int workingmode)
        {
            return _us.SaveModul(modulId, modulName, aktivepassivemi, workingmode);
        }



        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveControlsEnable(int controlId)
        {
            return _us.ChangeActivePassiveControlsEnable(controlId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveControlsVisible(int controlId)
        {
            return _us.ChangeActivePassiveControlsVisible(controlId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Gn_Control GetControlById(int id)
        {
            return _us.GetControlById(id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveControl(int modulId, int pageId, string controlId, string controlText, string controlTextEng, string controlTypeName, bool isEnable, bool isVisible, int workingmode)
        {
            return _us.SaveControl(modulId, pageId, controlId, controlText, controlTextEng, controlTypeName, isEnable, isVisible, workingmode);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUserUGADelete(int userGroupAuthId)
        {
            return _us.ChangeActivePassiveUserUGADelete(userGroupAuthId);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUserUGAActivity(int userGroupAuthId)
        {
            return _us.ChangeActivePassiveUserUGAActivity(userGroupAuthId);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Gn_UserGroupRights GetGroupRightsById(int id)
        {
            return _us.GetGroupRightsById(id);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveUserGroupRight(int userGroupRightId, int userGroupId, int modulId, bool uinsert, bool uedit, bool ushow, bool udelete, bool isActive, bool isdeleted, int workingmode, int uid, int departmentId, int roleId, int userGroupDetailId)
        {
            return _us.SaveUserGroupRight(userGroupRightId, userGroupId, modulId, uinsert, uedit, ushow, udelete, isActive, isdeleted, workingmode, uid, departmentId, roleId, userGroupDetailId);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUserUADelete(int userAuthId)
        {
            return _us.ChangeActivePassiveUserUADelete(userAuthId);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveUserUAActivity(int userAuthId)
        {
            return _us.ChangeActivePassiveUserUAActivity(userAuthId);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Gn_UserRights GetUserRightsById(int id)
        {
            return _us.GetUserRightsById(id);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveUserRight(int userRightId, int userId, int modulId, bool uinsert, bool uedit, bool ushow, bool udelete, bool isActive, bool isdeleted, int workingmode)
        {
            return _us.SaveUserRight(userRightId, userId, modulId, uinsert, uedit, ushow, udelete, isActive, isdeleted, workingmode);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveCAEnable(int caId)
        {
            return _us.ChangeActivePassiveCAEnable(caId);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool ChangeActivePassiveCAVisible(int caId)
        {
            return _us.ChangeActivePassiveCAVisible(caId);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveCA(int caId, int userId, bool isVisible, bool isEnable, int workingmode)
        {
            return _us.SaveCA(caId, userId, isVisible, isEnable, workingmode);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public Cm_CustomerAuthenticators AuthenticatorSave(int customerId, int authLocation, string authName, string authGsm, string authPhone, string authFax, string authEmail, string authTitle, int workingmode, int uid)
        {
            return _cs.AuthenticatorSave(customerId, authLocation, authName, authGsm, authPhone, authFax, authEmail, authTitle, workingmode, uid);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SalesmanSave(int customerId, int selesDirector, int salesMan, int salesType, bool salesFlag, bool salesAktivity, bool saleDeleted, int workingmode, int uid, int rowId)
        {
            return _cs.SalesmanSave(customerId, selesDirector, salesMan, salesType, salesFlag, salesAktivity, saleDeleted, workingmode, uid, rowId);
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public MachineparkWrapper MachineparkSave(int customerId, int categoryId, int markId, string modelName, string serialNo, int year, string saleDate, int mpCount, int ownerShip, int mplocation, int workingmode, int uid, string releaseDate, string planedReleaseDate)
        //{
        //    DateTime? _releaseDate = Util.Utility.StringToDatetimeForJson(releaseDate);
        //    DateTime? _planedReleaseDate = Util.Utility.StringToDatetimeForJson(planedReleaseDate);
        //    DateTime? _saleDate = Util.Utility.StringToDatetimeForJson(saleDate);

        //    return _cs.MachineparkSave(customerId, categoryId, markId, modelName, serialNo, year, _saleDate, mpCount, ownerShip, mplocation, workingmode, uid, _releaseDate, _planedReleaseDate);
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteMachinePark(int rowId, int uid)
        {
            return _cs.DeleteMachinePark(rowId, uid);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetPossibleCustomers(string key)
        {
            return _cs.GetPossibleCustomers(key);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public RoleWrapper AddRole(int role, int? area, int uid)
        {
            return _us.AddRole(role, area, uid);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool RemoveRole(int drid, int? area, int uid)
        {
            return _us.RemoveRole(drid, area, uid);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool InsertorUpdateCategoryRightDetails(int id, int categoryId, int crgId, bool isActive)
        {
            return _cs.InsertorUpdateCategoryRightDetails(id, categoryId, crgId, isActive);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool InsertorUpdateCategoryRightGroups(int id, string title)
        {
            return _cs.InsertorUpdateCategoryRightGroups(id, title);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool InsertorUpdateCustomerMachineparkCategories(int id, int parentId, string categoryName, bool isActive)
        {
            return _cs.InsertorUpdateCustomerMachineparkCategories(id, parentId, categoryName, isActive);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteEntity(int id, string tableName)
        {
            return _cs.DeleteEntity(id, tableName);
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public dynamic InterviewGetNote(int Id)
        //{
        //    var item = this._cs.GetListCustomerInterviews(new CustomerInterviewsFilter() { RowId = Id }).List[0];
        //    return new
        //    {
        //        item.Note,
        //        item.ImportantId
        //    };

        //}

       




      


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Gn_ModulsAndMenus> GetMenusByModulId(int modulId)
        {
            return _cs.GetMenusByModulId(modulId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool SaveCMContent(int modulId, int pageId, string content, int contentTypeId, int creatorId)
        {
            return _cs.SaveCMContent(modulId, pageId, content, contentTypeId, creatorId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetContentIfExist(int contentTypeId, int modulId, int menuId)
        {
            return _cs.GetContentIfExist(contentTypeId, modulId, menuId);
        }



        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public bool InterviewUpdateDeleteValidation(int userId, int interviewId)
        //{
        //    return _cs.InterviewUpdateDeleteValidation(userId, interviewId);
        //}

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Log(string em, string eu, string el, string us)
        {
            var anon = new
            {
                errorMessage = em,
                errorUrl = eu,
                errorLine = el,
                userId = us
            };

            Logger.Log(anon, true);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string TaxNumberValid(string strTaxNumber, int customerId)
        {
            return _cs.TaxNumberValid(strTaxNumber, customerId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Gn_Notifications> GetListNotifications(int userId)
        {
            var temp = _cs.GetListNotifications(new NotificationsFilter()
            {
                SenderUserId = userId
            });

            return temp.List;
        }
    }
}
