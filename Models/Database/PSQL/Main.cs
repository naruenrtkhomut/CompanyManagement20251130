using Npgsql;

namespace api.Models.Database.PSQL
{
    public class Main
    {
        /** store procesures */
        public Procedure.Admin? PrcAdmin { get; set; }


        /** connections */
        private NpgsqlConnection? PSQLConn { get; set; }
        private string? PSQLConnString { get; set; }


        /** web service database of postgresql configuration */
        public Main(WebApplicationBuilder inWebBuilder)
        {
            this.PSQLConnString = inWebBuilder.Configuration.GetValue<string>("ConnectionStrings:PSQL");
            this.PSQLConn = new NpgsqlConnection(this.PSQLConnString);
            this.PrcAdmin = new Procedure.Admin(this.PSQLConn, this.PSQLConnString);
        }
    }
}
