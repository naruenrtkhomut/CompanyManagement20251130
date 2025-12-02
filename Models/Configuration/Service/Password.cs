using System.Text.RegularExpressions;

namespace api.Models.Configuration.Service
{
    public class Password
    {
        private string? UserPasswordFormat { get; set; }

        /** web application password configuration */
        public Password(WebApplicationBuilder inWebBuilder)
        {
            this.UserPasswordFormat = @"(?=.*[@$!%*?&])(?=.*[a-z])(?=.*[A-Z]).{8,15}$";
        }


        /** generate user password */
        public string GenerateUserPassword(string? inPassword)
        {
            inPassword = (inPassword ?? "").Trim();
            if (string.IsNullOrEmpty(this.UserPasswordFormat)) return string.Empty;
            if (string.IsNullOrEmpty(inPassword)) return string.Empty;
            if (!Regex.IsMatch(inPassword, this.UserPasswordFormat)) return string.Empty;
            return inPassword;
        }
    }
}
