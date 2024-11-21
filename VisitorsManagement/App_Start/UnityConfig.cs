using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using VisitorsManagement.Models.RemoteEmployee;
using VisitorsManagement.Repository;

namespace VisitorsManagement
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IGenericRepository, GenericRepository>();
            container.RegisterType<IAuthRepository, AuthRepository>();
            container.RegisterType<IVisitorsManagementRepository, VisitorsManagementRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IWPRepository, WPRepository>();
            container.RegisterType<IContractorRepository, ContractorRepository>();
            container.RegisterType<IVMReportRepository, VMReportRepository>();
            container.RegisterType<IRemoteEmployee, RemoteEmployeeRepository>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}