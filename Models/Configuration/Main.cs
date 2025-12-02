namespace api.Models.Configuration
{
    public class Main
    {
        /** configuration service in public */
        public Service.JWT? JWTService { get; set; }
        public Service.Encryption EncryptionService { get; set; }
        public Service.Decryption DecryptionService { get; set; }
        public Service.RateLimit RateLimitService { get; set; }
        public Service.Cor CorService { get; set; }
        public Service.Password PasswordService { get; set; }
        public Service.Database DatabaseService { get; set; }


        /** configuration service in private */


        /** configuration service in private */
        private Service.Session SessionService { get; set; }

        /** web application configuration */
        public Main(WebApplicationBuilder inWebBuilder)
        {
            /** all service */
            this.JWTService = new Service.JWT(inWebBuilder);
            this.EncryptionService = new Service.Encryption(inWebBuilder);
            this.DecryptionService = new Service.Decryption(inWebBuilder);
            this.RateLimitService = new Service.RateLimit(inWebBuilder);
            this.SessionService = new Service.Session(inWebBuilder);
            this.CorService = new Service.Cor(inWebBuilder);
            this.PasswordService = new Service.Password(inWebBuilder);
            this.DatabaseService = new Service.Database(inWebBuilder);
        }
    }
}
