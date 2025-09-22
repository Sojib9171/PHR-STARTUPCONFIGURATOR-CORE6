using HBS.ImplementationAutomationConsole.Core.BLL.IServices;
using HBS.ImplementationAutomationConsole.Core.BLL.Services;
using HBS.ImplementationAutomationConsole.Core.DataAccess.Data;
using HBS.ImplementationAutomationConsole.Core.DataAccess.IRepository;
using HBS.ImplementationAutomationConsole.Core.DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HBS.ImplementationAutomationConsole.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services)
        {
            services.AddScoped<ILoginServices, LoginServices>();
            services.AddScoped<IDashboardServices, DashboardServices>();
            services.AddScoped<IExcelDataServices, ExcelDataServices>();
            services.AddScoped<ITemplateServices, TemplateServices>();
            services.AddScoped<IValidationServices, ValidationServices>();
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<ISignOffService, SignOffService>();
            services.AddScoped<IWizardDataServices, WizardDataServices>();
            services.AddScoped<IBaseServices, BaseServices>();
            services.AddScoped<ISignOffService, SignOffService>();
            services.AddScoped<ICommonConfigServices, CommonConfigServices>();
            services.AddScoped<IConfigControlServices, ConfigControlServices>();
            services.AddScoped<ISectionEnableDisableServices, SectionEnableDisableServices>();
            services.AddScoped<ISidebarServices, SidebarServices>();
            services.AddScoped<IHomeServices, HomeServices>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IExcelDataRepository, ExcelDataRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IValidationRepository, ValidationRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<ISignOffRepository, SignOffRepository>();
            services.AddScoped<IWizardDataRepository, WizardDataRepository>();
            services.AddScoped<ICommonConfigRepository, CommonConfigRepository>();
            services.AddScoped<IConfigControlRepository, ConfigControlRepository>();
            services.AddScoped<ISectionEnableDisableRepository, SectionEnableDisableRepository>();
            services.AddScoped<ISidebarRepository, SidebarRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();

            services.AddScoped<IBaseDataAccess, BaseDataAccess>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
    }
}