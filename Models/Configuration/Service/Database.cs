using Microsoft.EntityFrameworkCore;
using System;

namespace api.Models.Configuration.Service
{
    public class Database
    {
        /** connection string */
        public Models.Database.PSQL.Main? PSQL { get; set; }


        /** web application database configuration */
        public Database(WebApplicationBuilder inWebBuilder)
        {
            this.PSQL = new Models.Database.PSQL.Main(inWebBuilder);
            inWebBuilder.Services.AddDbContext<Models.Database.PSQL.MainDbContext>(options => options.UseNpgsql(inWebBuilder.Configuration.GetValue<string>("ConnectionStrings:PSQL")));
        }
    }
}
