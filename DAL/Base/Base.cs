using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IBusinessEntity
    {
        int CreateUserId { get; set; }
        System.DateTime CreateDate { get; set; }
        Nullable<int> UpdateUserId { get; set; }
        Nullable<System.DateTime> UpdateDate { get; set; }
        Nullable<int> DeleteUserId { get; set; }
        Nullable<System.DateTime> DeleteDate { get; set; }
    }

    public partial class Gn_ControlAuthorities : IEntity { }

    public partial class Cm_MachineparkCategory : IEntity { }

    public partial class Cm_MachineparkMark : IEntity { }

    public partial class Cm_CustomerSalesmans : IEntity { }

    public partial class Gn_Department : IEntity { }

    public partial class Gn_DepartmentRoles : IEntity { }

    public partial class Cm_MachineparkOwnership : IEntity { }

    public partial class Cm_MachineparkYear : IEntity { }

    public partial class Gn_ModulsAndMenus : IEntity { }

    public partial class Gn_Notifications : IEntity { }

    public partial class Ns_BranchCode : IEntity { }

    public partial class Gn_RoleRelation : IEntity { }

    public partial class Gn_UserGroupRights : IEntity { }

    public partial class Gn_UserGroup : IEntity { }

    public partial class Gn_UserModuls : IEntity { }

    public partial class Gn_UserRights : IEntity { }

    public partial class Cm_CustomerAuthenticators : IEntity { }

    public partial class Gn_ContentTypes : IEntity { }

    public partial class Gn_AreaCityRegions : IEntity { }

    public partial class Gn_AreaCity : IEntity { }

    public partial class Pr_MachineModel : IEntity { }

    public partial class Cm_CustomerLocations : IEntity { }

    public partial class Gn_ConnectionChannel : IEntity { }

    public partial class Gn_User : IEntity { }

    public partial class Cm_CustomerInterviews : IEntity, IBusinessEntity
    {
          }


    public partial class Cm_CustomerMachineparks : IEntity, IBusinessEntity { }

    public partial class Gn_Sector : IEntity { }

    public partial class Gn_ContentManagement : IEntity { }

    public partial class Gn_UserRoles : IEntity { }

    public partial class Cm_CustomerRequest : IEntity, IBusinessEntity
    {
        public int MachineCount { get; set; }
    }

    public partial class Gn_InterviewImportant : IEntity { }

    public partial class Gn_Role : IEntity { }

    public partial class Cm_Customer : IEntity { }

    public partial class Gn_ComboTypes : IEntity { }

    public partial class Cm_Interview : IEntity { }

    public partial class Gn_Combos : IEntity { }

    public partial class Log_Cm_CustomerInterviews : IEntity { }

    public partial class Log_Cm_Customer : IEntity { }

    public partial class Log_Cm_CustomerMachineparks : IEntity { }

    public partial class Log_Cm_CustomerAuthenticators : IEntity { }

    public partial class Log_Cm_CustomerRequest : IEntity { }

    public partial class Log_Cm_CustomerSalesmans : IEntity { }

    public partial class Log_Cm_CustomerLocations : IEntity { }

    public partial class Cm_MachineparkRental : IEntity { }

    public partial class Log_Cm_MachineparkRental : IEntity { }

    public partial class Gn_Area : IEntity { }

    public partial class Gn_CategoryDetails : IEntity { }

    public partial class Gn_Segment : IEntity { }

    public partial class Gn_Category : IEntity { }

    public partial class Gn_Control : IEntity { }
}