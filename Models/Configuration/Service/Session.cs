namespace api.Models.Configuration.Service
{
    public class Session
    {
        /** web application session configuration */
        public Session(WebApplicationBuilder inWebBuilder)
        {
            inWebBuilder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            inWebBuilder.Services.AddDistributedMemoryCache();
            Console.WriteLine("Session service success");
        }
    }
}
