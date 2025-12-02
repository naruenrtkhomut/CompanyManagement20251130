using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Models.Configuration.Service
{
    public class JWT
    {
        /** JWT key configuration */
        private string? SecretKey { get; set; }
        private string? Issue { get; set; }
        private string? Audience { get; set; }
        private bool Error { get; set; }


        /** web application JWT configuration */
        public JWT(WebApplicationBuilder inWebBuilder)
        {
            Console.WriteLine("Loading secret key, issues and audience (JWT)");
            this.SecretKey = inWebBuilder.Configuration.GetValue<string>("JWT:SecretSupperKey");
            this.Issue = inWebBuilder.Configuration.GetValue<string>("JWT:Issues");
            this.Audience = inWebBuilder.Configuration.GetValue<string>("JWT:Audience");

            /** checking jwt key, issues and audiences */
            this.Error = string.IsNullOrEmpty(this.SecretKey) || string.IsNullOrEmpty(this.Issue) || string.IsNullOrEmpty(this.Audience);
            if (string.IsNullOrEmpty(this.SecretKey))
            {
                Console.WriteLine("JWT secret key is empty");
                return;
            }
            if (string.IsNullOrEmpty(this.Issue))
            {
                Console.WriteLine("JWT issue value is empty");
                return;
            }
            if (string.IsNullOrEmpty(this.Audience))
            {
                Console.WriteLine("JWT audience value is empty");
                return;
            }
            this.Add(inWebBuilder);
        }

        /** adding jwt sevice */
        private void Add(WebApplicationBuilder inWebBuilder) 
        {
            if (string.IsNullOrEmpty(this.SecretKey)) return;
            inWebBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issue,
                    ValidAudience = this.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey))
                };
            });
            inWebBuilder.Services.AddAuthorization();
            Console.WriteLine("Added JWT service success");
        }

        /** get token */
        public string GetToken(string? inUsername, string? inRole, ILogger inLogger)
        {
            if (this.Error)
            {
                inLogger.LogError("JWT no service");
                return string.Empty;
            }

            inUsername = (inUsername ?? "").Trim();
            inRole = (inRole ?? "").Trim();
            if (string.IsNullOrEmpty(inUsername))
            {
                inLogger.LogError("No username for generate token");
                return string.Empty;
            }

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, inUsername),
                new Claim(ClaimTypes.Role, inRole)
            };
#pragma warning disable CS8604
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));
#pragma warning restore CS8604
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken newToken = new JwtSecurityToken(
                issuer: Issue,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            string token = new JwtSecurityTokenHandler().WriteToken(newToken);
            inLogger.LogInformation("Got new token");
            return token;
        }
    }
}
