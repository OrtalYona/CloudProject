using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.HttpContext;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Operations.MessageSequence;
using RawRabbit.Operations.StateMachine;
using RawRabbit.Instantiation;
using RawRabbit.Enrichers.MessageContext.Context;

using StackExchange.Redis;
using CloudProject.Helpers;





namespace CloudProject
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
           // services.AddMvc();
                       services
            .AddRawRabbit(new RawRabbitOptions {
                ClientConfiguration = 
                    RawRabbit.Common
                    .ConnectionStringParser.Parse("pibniaca:WbFVWQe0JkUyudLCMwrlmKVEZd08SlV7@golden-kangaroo.rmq.cloudamqp.com/pibniaca"),
                Plugins = p => p.UseGlobalExecutionId().UseMessageContext<MessageContext>()
            })
            .AddMvc();

                        services
            .AddSingleton<IRedisConnectionFactory,RedisConnectionFactory>()
            .AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
