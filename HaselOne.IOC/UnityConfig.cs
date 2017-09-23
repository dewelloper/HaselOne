using DAL;
using DAL_Dochuman;
using HaselOne.Domain.Repository;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using HaselOne.Services.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unity.Mvc5;

namespace HaselOne.Ioc
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            UnityContainer container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.BindInRequestScope<IUnitOfWork, UnitOfWork>();
            container.BindInRequestScope<ICustomerService, CustomerService>();
            container.BindInRequestScope<IInterviewService,InterviewService>();
            container.BindInRequestScope<IStatsReportService, StatsReportService>();
            container.BindInRequestScope<ICoreService, CoreService>();
            container.BindInRequestScope<IUserService, UserService>();
            container.BindInRequestScope<IMachineparkService, MachineparkService>();
            //container.BindInRequestScope<ExtendedMembershipProvider, SimpleMembershipProvider>();
            //container.BindInRequestScope<IGRepository<HSL_CARI>, GRepository<HSL_CARI>>();
            container.BindInRequestScope<IGRepository<Cm_Customer>, GRepository<Cm_Customer>>();
            container.BindInRequestScope<IGRepository<Gn_UserModuls>, GRepository<Gn_UserModuls>>();
            //container.BindInRequestScope<IGRepository<Gn_UserGroups>, GRepository<Gn_UserGroups>>();
            container.BindInRequestScope<IGRepository<Gn_UserGroupRights>, GRepository<Gn_UserGroupRights>>();
            container.BindInRequestScope<IGRepository<Gn_UserRights>, GRepository<Gn_UserRights>>();
            container.BindInRequestScope<IGRepository<Gn_ModulsAndMenus>, GRepository<Gn_ModulsAndMenus>>();
            container.BindInRequestScope<IGRepository<Gn_User>, GRepository<Gn_User>>();

            container.BindInRequestScope<IGRepository<Cm_MachineparkMark>, GRepository<Cm_MachineparkMark>>();
            //container.BindInRequestScope<IGRepository<One_CustomerSaleEngineer>, GRepository<One_CustomerSaleEngineer>>();
            container.BindInRequestScope<IGRepository<Cm_CustomerLocations>, GRepository<Cm_CustomerLocations>>();
            container.BindInRequestScope<IGRepository<Gn_AreaCity>, GRepository<Gn_AreaCity>>();
            container.BindInRequestScope<IGRepository<Gn_Area>, GRepository<Gn_Area>>();
            container.BindInRequestScope<IGRepository<Gn_Control>, GRepository<Gn_Control>>();
            container.BindInRequestScope<IGRepository<Gn_ControlAuthorities>, GRepository<Gn_ControlAuthorities>>();
            //container.BindInRequestScope<IGRepository<authe>, GRepository<One_AuthenticatorPositions>>();
            container.BindInRequestScope<IGRepository<Cm_CustomerAuthenticators>, GRepository<Cm_CustomerAuthenticators>>();
            container.BindInRequestScope<IGRepository<Cm_CustomerSalesmans>, GRepository<Cm_CustomerSalesmans>>();
            container.BindInRequestScope<IGRepository<Ns_BranchCode>, GRepository<Ns_BranchCode>>();
            container.BindInRequestScope<IGRepository<Cm_MachineparkOwnership>, GRepository<Cm_MachineparkOwnership>>();
            container.BindInRequestScope<IGRepository<Cm_MachineparkYear>, GRepository<Cm_MachineparkYear>>();
            container.BindInRequestScope<IGRepository<Cm_CustomerMachineparks>, GRepository<Cm_CustomerMachineparks>>();

            container.BindInRequestScope<IGRepository<Gn_CategoryDetails>, GRepository<Gn_CategoryDetails>>();
            container.BindInRequestScope<IGRepository<Gn_Area>, GRepository<Gn_Area>>();
            container.BindInRequestScope<IGRepository<Gn_Department>, GRepository<Gn_Department>>();
            container.BindInRequestScope<IGRepository<Gn_Role>, GRepository<Gn_Role>>();
            container.BindInRequestScope<IGRepository<Gn_DepartmentRoles>, GRepository<Gn_DepartmentRoles>>();
            container.BindInRequestScope<IGRepository<Gn_UserRoles>, GRepository<Gn_UserRoles>>();
            container.BindInRequestScope<IGRepository<Gn_RoleRelation>, GRepository<Gn_RoleRelation>>();
            container.BindInRequestScope<IGRepository<Gn_Sector>, GRepository<Gn_Sector>>();
            //todo: bu iki rep
            //container.BindInRequestScope<IGRepository<DCH_SEKTOR>, GRepository<DCH_SEKTOR>>();
            //container.BindInRequestScope<IGRepository<HSV_CASABIT>, GRepository<HSV_CASABIT>>();

            container.BindInRequestScope<IGRepository<Gn_ContentTypes>, GRepository<Gn_ContentTypes>>();
            container.BindInRequestScope<IGRepository<Gn_ContentManagement>, GRepository<Gn_ContentManagement>>();
            container.BindInRequestScope<IGRepository<Gn_Notifications>, GRepository<Gn_Notifications>>();
            container.BindInRequestScope<IGRepository<Gn_ComboTypes>, GRepository<Gn_ComboTypes>>();
            container.BindInRequestScope<IGRepository<Gn_Combos>, GRepository<Gn_Combos>>();
        }

        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }
    }
}