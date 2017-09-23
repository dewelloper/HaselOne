using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserKnowledge
    {
        string _sessionId = "";
        public string SessionId
        {
            get
            {
                return _sessionId;
            }

            set
            {
                _sessionId = value;
            }
        }

        int _userId = 0;
        public int UserId
        {
            get
            {
                return _userId;
            }

            set
            {
                _userId = value;
            }
        }

        Gn_User _user = new Gn_User();
        public Gn_User User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
            }
        }

        List<Gn_ModulsAndMenus> _modulAndMenus = new List<Gn_ModulsAndMenus>();
        public List<Gn_ModulsAndMenus> ModulAndMenus
        {
            get
            {
                return _modulAndMenus;
            }

            set
            {
                _modulAndMenus = value;
            }
        }

        public List<Gn_UserGroupRights> GroupRights
        {
            get
            {
                return _groupRights;
            }

            set
            {
                _groupRights = value;
            }
        }
        List<Gn_UserGroupRights> _groupRights = new List<Gn_UserGroupRights>();

        List<Gn_UserRights> _userRights = new List<Gn_UserRights>();
        public List<Gn_UserRights> UserRights
        {
            get
            {
                return _userRights;
            }

            set
            {
                _userRights = value;
            }
        }

        List<Int32?> _roleRelatedUserIds = new List<int?>();
        public List<int?> RoleRelatedUserIds
        {
            get
            {
                return _roleRelatedUserIds;
            }

            set
            {
                _roleRelatedUserIds = value;
            }
        }

        public List<int?> UserRoles { get; set; }

        public string ImagePath = "";


    }
}
