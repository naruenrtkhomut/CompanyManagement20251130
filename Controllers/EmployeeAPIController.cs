using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace api.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        private readonly Models.Database.PSQL.MainDbContext psqlDb;
        private readonly ILogger logger;
        public EmployeeAPIController(Models.Database.PSQL.MainDbContext inPSQLDb, ILogger<EmployeeAPIController> inLogger)
        {
            this.psqlDb = inPSQLDb;
            this.logger = inLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await psqlDb.employee.ToListAsync());
        }

        [HttpGet("get-version")]
        public async Task<IActionResult> GetVersion()
        {
            // Fix CS8602: Check for nulls before dereferencing
            var prcAdmin = Program.Service?.DatabaseService?.PSQL?.PrcAdmin;
            JObject getData = prcAdmin != null
                ? await prcAdmin.Execute(1000)
                : new JObject();

            // IDE0028: Use collection initializer for JObject
            int result = int.TryParse((getData["result"] ?? "").ToObject<string>() ?? "", out result) ? result : 0;
            string message = (getData["message"] ?? "").ToObject<string>() ?? "";
            return Ok(new { result, message });
        }
    }
}
