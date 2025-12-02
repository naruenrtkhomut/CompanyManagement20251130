namespace api.Models.Configuration.Service
{
    public class Cor
    {
        /** cor list configuration */
        public List<CorModel> CorList = new List<CorModel>()
        {
            new CorModel(){ Name = "AllowAngularOrigins", Link = "http://localhost:4200" }
        };


        /** web application cor configuration */
        public Cor(WebApplicationBuilder inWebBuilder)
        {
            inWebBuilder.Services.AddCors(options =>
            {
                CorList.ForEach(getCor =>
                {
                    if (!string.IsNullOrEmpty(getCor.Name) && !string.IsNullOrEmpty(getCor.Link))
                    {
                        Console.WriteLine($"Add cor: {getCor.Link}");
                        options.AddPolicy(getCor.Name, builder =>
                        {
                            builder.WithOrigins(getCor.Link)
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                        });
                    }
                    else Console.WriteLine("No cor name or link");
                });
            });
            Console.WriteLine("Web cor service success");
        }
    }

    /** web application cor model */
    public class CorModel
    {
        public string? Name { get; set; }
        public string? Link { get; set; }
    }

}
