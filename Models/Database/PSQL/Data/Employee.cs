namespace api.Models.Database.PSQL.Data
{
    public class Employee
    {
        public int? id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? birthdate { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? department { get; set; }
        public string? job_title { get; set; }
        public int? salary { get; set; }
        public DateTime? start_date { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? rank { get; set; }
        public string? rule { get; set; }
    }
}
