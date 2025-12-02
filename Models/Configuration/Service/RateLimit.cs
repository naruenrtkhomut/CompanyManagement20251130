using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace api.Models.Configuration.Service
{
    public class RateLimit
    {
        /** configuration data */
        public string? FixedLimit { get; set; }
        public string? ApiLimit { get; set; }


        /** web application rate limit configuration */
        public RateLimit(WebApplicationBuilder inWebBuilder)
        {
            this.FixedLimit = "FastPolicy";
            this.ApiLimit = "ApiPolicy";



            inWebBuilder.Services.AddRateLimiter(options =>
            {
                /** global limit */
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromSeconds(10),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }));

                /** rate limit in name policy */
                options.AddFixedWindowLimiter(this.FixedLimit, opt =>
                {
                    opt.PermitLimit = 10;
                    opt.Window = TimeSpan.FromSeconds(10);
                    opt.QueueLimit = 0;
                });

                /** too many request */
                options.OnRejected = (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.WriteAsync("Rate limit exceeded.");
                    return new ValueTask();
                };

                /** window limit */
                options.AddFixedWindowLimiter(this.ApiLimit, opt =>
                {
                    opt.PermitLimit = 10;
                    opt.Window = TimeSpan.FromSeconds(10);
                });
            });
            Console.WriteLine("Rate limit service success");
        }
    }
}
