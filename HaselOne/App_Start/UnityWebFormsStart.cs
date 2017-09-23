using System.Web;

using Microsoft.Practices.Unity;
using Unity.WebForms;
using HaselOne.Domain.UnitOfWork;
using HaselOne.Services.Interfaces;
using HaselOne.Services.Services;
using DAL;
using HaselOne.Domain.Repository;
using DAL_Dochuman;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(HaselOne.App_Start.UnityWebFormsStart), "PostStart")]
namespace HaselOne.App_Start
{
    /// <summary>
    ///		Startup class for the Unity.WebForms NuGet package.
    /// </summary>
    internal static class UnityWebFormsStart
    {
        /// <summary>
        ///     Initializes the unity container when the application starts up.
        /// </summary>
        /// <remarks>
        ///		Do not edit this method. Perform any modifications in the
        ///		<see cref="RegisterDependencies" /> method.
        /// </remarks>
        internal static void PostStart()
        {
            IUnityContainer container = new UnityContainer();
            HttpContext.Current.Application.SetContainer(container);

            RegisterDependencies(container);
        }

        /// <summary>
        ///		Registers dependencies in the supplied container.
        /// </summary>
        /// <param name="container">Instance of the container to populate.</param>
        private static void RegisterDependencies(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICoreService, CoreService>();

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

    }

    /// <summary>
    /// Bind the given interface in request scope
    /// </summary>
    public static class IOCExtensions
    {
        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }
    }
}