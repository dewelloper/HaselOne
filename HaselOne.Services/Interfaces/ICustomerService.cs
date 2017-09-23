using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Helper;
using HaselOne.Domain.Repository;

namespace HaselOne.Services.Interfaces
{
    public interface ICustomerService : IServiceBase
    {
        IQueryable<Cm_Customer> GetCustomers();

        IQueryable<CustomerWrapper> GetCustomers(CustomerFilter filter);

        IQueryable<Cm_MachineparkCategory> GetCategories();

        //IQueryable<One_CustomerMachinepark> GetCustomerMP();
        IQueryable<Cm_Customer> GetCustomersWhere(string criteria);

        IQueryable<Cm_Customer> GetCustomersWhere(string criteria, int pageSkip, int pageSize);

        IQueryable<Gn_Sector> GetSectors();

        bool SaveCustomer(Cm_Customer cari, int uid);

        Cm_Customer GetCustomerById(int customerId);

        List<Cm_MachineparkMark> GetMarks();

        //List<One_CustomerSaleEngineer> GetSaleEngineersFromMachinepark();
        //List<DFSUserSet> GetSaleEngineers();
        Int32 InsertCustomer(Cm_Customer cari, int uid);

        List<Gn_Area> GetAreas();

        //List<Gn_Area> GetAreasForCitiesAll();
        List<Gn_AreaCity> GetAreasForCitiesAll();

        List<Gn_AreaCity> GetCitiesAll();

        List<Gn_AreaCity> GetCities(string kod);

        List<Gn_AreaCity> GetRegions(string il);

        List<Gn_AreaCity> GetRegionsByCityName(string ilname);

        List<Gn_AreaCityRegions> GetDistrictByCityName(string ilname);

        Cm_CustomerLocations GetLocationById(int locId);

        List<Cm_CustomerLocations> GetLocationByCustomerId(int customerId);

        List<Cm_CustomerLocations> GetLocationBy(LocationFilter filter);

        IQueryable<SalesmanWraper> GetSalesmanById(int customerId);

        List<Cm_CustomerAuthenticators> GetAuthenticatorByCustomerId(int customerId);

        IQueryable<Cm_Customer> GetCustomersLast20();

        List<CustomerWrapper> GetCustomersLast202(int page, int pageSize, string sortIndex, string sortDirection);

        bool UpdateCustomerCoLocation(int locId, string Longitude, string Latitude);

        Cm_CustomerLocations SaveNewLocation(int customerId, string location, string region, string address, string shordtdef,
            string tel, string fax, string longitude, string latitude, bool faturakesebilirmi, int uid, bool isLocActive,
            bool isLocDeleted, bool workingmode);

        object LocationDelete(int locationIdTo);

        object SalesmanDelete(int salesmanId, int customerId, string operationType);

        bool AuthDelete(int authenticatorId);

        string TaxNumberValid(string str, int customerId);

        bool ChangeLocation(int locId);

        FlagDto ChangeSalesmanFlag(int flagId, int flagTypeId);

        Cm_CustomerAuthenticators AuthenticatorSave(int customerId, int authLocation, string authName, string authGsm,
            string authPhone, string authFax, string authEmail, string authTitle, int workingmode, int uid);

        IQueryable<Gn_User> GetUserIfAreaDirectors();

        IQueryable<Gn_User> GetUserIfSaleEngineers();

        IQueryable<Gn_Category> GetUserSalesmanTypes(int uid);

        IQueryable<Gn_Category> GetUserSalesmanTypesRoles(int uid);

        IQueryable<Gn_Category> GetUserSalesmanTypesRoles_Old(int uid);

        object SalesmanSave(int customerId, int selesDirector, int salesMan, int salesType, bool salesFlag, bool salesAktivity, bool saleDeleted, int workingmode, int uid, int rowId);

        Gn_Category GetSalesmanTypeById(int salestypeId);

        Gn_User HasAreaDirector(int customerId);

        IQueryable<Cm_MachineparkYear> GetYears();

        IQueryable<Cm_MachineparkOwnership> GetOwnerships();

        IQueryable<Gn_User> GetSaleEngineersByCustomerId(int cid);

        // MachineparkWrapper MachineparkSave(int customerId, int categoryId, int markId, string modelName, string serialNo,
        //     int year,
        //     DateTime? saleDate, int mpCount, int ownerShip, int mplocation, int workingmode, int uid,
        //     DateTime? relaseDate, DateTime? planedReleaseDate);

        IQueryable<MachineparkWrapper> GetMachineparksById(int customerId);

        object DeleteMachinePark(int rowId, int uid);

        string[] GetPossibleCustomers(string key);

        string IsApplicableCustomer(int customerId, int uid, bool isAuthenticated);

        bool IsEditableAndInsertableCustomerForYou(int customerId, int uid, List<Int32?> relatedIds);

        IQueryable<Gn_User> GetSalesmansByOperationTypeId(int operationId);

        IQueryable<Gn_User> GetSalesmansByOperationTypeIdForPopup(int operationId, int userId);

        IQueryable<Gn_Category> GetOperationsAll();

        IQueryable<Gn_Area> GetAreasAll();

        IQueryable<Gn_UserRoles> GetRolesByOperationTypeId(int operationTypeId);

        object IsMachineparkSalesmanControlOk(int categoryId, int custId, int x);

        string CustomerDeleteorUndo(int customerId, int userId, bool DeleteorRecovery = true);

        bool InsertorUpdateCategoryRightDetails(int id, int categoryId, int crgId, bool isActive);

        IQueryable<Gn_CategoryDetails> GetCategoryRightDetails();

        IQueryable<Gn_Category> GetCategoryRightGroups();

        bool InsertorUpdateCategoryRightGroups(int id, string title);

        string GetCatNameById(int catId);

        bool InsertorUpdateCustomerMachineparkCategories(int id, int parentId, string categoryName, bool isActive);

        string DeleteEntity(int id, string tableName);

        //ServiceResponse<Cm_CustomerInterviews> CustomerInterviewsInsertorUpdate(Cm_CustomerInterviews entity);

        //ServiceResponse<Cm_CustomerInterviews> GetListCustomerInterviews(CustomerInterviewsFilter filter);

        //List<CustomerInterviewsWrapper> GetListCustomerInterviewsForLoad(int customerId);

        //List<Cm_Interview> GetListInterview(int id = 0, bool isDelete = false);

       // List<Gn_InterviewImportant> GetListInterviewImportant();

        List<Gn_ContentTypes> GetContentTypesAll();

        List<Gn_UserModuls> GetModuls();

        List<Gn_ModulsAndMenus> GetMenusByModulId(int modulId);

        bool SaveCMContent(int modulId, int pageId, string content, int contentTypeId, int creatorId);

        Gn_ContentManagement GetContentByArg(int modulId, int pageId);

        string GetContentIfExist(int contentTypeId, int modulId, int menuId);

        List<Gn_User> GetSalesmanListForAreaAndOperationTypeForLoad(int uid);

        //bool InterviewUpdateDeleteValidation(int userId, int customerCreateId);

        ServiceResponse<Gn_Notifications> GetListNotifications(NotificationsFilter filter);

        int GetCountNotifications(NotificationsFilter filter);

        IQueryable<Cm_MachineparkMark> GetListMachineparkMark();

        IQueryable<Pr_MachineModel> GetListMachineModel();

        CustomerRequestWrapper CustomerRequestInsertOrUpdate(CustomerRequestWrapper vm);

        List<Cm_CustomerRequest> GetListCustomerRequest(CustomerRequestFilter filter);

        new List<TEntity> GetListGeneric<TEntity>(Expression<Func<TEntity, bool>> filter = null,
            string[] includePaths = null, int? page = null, int? pageSize = null,
            params SortExpression<TEntity>[] sortExpressions) where TEntity : class, IEntity;

        IQueryable<Gn_Combos> GetCombos(int comboId);
    }
}