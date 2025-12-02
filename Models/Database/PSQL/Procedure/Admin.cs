using Newtonsoft.Json.Linq;
using Npgsql;
using System.Threading.Tasks;

namespace api.Models.Database.PSQL.Procedure
{
    public class Admin
    {
        /** store procedure admin */
        private NpgsqlConnection? Connection { get; set; }
        private string? CommandString { get; set; }
        private string? ConnectionString { get; set; }


        /** web application store procedure admin configuration */
        public Admin(NpgsqlConnection? inConn, string? inConnectionString)
        {
            this.ConnectionString = inConnectionString;
            this.CommandString = "CALL public.prc_admin(@mode, @in_data, null)";
            this.Connection = inConn;
        }

        /** execute store procedure */
        public async Task<JObject> Execute(int inMode, JObject? inData = null)
        {
            JObject getData = new JObject();
            getData["result"] = 0;
            getData["message"] = "Database connection disconnected";
            if (this.Connection == null) this.Connection = new NpgsqlConnection(this.ConnectionString);
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    await this.Connection.OpenAsync();
                }
                catch (Exception getError)
                {
                    getData["message"] = $"Connection error: {getError.Message}";
                    return getData;
                }
            }
            try
            {

                NpgsqlCommand comm = new NpgsqlCommand(this.CommandString, this.Connection);
                comm.Parameters.Add(new NpgsqlParameter("@mode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = inMode });
                if (inData == null) comm.Parameters.Add(new NpgsqlParameter("@in_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = DBNull.Value });
                else comm.Parameters.Add(new NpgsqlParameter("@in_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = inData.ToString() });
                NpgsqlDataReader reader = await comm.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    try
                    {
                        if (!reader.IsDBNull(0)) getData = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(reader.GetString(0)) ?? new JObject()
                        {
                            { "result", 0 },
                            { "message", "No query data" }
                        };
                    }
                    catch (Exception getConvertError)
                    {
                        getData["message"] = $"Convert error: {getConvertError.Message}";
                    } 
                }
                await reader.CloseAsync();
                await reader.DisposeAsync();
                await comm.DisposeAsync();
            }
            catch (Exception getError)
            {
                getData["message"] = $"Command error: {getError.Message}";
            }
            return getData;
        }
    }
}
