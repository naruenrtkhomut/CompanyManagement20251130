
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace api
{
    public class Program
    {
        // depedance configuration
        //public static Models.ConfigurationOld? configuration { get; set; }
        public static Models.Configuration.Main? Service { get; set; }

        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            //configuration = new Models.ConfigurationOld(builder);
            Service = new Models.Configuration.Main(builder);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();


            app.MapControllers();
            Service.CorService.CorList.ForEach(getCor =>
            {
                if (!string.IsNullOrEmpty(getCor.Name) && !string.IsNullOrEmpty(getCor.Link))
                {
                    app.UseCors(getCor.Name);
                    Console.WriteLine($"Add cor name: {getCor.Name}");
                    Console.WriteLine($"Add cor link: {getCor.Link}");
                }
            });


            /** web application run */
            Console.WriteLine("/////////////////////////////////////////////////////////////");
            Console.WriteLine("*************************************************************");
            Console.WriteLine("                   web application on running                ");
            Console.WriteLine("*************************************************************");
            Console.WriteLine("/////////////////////////////////////////////////////////////");
            app.Run();
        }
    }
}
