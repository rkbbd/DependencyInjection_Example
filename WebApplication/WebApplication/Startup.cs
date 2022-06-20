using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication.Model;

[assembly: OwinStartup(typeof(WebApplication.Startup))]

namespace WebApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(resolver);

            // MVC Route
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            // Set WebAPI Resolver and register
            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = resolver;

            // WebApi Route
            config.Routes.MapHttpRoute(
                name: "MyDefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
           );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            app.UseWebApi(config);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
              .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
              .Where(t => typeof(IController).IsAssignableFrom(t)
                          || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
            services.AddSingleton<IAuditsBL, AuditsBL>();
        }
    }
    public class DefaultDependencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver
    {
        protected IServiceProvider serviceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IDependencyScope BeginScope()
        {
            return new DefaultDependencyResolver(this.serviceProvider.CreateScope().ServiceProvider);
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
            IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}

