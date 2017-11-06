using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartValley.Authentication;
using SmartValley.Common;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartValley
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
        {
            Configuration = configuration;
            _currentEnvironment = currentEnvironment;
        }

        private readonly IHostingEnvironment _currentEnvironment;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureOptions(Configuration, typeof(SiteOptions));

            ConfigureCorsPolicy(services);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "SmartValley API", Version = "v1"}); });

            services.AddAuthentication(options =>
                                       {
                                           options.DefaultAuthenticateScheme = MetamaskAuthenticationOptions.DefaultScheme;
                                           options.DefaultChallengeScheme = MetamaskAuthenticationOptions.DefaultScheme;
                                       })
                    .AddScheme<MetamaskAuthenticationOptions, MetamaskAuthenticationHandler>(MetamaskAuthenticationOptions.DefaultScheme, options => { });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var options = new RewriteOptions()
                .AddRedirectToHttps();

            app.UseRewriter(options);

            app.UseCors(CustomCorsConstants.CorsPolicyName);

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
            corsPolicyBuilder.WithExposedHeaders(CustomCorsConstants.XNewEthereumAddress, CustomCorsConstants.XNewSignature);
            corsPolicyBuilder.AllowCredentials();

            services.AddCors(options => { options.AddPolicy(CustomCorsConstants.CorsPolicyName, corsPolicyBuilder.Build()); });
        }
    }
}