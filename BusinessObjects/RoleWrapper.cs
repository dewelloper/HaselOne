using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class RoleWrapper
    {
        string _departmentName = "";
        int _departmentId = 0;
        string _groupName = "";
        int _groupId = 0;
        string _ruleName = "";
        int _ruleId = 0;
        int _dgoId = 0;
        string _fullName = "";

        public string DepartmentName
        {
            get
            {
                return _departmentName;
            }

            set
            {
                _departmentName = value;
            }
        }

        public int DepartmentId
        {
            get
            {
                return _departmentId;
            }

            set
            {
                _departmentId = value;
            }
        }

        public string GroupName
        {
            get
            {
                return _groupName;
            }

            set
            {
                _groupName = value;
            }
        }

        public int GroupId
        {
            get
            {
                return _groupId;
            }

            set
            {
                _groupId = value;
            }
        }

        public string RuleName
        {
            get
            {
                return _ruleName;
            }

            set
            {
                _ruleName = value;
            }
        }

        public int RuleId
        {
            get
            {
                return _ruleId;
            }

            set
            {
                _ruleId = value;
            }
        }

        public int DgoId
        {
            get
            {
                return _dgoId;
            }

            set
            {
                _dgoId = value;
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }

            set
            {
                _fullName = value;
            }
        }
    }
}
