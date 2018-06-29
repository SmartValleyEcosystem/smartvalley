using System;
using System.Collections.Generic;
using System.IO;
using IcoLab.Web.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Signer;
using Nethereum.Web3;
using NServiceBus;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;
using SmartValley.Application;
using SmartValley.Application.AzureStorage;
using SmartValley.Application.Email;
using SmartValley.Application.Templates;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Interfaces;
using SmartValley.Domain.Services;
using SmartValley.Ethereum;
using SmartValley.Ethereum.Contracts.AllotmentEvent;
using SmartValley.Ethereum.Contracts.AllotmentEventsManager;
using SmartValley.Ethereum.Contracts.EtherManager;
using SmartValley.Ethereum.Contracts.Scoring;
using SmartValley.Ethereum.Contracts.ScoringOffersManager;
using SmartValley.Ethereum.Contracts.ScoringsRegistry;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;
using SmartValley.WebApi.Admin;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Estimates;
using SmartValley.WebApi.ExceptionHandler;
using SmartValley.WebApi.Feedbacks;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Users;
using Swashbuckle.AspNetCore.Swagger;
using Headers = SmartValley.WebApi.WebApi.Headers;
using ILogger = Serilog.ILogger;
using IScoringApplicationService = SmartValley.WebApi.ScoringApplications.IScoringApplicationService;
using IScoringService = SmartValley.WebApi.Scorings.IScoringService;
using ScoringApplicationService = SmartValley.WebApi.ScoringApplications.ScoringApplicationService;
using ScoringService = SmartValley.WebApi.Scorings.ScoringService;

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
            services.ConfigureOptions(Configuration, typeof(NethereumOptions), typeof(AuthenticationOptions), typeof(SiteOptions), typeof(SmtpOptions), typeof(AzureStorageOptions));

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "SmartValley API", Version = "v1"}); });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                                  {
                                      options.RequireHttpsMetadata = _currentEnvironment.IsProduction();
                                      options.TokenValidationParameters = new TokenValidationParameters
                                                                          {
                                                                              ValidateIssuer = true,
                                                                              ValidIssuer = AuthenticationOptions.Issuer,
                                                                              ValidateAudience = true,
                                                                              ValidAudience = AuthenticationOptions.Audience,
                                                                              ValidateLifetime = false,
                                                                              IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(),
                                                                              ValidateIssuerSigningKey = true
                                                                          };
                                  });

            services.AddSingleton(provider => InitializeWeb3(provider.GetService<NethereumOptions>().RpcAddress));
            services.AddSingleton<IClock, UtcClock>();
            services.AddSingleton<EthereumMessageSigner>();
            services.AddSingleton<EthereumClient>();
            services.AddSingleton<EthereumContractClient>();
            services.AddSingleton<MailService>();
            services.AddSingleton<MailTokenService>();
            services.AddSingleton<MailSender>();
            services.AddSingleton<ITemplateProvider, TemplateProvider>(provider => new TemplateProvider(_currentEnvironment.ContentRootPath));
            services.AddSingleton(InitializeProjectTeamMembersStorageProvider);
            services.AddSingleton(InitializeApplicationTeamMembersStorageProvider);
            services.AddSingleton(InitializeExpertApplicationsStorageProvider);
            services.AddSingleton(InitializeProjectStorageProvider);
            services.AddSingleton<IScoringContractClient, ScoringContractClient>(
                provider => new ScoringContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().ScoringContract));
            services.AddSingleton<IEtherManagerContractClient, EtherManagerContractClient>(
                provider => new EtherManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().EtherManagerContract));
            services.AddSingleton<IScoringsRegistryContractClient, ScoringsRegistryContractClient>(
                provider => new ScoringsRegistryContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().ScoringsRegistryContract));
            services.AddSingleton<IScoringOffersManagerContractClient, ScoringOffersManagerContractClient>(
                provider => new ScoringOffersManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().ScoringOffersManagerContract));
            services.AddSingleton<IAllotmentEventsManagerContractClient, AllotmentEventsManagerContractClient>(
                provider => new AllotmentEventsManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().AllotmentEventsManagerContract));
            services.AddSingleton<IAllotmentEventContractClient, AllotmentEventContractClient>(
                provider => new AllotmentEventContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().AllotmentEventContract.Abi));

            services.AddMemoryCache();

            services.AddMvc(options =>
                            {
                                options.Filters.Add(new AuthenticationFilterFactory());
                                options.Filters.Add(new AppErrorsExceptionFilter());
                                options.Filters.Add(new ModelStateFilter());
                            });

            var builder = new DbContextOptionsBuilder<AppDBContext>();
            builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            var dbOptions = builder.Options;
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient(x => AppDBContext.CreateEditable(dbOptions));
            services.AddTransient(x => AppDBContext.CreateReadOnly(dbOptions));
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IScoringRepository, ScoringRepository>();
            services.AddTransient<IScoringOffersRepository, ScoringOffersRepository>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IEstimationService, EstimationService>();
            services.AddTransient<IScoringService, ScoringService>();
            services.AddTransient<IScoringCriterionRepository, ScoringCriterionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IExpertRepository, ExpertRepository>();
            services.AddTransient<IExpertService, ExpertService>();
            services.AddTransient<Experts.IExpertService, Experts.ExpertService>();
            services.AddTransient<IExpertApplicationRepository, ExpertApplicationRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IExpertApplicationRepository, ExpertApplicationRepository>();
            services.AddTransient<IScoringApplicationRepository, ScoringApplicationRepository>();
            services.AddTransient<IScoringApplicationQuestionsRepository, ScoringApplicationQuestionsRepository>();
            services.AddTransient<IScoringApplicationService, ScoringApplicationService>();
            services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IAllotmentEventService, AllotmentEventService>();
            services.AddTransient<IAllotmentEventRepository, AllotmentEventRepository>();
            services.AddTransient<IEthereumTransactionService, EthereumTransactionService>();
            services.AddTransient<IEthereumTransactionRepository, EthereumTransactionRepository>();

            var logger = CreateLogger();
            Log.Logger = logger;

            services.AddSingleton<ILogger>(logger);

            var serviceProvider = services.BuildServiceProvider();
            var siteOptions = serviceProvider.GetService<SiteOptions>();

            ConfigureCorsPolicy(services, siteOptions);

            var dataProtectionProvider = serviceProvider.GetService<IDataProtectionProvider>();
            services.AddSingleton<IMessageSession>(
                provider => EndpointConfigurator
                            .StartAsync(Configuration, _currentEnvironment.ContentRootPath, dataProtectionProvider, logger)
                            .GetAwaiter()
                            .GetResult());
        }

        private Logger CreateLogger()
        {
            return new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .CreateLogger();
        }

        private static ApplicationTeamMembersStorageProvider InitializeApplicationTeamMembersStorageProvider(IServiceProvider serviceProvider)
        {
            var storageProvider = new ApplicationTeamMembersStorageProvider(serviceProvider.GetService<AzureStorageOptions>());
            storageProvider.InitializeAsync().Wait();
            return storageProvider;
        }

        private static ProjectTeamMembersStorageProvider InitializeProjectTeamMembersStorageProvider(IServiceProvider serviceProvider)
        {
            var storageProvider = new ProjectTeamMembersStorageProvider(serviceProvider.GetService<AzureStorageOptions>());
            storageProvider.InitializeAsync().Wait();
            return storageProvider;
        }

        private static ExpertApplicationsStorageProvider InitializeExpertApplicationsStorageProvider(IServiceProvider serviceProvider)
        {
            var storageProvider = new ExpertApplicationsStorageProvider(serviceProvider.GetService<AzureStorageOptions>());
            storageProvider.InitializeAsync().Wait();
            return storageProvider;
        }

        private static ProjectStorageProvider InitializeProjectStorageProvider(IServiceProvider serviceProvider)
        {
            var storageProvider = new ProjectStorageProvider(serviceProvider.GetService<AzureStorageOptions>());
            storageProvider.InitializeAsync().Wait();
            return storageProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            app.UseCors(CorsPolicyName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartValley API V1"); });
            }

            app.UseAuthentication();
            app.Use(async (context, next) =>
                    {
                        await next();
                        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                        {
                            context.Request.Path = "/index.html";
                            await next();
                        }
                    })
               .UseDefaultFiles(new DefaultFilesOptions {DefaultFileNames = new List<string> {"index.html"}})
               .UseStaticFiles()
               .UseMvc();
        }

        private void ConfigureCorsPolicy(IServiceCollection services, SiteOptions siteOptions)
        {
            var corsPolicy = new CorsPolicyBuilder()
                             .WithOrigins(_currentEnvironment.IsProduction() ? siteOptions.Root : "*")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .WithExposedHeaders(Headers.XNewAuthToken,
                                                 Headers.XNewRoles,
                                                 Headers.XEthereumAddress,
                                                 Headers.XSignature,
                                                 Headers.XSignedText)
                             .AllowCredentials()
                             .Build();

            services.AddCors(options => { options.AddPolicy(CorsPolicyName, corsPolicy); });
        }

        private static Web3 InitializeWeb3(string rpcAddress)
            => !string.IsNullOrEmpty(rpcAddress) ? new Web3(rpcAddress) : throw new InvalidOperationException("RPC address is not specified.");
    }
}