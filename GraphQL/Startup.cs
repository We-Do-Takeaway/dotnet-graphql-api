using System;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeDoTakeawayAPI.GraphQL.Baskets;
using WeDoTakeawayAPI.GraphQL.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<ApplicationDbContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddGraphQLServer()
                .BindRuntimeType<Guid, IdType>()
                .AddTypeConverter<Guid, string>(from => from.ToString("D"))
                .AddQueryType(d => d.Name("Query"))
                .AddTypeExtension<BasketQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                .AddTypeExtension<BasketMutations>()
                .AddDataLoader<ItemByIdDataLoader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UsePlayground();
            app.UseVoyager();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
