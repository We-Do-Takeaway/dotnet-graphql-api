using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.Migration
{
  public class ConsoleStartup
  {
    public ConsoleStartup()
    {
      var builder = new ConfigurationBuilder()
          .AddEnvironmentVariables();

      Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
      });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

    }
  }
}