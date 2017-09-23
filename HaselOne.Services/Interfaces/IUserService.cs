using BusinessObjects;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaselOne.Services.Interfaces
{
    public interface IUserService
    {
        UserKnowledge LoginUser(string name, string password);
        IQueryable<Gn_User> GetUsers();
        IQueryable<Gn_User> GetUsersLast20();
        IQueryable<Gn_User> GetUsersWhere(string criteria);
        IQueryable<Gn_User> GetUsersWhere(string criteria, int pageSkip, int pageSize);
        Gn_User GetUserById(int id);
        IQueryable<Gn_Category> GetUserGroups();
        IQueryable<Gn_Category> GetCategoryRightGroups();
        //IQueryable<One_AuthenticatorPositions> GetPositionsAll();
        Gn_User SaveUser(int userId, string userGroup, string name, string surname, string username, int level, string email, string position, string phone, string gsm, bool aktivepassivemi, int workingmode,
            string fax, int branchCode, string mainArea, string subArea, string businessGroup, string department, bool areaDirector, bool salesman, int uid, string fileName, bool isadmin, string password);
        bool DeleteUser(int userId);
        bool UpdateUserImage(int userId, string imagePath);
        bool ChangeActivePassiveUser(int userId);

        //User Groups
        IQueryable<Gn_Category> GetUsersLast20UG();
        IQueryable<Gn_Category> GetUsersWhereUG(string criteria);
        IQueryable<Gn_Category> GetUsersWhereUG(string criteria, int pageSkip, int pageSize);
        bool ChangeActivePassiveUserUG(int userGroupId);
        Gn_Category GetUserGroupById(int id);
        Gn_Department GetUserDepartmentById(int id);
        Gn_Role GetUserRuleById(int id);
        bool SaveUserGroup(int userGroupId, string userGroupName, bool aktivepassivemi, int workingmode);

        //User Moduls
        IQueryable<Gn_UserModuls> GetModuls();
        IQueryable<Gn_UserModuls> GetUsersLast20Moduls();
        IQueryable<Gn_UserModuls> GetUsersWhereModuls(string criteria);
        IQueryable<Gn_UserModuls> GetUsersWhereModuls(string criteria, int pageSkip, int pageSize);
        bool ChangeActivePassiveUserModuls(int userGroupId);
        Gn_UserModuls GetUserModulById(int id);
        bool SaveModul(int modulId, string modulName, bool aktivepassivemi, int workingmode);

        //Controls
        IQueryable<Gn_Control> GetControls();
        IQueryable<Gn_Control> GetUsersLast20Controls();
        IQueryable<Gn_Control> GetUsersWhereControls(string criteria);
        IQueryable<Gn_Control> GetUsersWhereControls(string criteria, int pageSkip, int pageSize);
        bool ChangeActivePassiveControlsEnable(int controlId);
        bool ChangeActivePassiveControlsVisible(int controlId);
        Gn_Control GetControlById(int id);
        bool SaveControl(int modulId, int pageId, string controlId, string controlText, string controlTextEng, string controlTypeName, bool isEnable, bool isVisible, int workingmode);

        //User Group Rights
        IQueryable<Gn_UserGroupRights> GetUsersLast20GroupRights();
        IQueryable<Gn_UserGroupRights> GetUsersWhereGroupRights(string criteria);
        IQueryable<Gn_UserGroupRights> GetUsersWhereGroupRights(string criteria, int pageSkip, int pageSize);
        bool ChangeActivePassiveUserUGAActivity(int controlId);
        bool ChangeActivePassiveUserUGADelete(int controlId);
        Gn_UserGroupRights GetGroupRightsById(int id);
        bool SaveUserGroupRight(int userGroupRightId, int userGroupId, int modulId, bool uinsert, bool uedit, bool ushow, bool udelete, bool isActive, bool isdeleted, int workingmode, int uid, int departmentId, int roleId, int userGroupDetailId);

        //User Rights
        IQueryable<Gn_UserRights> GetUsersLast20UserRights();
        IQueryable<Gn_UserRights> GetUsersWhereUserRights(string criteria);
        IQueryable<Gn_UserRights> GetUsersWhereUserRights(string criteria, int pageSkip, int pageSize);
        bool ChangeActivePassiveUserUAActivity(int controlId);
        bool ChangeActivePassiveUserUADelete(int controlId);
        Gn_UserRights GetUserRightsById(int id);
        bool SaveUserRight(int userRightId, int userId, int modulId, bool uinsert, bool uedit, bool ushow, bool udelete, bool isActive, bool isdeleted, int workingmode);

        //Control Authorities
        IQueryable<Gn_ControlAuthorities> GetUsersLast20CA();
        IQueryable<Gn_ControlAuthorities> GetUsersWhereCA(string criteria);
        IQueryable<Gn_ControlAuthorities> GetUsersWhereCA(string criteria, int pageSkip, int pageSize);
        bool ChangeActivePassiveCAEnable(int controlId);
        bool ChangeActivePassiveCAVisible(int controlId);
        Gn_ControlAuthorities GetCAById(int id);
        bool SaveCA(int caId, int userId, bool isVisible, bool isEnable, int workingmode);

        IQueryable<Ns_BranchCode> GetBranchs();
        IQueryable<Gn_Area> GetMainAreas();
        IQueryable<Gn_Area> GetSubAreas();
        IQueryable<Gn_User> GetBusinessGroups();
        IQueryable<Gn_Department> GetDepartments();
        Gn_Department GetDepartmentByUserId(Int32 uid);
        IQueryable<Gn_Role> GetRoles();
        List<Gn_Role> GetRoleByUserId(Int32 uid);
        Gn_Category GetGroupByUserId(Int32 uid);

        bool UpdateUser(Gn_User user);

        List<RoleWrapper> GetRolesAll();
        List<RoleWrapper> GetRolesByUserId(int uid);
        RoleWrapper AddRole(int role, int? area, int uid);
        bool RemoveRole(int drid, int? area, int uid);

        bool UserPasswordChange(int userId, string password, string password2);
        Gn_Category GetGnCategoriById(int id);

        List<Gn_ConnectionChannel> ListConnectionChannel();


    }
}
