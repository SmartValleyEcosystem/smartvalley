using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartValley.Common
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration, params Type[] optionsTypes)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            foreach (var type in optionsTypes)
            {
                var config = Activator.CreateInstance(type);
                configuration.Bind(type.ShortDisplayName(), config);
                services.AddSingleton(type, config);
            }
        }
    }
}