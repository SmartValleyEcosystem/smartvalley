using IcoLab.Common.Web.WebApi;
using IcoLab.Web.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Application;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Contract;
using SmartValley.WebApi.ExceptionHandler;
using SmartValley.WebApi.WebApi;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartValley.WebApi
{
    public class Startup
    {
        private const string CorsPolicyName = "SVPolicy";
        
        private readonly IHostingEnvironment _currentEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
        {
            Configuration = configuration;
            _currentEnvironment = currentEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ReSharper disable once UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureOptions(Configuration, typeof(SiteOptions), typeof(ContractOptions));

            ConfigureCorsPolicy(services);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "SmartValley API", Version = "v1"}); });

            services.AddAuthentication(options =>
                                       {
                                           options.DefaultAuthenticateScheme = EcdsaAuthenticationOptions.DefaultScheme;
                                           options.DefaultChallengeScheme = EcdsaAuthenticationOptions.DefaultScheme;
                                       })
                    .AddScheme<EcdsaAuthenticationOptions, EcdsaAuthenticationHandler>(EcdsaAuthenticationOptions.DefaultScheme, options => { });

            services.AddSingleton<EthereumMessageSigner>();
            services.AddSingleton<IEtherManagerContractService, EtherManagerContractService>();

            services.AddMvc(options =>
                            {
                                options.Filters.Add(new AppErrorsExceptionFilter());
                                options.Filters.Add(new ModelStateFilter());
                            });

            var builder = new DbContextOptionsBuilder<AppDBContext>();
            builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            var dbOptions = builder.Options;
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient(x => AppDBContext.CreateEditable(dbOptions));
            services.AddTransient(x => AppDBContext.CreateReadOnly(dbOptions));
            services.AddTransient<TeamMemberRepository, TeamMemberRepository>();
            services.AddTransient<ApplicationRepository, ApplicationRepository>();
            services.AddTransient<ProjectRepository, ProjectRepository>();
            services.AddTransient<IApplicationService, ApplicationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(CorsPolicyName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartValley API V1"); });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
                       {
                           routes.MapSpaFallbackRoute(
                               name: "spa-fallback",
                               defaults: new {controller = "Home", action = "Index"});
                       });
        }

        private void ConfigureCorsPolicy(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var siteOptions = sp.GetService<SiteOptions>();
            var url = siteOptions.BaseUrl;
            if (!_currentEnvironment.IsProduction())
            {
                url = "*";
            }
            var corsPolicyBuilder = new CorsPolicyBuilder();
            corsPolicyBuilder.WithOrigins(url);
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowAnyMethod();
            corsPolicyBuilder.WithExposedHeaders(Headers.XEthereumAddress, Headers.XSignedText, Headers.XSignature);
            corsPolicyBuilder.AllowCredentials();

            services.AddCors(options => { options.AddPolicy(CorsPolicyName, corsPolicyBuilder.Build()); });
        }
    }
}