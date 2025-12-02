using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Security.Claims;

namespace api.Controllers
{
    [Route("api/admin")]
    [EnableRateLimiting("FastPolicy")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private readonly Models.Database.PSQL.MainDbContext psqlDb;
        private readonly ILogger<AdminAPIController> logger;
        private readonly Models.Configuration.Main? WebService = Program.Service;
        public AdminAPIController(Models.Database.PSQL.MainDbContext inPSQLDb, ILogger<AdminAPIController> inLogger)
        {
            this.psqlDb = inPSQLDb;
            this.logger = inLogger;
        }

        /** admin login */
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> GetLogin([FromBody] Models.Data.UserLogin? inUserLogin)
        {
            if (inUserLogin == null) return Ok(new { result = 0, message = "No user login" });
            if (this.WebService == null) return Ok(new { result = 0, message = "No web appliction service" });
            // Fix CS8602: Check for nulls before dereferencing
            var prcAdmin = this.WebService.DatabaseService?.PSQL?.PrcAdmin;
            JObject getData = prcAdmin != null
                ? await prcAdmin.Execute(1001, new JObject()
                {
                    { "username", this.WebService.EncryptionService.GetValue(inUserLogin.Username, this.logger) },
                    { "password", this.WebService.EncryptionService.GetValue(inUserLogin.Password, this.logger) }
                })
                : new JObject();

            // IDE0028: Use collection initializer for JObject
            int result = int.TryParse((getData["result"] ?? "").ToObject<string>() ?? "", out result) ? result : 0;
            string message = (getData["message"] ?? "").ToObject<string>() ?? "";
            var JWTService = this.WebService.JWTService;
            if (JWTService == null) return Ok(new { result = 0, message = "No JWT service" });
            if (result == 1) message = JWTService.GetToken(inUserLogin.Username, message, logger);
            return Ok(new { result, message });
        }






        /** getting postgresql server version */
        //[Authorize(Roles = "admin")]
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("postgresql/server-version")]
        public async Task<IActionResult> GetPSQL_SERVER_VERSION()
        {
            if (this.WebService == null) return Ok(new { result = 0, message = "No web appliction service" });
            // Fix CS8602: Check for nulls before dereferencing
            var prcAdmin = this.WebService.DatabaseService?.PSQL?.PrcAdmin;
            if (prcAdmin == null) return Ok(new { result = 0, message = "No database service" });
            JObject getData = await prcAdmin.Execute(1000);

            // IDE0028: Use collection initializer for JObject
            int result = int.TryParse((getData["result"] ?? "").ToObject<string>() ?? "", out result) ? result : 0;
            string message = (getData["message"] ?? "").ToObject<string>() ?? "";
            return Ok(new { result, message });
        }
    }
}
