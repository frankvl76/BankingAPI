using APITemplateCore.Configurations.Swagger;
using APITemplateCore.Data.Context;
using AutoMapper;
using Banking.BusinessServices.TransactionServices;
using Banking.Data.Configuration;
using Banking.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Linq;

namespace APITemplateCore
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

        public Startup(IHostingEnvironment hostingEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnv.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnv.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _hostingEnv = hostingEnv;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });
            // Add framework services.
            services.AddMvc();
            // Add Automapper 
            services.AddAutoMapper();
            // Add versioning 
            services.AddApiVersioning(o => o.AssumeDefaultVersionWhenUnspecified = true);

            services.Configure<MongoDbSettings>(options => Configuration.GetSection("MongoDbSettings").Bind(options));

            // Configure named auth policies that map directly to OAuth2.0 scopes
            services.AddAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "Banking Tools API",
                        Description = "The API for the banking tools by FR",
                        TermsOfService = ""
                    }
                );

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });

                c.OperationFilter<RemoveVersionParameters>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.DocumentFilter<SetVersionInPaths>();
            });

            services.ConfigureSwaggerGen(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath(PlatformServices.Default.Application));
            });

            services.AddScoped<DbContext, ApiContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IBankingToolsContext, BankingToolsContext>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IFixedTransactionService, FixedTransactionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env,
                              ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/myapp-{Date}.txt", LogLevel.Warning);

            var identityServerSection = Configuration.GetSection("IdentityServer");
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = identityServerSection.GetValue<string>("Url"),
                RequireHttpsMetadata = true,
                ApiName = identityServerSection.GetValue<string>("ApiName"),
                AutomaticAuthenticate = true
            });

            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "V1 Docs");
            });
        }

        private string GetXmlCommentsPath(ApplicationEnvironment appEnvironment)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var xmlPath = Path.Combine(basePath, "banking.xml");
            return xmlPath;
        }
    }
}
