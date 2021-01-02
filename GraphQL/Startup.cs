using System;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeDoTakeawayAPI.GraphQL.Basket;
using WeDoTakeawayAPI.GraphQL.Basket.Mutations;
using WeDoTakeawayAPI.GraphQL.Ingredient;
using WeDoTakeawayAPI.GraphQL.Item;
using WeDoTakeawayAPI.GraphQL.Menu;
using WeDoTakeawayAPI.GraphQL.Menu.DataLoaders;
using WeDoTakeawayAPI.GraphQL.Model;
using WeDoTakeawayAPI.GraphQL.Order;
using WeDoTakeawayAPI.GraphQL.Section;
using WeDoTakeawayAPI.GraphQL.Section.DataLoaders;

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
                    .AddTypeExtension<ItemQueries>()
                    .AddTypeExtension<MenuQueries>()
                    .AddTypeExtension<OrderQueries>()
                    .AddTypeExtension<SectionQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<AddBasketItemMutations>()
                    .AddTypeExtension<ClearBasketMutations>()
                    .AddTypeExtension<RemoveBasketItemMutations>()
                    .AddTypeExtension<UpdateBasketItemMutations>()
                    .AddTypeExtension<AddOrderMutation>()
                .AddType<BasketType>()
                .AddType<ItemType>()
                .AddType<MenuType>()
                .AddType<OrderType>()
                .AddType<SectionType>()
                .AddDataLoader<IngredientByIdDataLoader>()
                .AddDataLoader<ItemByIdDataLoader>()
                .AddDataLoader<MenuByIdDataLoader>()
                .AddDataLoader<SectionByIdDataLoader>()
                .AddFiltering()
                .AddSorting();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("*").AllowAnyHeader();
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
