using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Threading.Tasks;

namespace api.Models.Database
{
    public class POSTGRESQL
    {
        /** store procedure for database admin */
        public async Task<JObject> Prc_Admin(ILogger inLogger, int inMode, JObject? inData = null)
        {
            string getCommStr = new Models.Modeling.Database.PSQL.MAIN.PRC.ADMIN().ToString();
            return await CallProceduree(getCommStr, inLogger, inMode, inData);
        }


        /** store procedure call */
        private async Task<JObject> CallProceduree(string inCommStr, ILogger inLogger, int inMode, JObject? inData = null)
        {
            JObject getData = new JObject()
            {
                { "result", 0 },
                { "message", "Web application config is null" }
            };
            if (Program.configuration == null) return getData;
            NpgsqlConnection getConn = await Program.configuration.GetPSQLMain(inLogger);
            if (getConn.State != System.Data.ConnectionState.Open) return getData;
            try
            {
                NpgsqlCommand getComm = new NpgsqlCommand(inCommStr, getConn);
                getComm.Parameters.Add(new NpgsqlParameter("@mode", NpgsqlTypes.NpgsqlDbType.Integer) { Value = inMode });
                if (inData == null) getComm.Parameters.Add(new NpgsqlParameter("@in_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = DBNull.Value });
                else getComm.Parameters.Add(new NpgsqlParameter("@in_data", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = inData.ToString() });
                NpgsqlDataReader getReader = await getComm.ExecuteReaderAsync();
                if (await getReader.ReadAsync())
                {
                    if (!getReader.IsDBNull(0))
                    {
                        try
                        {
                            getData = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(getReader.GetString(0)) ?? getData;
                        }
                        catch (Exception getReaderError)
                        {
                            getData["message"] = getReaderError.Message;
                        }
                    }
                }
                await getReader.CloseAsync();
                await getReader.DisposeAsync();
                await getComm.DisposeAsync();
            }
            catch (Exception getCommandError)
            {
                getData["message"] = getCommandError.Message;
                return getData;
            }
            return getData;
        }
    }
}
