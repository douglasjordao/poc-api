using System.Web.Http;
using WebActivatorEx;
using LG.POC.WebApi;
using Swashbuckle.Application;
using System;
using System.IO;
using System.Reflection;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace LG.POC.WebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Cadastro de Pedidos");
                        c.PrettyPrint();
                        c.IncludeXmlComments(GetXmlCommentsPath());
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("Cadastro de Pedidos");
                        c.DisableValidator();
                    });
        }

        private static string GetXmlCommentsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml"));
        }
    }
}
