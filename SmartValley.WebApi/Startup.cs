using System.Collections.Generic;
using System.IO;
using IcoLab.Web.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nethereum.JsonRpc.IpcClient;
using Nethereum.Signer;
using Nethereum.Web3;
using SmartValley.Application;
using SmartValley.Application.AzureStorage;
using SmartValley.Application.Contracts;
using SmartValley.Application.Contracts.Options;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Application.Contracts.SmartValley.Application.Contracts;
using SmartValley.Application.Contracts.Votings;
using SmartValley.Application.Email;
using SmartValley.Application.Templates;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Admin;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Estimates;
using SmartValley.WebApi.ExceptionHandler;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Scoring;
using SmartValley.WebApi.ScoringApplications;
using SmartValley.WebApi.Users;
using SmartValley.WebApi.Votings;
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
            services.ConfigureOptions(Configuration, typeof(NethereumOptions), typeof(AuthenticationOptions), typeof(SiteOptions), typeof(SmtpOptions), typeof(AzureStorageOptions), typeof(ScoringOptions));

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

            services.AddSingleton<InitializationService>();
            services.AddSingleton(provider => InitializeWeb3(provider.GetService<NethereumOptions>().RpcAddress));
            services.AddSingleton<IClock, UtcClock>();
            services.AddSingleton<EthereumMessageSigner>();
            services.AddSingleton<EthereumClient>();
            services.AddSingleton<EthereumContractClient>();
            services.AddSingleton<MailService>();
            services.AddSingleton<MailTokenService>();
            services.AddSingleton<MailSender>();
            services.AddSingleton<ITemplateProvider, TemplateProvider>();
            services.AddSingleton<ProjectTeamMembersStorageProvider>();
            services.AddSingleton<ApplicationTeamMembersStorageProvider>();
            services.AddSingleton<ExpertApplicationsStorageProvider>();
            services.AddSingleton<ProjectStorageProvider>();
            services.AddSingleton<ITokenContractClient, TokenContractClient>(
                provider => new TokenContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().TokenContract));
            services.AddSingleton<IVotingSprintContractClient, VotingSprintContractClient>(
                provider => new VotingSprintContractClient(provider.GetService<EthereumContractClient>(),
                                                           provider.GetService<NethereumOptions>().VotingSprintContract,
                                                           provider.GetService<ITokenContractClient>()));
            services.AddSingleton<IVotingManagerContractClient, VotingManagerContractClient>(
                provider => new VotingManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().VotingManagerContract));
            services.AddSingleton<IScoringContractClient, ScoringContractClient>(
                provider => new ScoringContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().ScoringContract));
            services.AddSingleton<IEtherManagerContractClient, EtherManagerContractClient>(
                provider => new EtherManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().EtherManagerContract));
            services.AddSingleton<IScoringManagerContractClient, ScoringManagerContractClient>(
                provider => new ScoringManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().ScoringManagerContract));
            services.AddSingleton<IScoringExpertsManagerContractClient, ScoringExpertsManagerContractClient>(
                provider => new ScoringExpertsManagerContractClient(provider.GetService<EthereumContractClient>(), provider.GetService<NethereumOptions>().ScoringExpertsManagerContract));

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
            services.AddTransient<IEstimateRepository, EstimateRepository>();
            services.AddTransient<IVotingService, VotingService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IEstimationService, EstimationService>();
            services.AddTransient<IScoringService, ScoringService>();
            services.AddTransient<IScoringCriterionRepository, ScoringCriterionRepository>();
            services.AddTransient<IVotingService, VotingService>();
            services.AddTransient<IVotingRepository, VotingRepository>();
            services.AddTransient<IVotingProjectRepository, VotingProjectRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IExpertRepository, ExpertRepository>();
            services.AddTransient<IExpertService, ExpertService>();
            services.AddTransient<IExpertApplicationRepository, ExpertApplicationRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IExpertApplicationRepository, ExpertApplicationRepository>();
            services.AddTransient<IProjectTeamMemberRepository, ProjectTeamMemberRepository>();
            services.AddTransient<IScoringApplicationRepository, ScoringApplicationRepository>();
            services.AddTransient<IScoringApplicationQuestionsRepository, ScoringApplicationQuestionsRepository>();
            services.AddTransient<IScoringApplicationService, ScoringApplicationService>();

            var serviceProvider = services.BuildServiceProvider();
            var siteOptions = serviceProvider.GetService<SiteOptions>();

            ConfigureCorsPolicy(services, siteOptions);

            var service = serviceProvider.GetService<InitializationService>();
            service.InitializeAsync().Wait();
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
            var url = siteOptions.Root;
            if (!_currentEnvironment.IsProduction())
            {
                url = "*";
            }

            var corsPolicyBuilder = new CorsPolicyBuilder();
            corsPolicyBuilder.WithOrigins(url);
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowAnyMethod();
            corsPolicyBuilder.WithExposedHeaders(Headers.XNewAuthToken,
                                                 Headers.XNewRoles,
                                                 Headers.XEthereumAddress,
                                                 Headers.XSignature,
                                                 Headers.XSignedText);
            corsPolicyBuilder.AllowCredentials();

            services.AddCors(options => { options.AddPolicy(CorsPolicyName, corsPolicyBuilder.Build()); });
        }

        private static Web3 InitializeWeb3(string rpcAddress)
        {
            if (!string.IsNullOrEmpty(rpcAddress))
                return new Web3(rpcAddress);

            var ipcClient = new IpcClient("./geth.ipc");
            return new Web3(ipcClient);
        }
    }
}